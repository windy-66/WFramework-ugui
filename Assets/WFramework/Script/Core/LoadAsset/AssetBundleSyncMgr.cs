using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UObject = UnityEngine.Object;

public class AssetBundleInfo
{
    public AssetBundle m_AssetBundle;
    public int m_ReferencedCount;

    public AssetBundleInfo(AssetBundle assetBundle)
    {
        this.m_AssetBundle = assetBundle;
        this.m_ReferencedCount = 0;
    }
}

class LoadAssetRequest
{
    public Type assetType;
    public string[] assetNames;
    public Action<UObject[]> sharpFunc;
}
public class AssetBundleSyncMgr : MonoBehaviour
{
    private static AssetBundleSyncMgr _instance;

    string   m_BaseDownloadingURL = "";
    string[] m_AllManifest = null;
    AssetBundleManifest m_AssetBundleManifest = null;
    Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]>();
    Dictionary<string, List<LoadAssetRequest>> m_LoadRequests = new Dictionary<string, List<LoadAssetRequest>>();
    Dictionary<string, AssetBundleInfo> m_LoadedAssetBundles = new Dictionary<string, AssetBundleInfo>();

    public static AssetBundleSyncMgr Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new GameObject("_AssetBundleSyncMgr").AddComponent<AssetBundleSyncMgr>();
            }
            return _instance;
        }
    }

    public void Initialize(string manifestName, Action initOK)
    {
        m_BaseDownloadingURL = Util.GetRelativePath();
        LoadAsset<AssetBundleManifest>(manifestName, new string[] { "AssetBundleManifest" }, delegate (UObject[] objs) {
            if (objs.Length > 0)
            {
                m_AssetBundleManifest = objs[0] as AssetBundleManifest;
                m_AllManifest = m_AssetBundleManifest.GetAllAssetBundles();
            }
            if (initOK != null) initOK();
        });
    }

    public void LoadPrefab(string abName , string assetName, Action<UObject[]> func)
    {
        LoadAsset<GameObject>(abName, new string[] { assetName }, func);
    }
    void LoadAsset<T>(string abName, string[] assetNames, Action<UObject[]> action = null) where T : UObject
    {
        abName = GetRealAssetPath(abName);
        LoadAssetRequest request = new LoadAssetRequest();
        request.assetType = typeof(T);
        request.assetNames = assetNames;
        request.sharpFunc = action;

        List<LoadAssetRequest> requests = null;
        if (!m_LoadRequests.TryGetValue(abName, out requests))
        {
            requests = new List<LoadAssetRequest>();
            requests.Add(request);
            m_LoadRequests.Add(abName, requests);
            StartCoroutine(OnLoadAsset<T>(abName));
        }
        else
        {
            requests.Add(request);
        }

    }

    IEnumerator OnLoadAsset<T>(string abName) where T : UObject
    {
        AssetBundleInfo bundleInfo = GetLoadedAssetBundle(abName);
        if (bundleInfo == null)
        {
            yield return StartCoroutine(OnLoadAssetBundle(abName, typeof(T)));

            bundleInfo = GetLoadedAssetBundle(abName);
            if (bundleInfo == null)
            {
                m_LoadRequests.Remove(abName);
                Debug.LogError("OnLoadAsset--->>>" + abName);
                yield break;
            }
        }
        List<LoadAssetRequest> list = null;
        if (!m_LoadRequests.TryGetValue(abName, out list))
        {
            m_LoadRequests.Remove(abName);
            yield break;
        }
        for (int i = 0; i < list.Count; i++)
        {
            string[] assetNames = list[i].assetNames;
            List<UnityEngine.Object> result = new List<UnityEngine.Object>();

            AssetBundle ab = bundleInfo.m_AssetBundle;
            for (int j = 0; j < assetNames.Length; j++)
            {
                string assetPath = assetNames[j];
                AssetBundleRequest request = ab.LoadAssetAsync(assetPath, list[i].assetType);
                yield return request;
                result.Add(request.asset);

                //T assetObj = ab.LoadAsset<T>(assetPath);
                //result.Add(assetObj);
            }
       
            bundleInfo.m_ReferencedCount++;
        }
        m_LoadRequests.Remove(abName);
    }

    IEnumerator OnLoadAssetBundle(string abName, Type type)
    {
        string url = m_BaseDownloadingURL + abName;
        url = "file:///c:/luaframework/" + abName;
        WWW download = null;
        if (type == typeof(AssetBundleManifest))
            download = new WWW(url);
        else
        {
            string[] dependencies = m_AssetBundleManifest.GetAllDependencies(abName);
            if (dependencies.Length > 0)
            {
                m_Dependencies.Add(abName, dependencies);
                for (int i = 0; i < dependencies.Length; i++)
                {
                    string depName = dependencies[i];
                    AssetBundleInfo bundleInfo = null;
                    if (m_LoadedAssetBundles.TryGetValue(depName, out bundleInfo))
                    {
                        bundleInfo.m_ReferencedCount++;
                    }
                    else if (!m_LoadRequests.ContainsKey(depName))
                    {
                        yield return StartCoroutine(OnLoadAssetBundle(depName, type));
                    }
                }
            }
            download = WWW.LoadFromCacheOrDownload(url, m_AssetBundleManifest.GetAssetBundleHash(abName), 0);
        }
        yield return download;

        AssetBundle assetObj = download.assetBundle;
        if (assetObj != null)
        {
            m_LoadedAssetBundles.Add(abName, new AssetBundleInfo(assetObj));
        }
    }

    AssetBundleInfo GetLoadedAssetBundle(string abName)
    {
        AssetBundleInfo bundle = null;
        m_LoadedAssetBundles.TryGetValue(abName, out bundle);
        if (bundle == null) return null;

        // No dependencies are recorded, only the bundle itself is required.
        string[] dependencies = null;
        if (!m_Dependencies.TryGetValue(abName, out dependencies))
            return bundle;

        // Make sure all dependencies are loaded
        foreach (var dependency in dependencies)
        {
            AssetBundleInfo dependentBundle;
            m_LoadedAssetBundles.TryGetValue(dependency, out dependentBundle);
            if (dependentBundle == null) return null;
        }
        return bundle;
    }

    string GetRealAssetPath(string abName)
    {
        if (abName.Equals(AppConst.AssetDir)){
            return abName;
        }

        abName = abName.ToLower();
        if (!abName.EndsWith(AppConst.ExtName))
        {
            abName += AppConst.ExtName;
        }

        if (abName.Contains("/")){
            return abName;
        }

        for(int i = 0; i < m_AllManifest.Length; i++)
        {
            int index = m_AllManifest[i].LastIndexOf("/");
            string path = m_AllManifest[i].Remove(0, index + 1);
            if (path.Equals(abName))
            {
                return m_AllManifest[i];
            }
        }
        Debug.LogError("GetRealAssetPath Error:>>" + abName);
        return null;
    }

}
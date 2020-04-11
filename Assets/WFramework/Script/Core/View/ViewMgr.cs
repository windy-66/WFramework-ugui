  using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewMgr : MonoBehaviour
{
    private static ViewMgr _instance; 

    private SceneType CurrentSceneType = SceneType.None;

    private Dictionary<PanelName, GameObject> m_panelNameDic = new Dictionary<PanelName, GameObject>();

    private GameObject mParent;
    /// <summary>
    /// 获取资源加载实例
    /// </summary>
    /// <returns></returns>
    public static ViewMgr Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("_ViewMgr").AddComponent<ViewMgr>();
            }
            return _instance;
        }
    }

    public void SceneInit() 
    {
        mParent = GameObject.Find("Canvas");
        Transform mParentT = mParent.transform; 
    }
    private GameObject CreateLayerGameObject(string name, GameObject scene) 
    {
        GameObject layer = new GameObject(name);
        layer.transform.parent = scene.transform;
        layer.transform.localPosition(Vector3.zero).localEulerAngles(Vector3.zero).localScale(1);
        return layer;
    }
    public void SetScene(SceneType sceneType)
    {
        CurrentSceneType = sceneType;
    }
    public void SetScene(GameObject current , SceneType sceneType)
    {
        if (CurrentSceneType == sceneType) 
        {
            return;
        }
        CurrentSceneType = sceneType;
        mParent = GameObject.Find("Canvas");
        current.transform.parent = mParent.transform;
        current.transform.localPosition(Vector3.zero).localEulerAngles(Vector3.zero).localScale(1);

        //获取一个枚举的个数长度
        int nums = Enum.GetNames(typeof(LayerType)).Length;
        for (int i = 0; i < nums; i++)
        {
            //获取枚举的索引位置的值
            object obj = Enum.GetValues(typeof(LayerType)).GetValue(i);
            CreateLayerGameObject(obj.ToString(), current);
        }
    }

    public void SetLayer(GameObject current, LayerType layerType)  
    {
        Transform cuurentSceneTr = SceneMgr.Instance.scenes[CurrentSceneType].transform;
        Transform layerParent = cuurentSceneTr.Find(layerType.ToString());
        current.transform.parent = layerParent;
        current.transform.localPosition(Vector3.zero).localEulerAngles(Vector3.zero).localScale(1);
    }

    /// <summary>根据面板数组先后顺序设置深度 最后一个panel深度最高</summary>
    public void SetPanelsLayer(List<PanelBase> pbList)
    {
        for (int i = 0; i < pbList.Count; i++)
        {
            Transform[] panelArr = pbList[i].skin.GetComponentsInChildren<Transform>(true);
            for (int f = 0; f < panelArr.Length; f++)
            {
                panelArr[f].SetAsLastSibling();
            }
        }
    }

}
/// <summary>
/// layer type
/// </summary>
public enum LayerType
{
    /// <summary>弹框</summary>
    Panel = 100,
    /// <summary>提示</summary>
    Tips = 200,
    /// <summary>公告层</summary>
    Notice = 300,
}

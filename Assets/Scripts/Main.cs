/************************************************************
    文件: Main.cs
    作者: jiaxiaocheng
    功能: 游戏入口
*************************************************************/
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (AppConst.AssetbundleMode)
        {
            AssetBundleMgr.Instance.Initialize();
            SceneMgr.Instance.SwitchingScene(SceneType.LoginScene);
        }
        else
        {      
            SceneMgr.Instance.SwitchingScene(SceneType.LoginScene);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

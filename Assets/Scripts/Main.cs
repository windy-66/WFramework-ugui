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
        if (AppConst.UpdateMode)
        {
            SceneMgr.Instance.SwitchingScene(SceneType.UpdateScene);
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

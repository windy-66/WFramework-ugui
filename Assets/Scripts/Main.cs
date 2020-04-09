/************************************************************
    文件: Main.cs
    作者: jiaxiaocheng
    功能: 游戏入口
*************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneMgr.GetInstance.SwitchingScene(SceneType.LoginScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

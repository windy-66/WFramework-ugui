using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : SceneBase
{
    // Start is called before the first frame update
    protected override void OnInitSkinFront()
    {
        this.SetMainSkinPath("Scene/MainScene");
    }
    public override void Init()
    {
        base.Init();
        this.cache = true;
        this._type = SceneType.MainScene;
    }

    public override void OnShowing()
    {

    }

    protected override void OnClick(GameObject target)
    {
        switch (target.gameObject.name)
        {
            case "Btn_Panel":
                PanelMgr.GetInstance.ShowPanel(PanelName.TestPanel);
                break;
            case "Btn_login":
                // PanelMgr.GetInstance.ShowPanel(PanelName.TestPanel);
                SceneMgr.GetInstance.SwitchingScene(SceneType.LoginScene);
                break;
        }
    }
}

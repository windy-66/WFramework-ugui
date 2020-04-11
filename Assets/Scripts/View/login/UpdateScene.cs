using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateScene : SceneBase
{
    private Text m_txt;
    protected override void OnInitSkinFront()
    {
        this.SetMainSkinPath("Scene/UpdateScene");
    }

    public override void Init()
    {
        base.Init();
        this._type = SceneType.UpdateScene;
        m_txt = skinTrs.Find("messageTxt").GetComponent<Text>();
        RegisterEvent();
        UpdateMgr.Instance.Init();
    }

    private void RegisterEvent()
    {
        CEventDispatcher.Instance.addEventListener(CEventName.UPDATE_MESSAGE, UpdateMessage);
        CEventDispatcher.Instance.addEventListener(CEventName.UPDATE_FINISH, UpdateMessage);
        CEventDispatcher.Instance.addEventListener(CEventName.UPDATE_INITIALIZE, UpdateInit);

    }

    private void UpdateInit(CEvent evt)
    {
        SceneMgr.Instance.SwitchingScene(SceneType.LoginScene);
    }

    private void UpdateMessage(CEvent evt)
    {
        string message = evt.eventParams[0].ToString();
        m_txt.text += message +"\n";
    }

    private void UnRegisterEvent()
    {
        CEventDispatcher.Instance.removeEventListener(CEventName.UPDATE_MESSAGE, UpdateMessage);
        CEventDispatcher.Instance.removeEventListener(CEventName.UPDATE_FINISH, UpdateMessage);
        CEventDispatcher.Instance.removeEventListener(CEventName.UPDATE_INITIALIZE, UpdateInit);
    }

    private void OnDestroy()
    {
        UnRegisterEvent();
    }
}

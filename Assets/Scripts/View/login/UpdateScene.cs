using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UpdateScene : SceneBase
{
    private Text m_txt;
    private Text m_progressTxt; 
    private Image m_bar;
    protected override void OnInitSkinFront()
    {
        this.SetMainSkinPath("Scene/UpdateScene");
    }

    public override void Init()
    {
        base.Init();
        this._type = SceneType.UpdateScene;
        m_txt = skinTrs.Find("messageTxt").GetComponent<Text>();
        m_progressTxt = skinTrs.Find("progressTxt").GetComponent<Text>();
        m_bar = skinTrs.Find("bar").GetComponent<Image>();
        RegisterEvent();
        UpdateMgr.Instance.Init();
    }

    private void RegisterEvent()
    {
        CEventDispatcher.Instance.addEventListener(CEventName.UPDATE_CHECK, CheckUpdateMessage);
        CEventDispatcher.Instance.addEventListener(CEventName.UPDATE_MESSAGE, UpdateMessage);
        CEventDispatcher.Instance.addEventListener(CEventName.UPDATE_DONLOAD_SPEED, ShowDownloadSpeed);
        CEventDispatcher.Instance.addEventListener(CEventName.UPDATE_FINISH, UpdateFinish);
        CEventDispatcher.Instance.addEventListener(CEventName.UPDATE_PROGRESS, UpdateProgress);
    }

    private void UpdateFinish(CEvent evt)
    {
        m_bar.fillAmount = 1;
        StartCoroutine(EnterScene());
    }
    
    IEnumerator EnterScene() 
    {
        yield return new WaitForSeconds(2);
        SceneMgr.Instance.SwitchingScene(SceneType.LoginScene);
    }
    private void UpdateProgress(CEvent evt)
    {
        float  progress = (float)evt.eventParams[0];
        m_bar.fillAmount = progress;
    }

    private void ShowDownloadSpeed(CEvent evt)
    {
        string message = evt.eventParams[0].ToString();
        m_progressTxt.text = "下载速度：" + message;
    }

    private void CheckUpdateMessage(CEvent evt)
    {
        string message = evt.eventParams[0].ToString();
        m_txt.text = message;
    }

    private void UpdateMessage(CEvent evt)
    {
        string message = evt.eventParams[0].ToString();
        m_txt.text = message;
    }

    private void UnRegisterEvent()
    {
        CEventDispatcher.Instance.removeEventListener(CEventName.UPDATE_CHECK, CheckUpdateMessage);
        CEventDispatcher.Instance.removeEventListener(CEventName.UPDATE_MESSAGE, UpdateMessage);
        CEventDispatcher.Instance.removeEventListener(CEventName.UPDATE_DONLOAD_SPEED, ShowDownloadSpeed);
        CEventDispatcher.Instance.removeEventListener(CEventName.UPDATE_FINISH, UpdateFinish);
        CEventDispatcher.Instance.removeEventListener(CEventName.UPDATE_PROGRESS, UpdateProgress); 
    }

    private void OnDestroy()
    {
        UnRegisterEvent();
    }
}

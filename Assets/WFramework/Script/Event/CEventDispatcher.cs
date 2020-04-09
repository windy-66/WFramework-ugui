/************************************************************
    文件: CEventDispatcher.cs
    作者: jiaxiaocheng
    功能: 事件派发器
*************************************************************/
using System.Collections.Generic;

public class CEventDispatcher :Singleton<CEventDispatcher>{
    public Dictionary<string, CEventListener> eventListenerDic; //事件名称与事件监听者的字典

    public CEventDispatcher () {
        this.eventListenerDic = new Dictionary<string, CEventListener> ();
    }

    //为eventName事件，加一个监听回调事件callBack
    public void addEventListener (string eventName, CEventListener.CEventListenerDelegate callBack) {
        if (!this.eventListenerDic.ContainsKey (eventName)) {
            this.eventListenerDic.Add (eventName, new CEventListener ());
        }
        this.eventListenerDic[eventName].OnEvent += callBack;
    }

    public void removeEventListener (string eventName, CEventListener.CEventListenerDelegate callBack) {
        if (this.eventListenerDic.ContainsKey (eventName)) {
            this.eventListenerDic[eventName].OnEvent -= callBack;
        }
    }

    public void dispatchEvent (CEvent evt, object obj) {
        CEventListener cEventListener = this.eventListenerDic[evt.eventName]; //从字典中把监听这个事件的全部取出来
        if (cEventListener == null) return;
        evt.target = obj;
        cEventListener.Excute(evt);//
    }

    public bool hasListener(string eventName){
        return this.eventListenerDic.ContainsKey(eventName);
    }
}
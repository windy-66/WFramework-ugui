/************************************************************
    文件: CEventListener.cs
    作者: jiaxiaocheng
    功能: 事件派发器
*************************************************************/
public class CEventListener {
    public CEventListener () { }
    //委托
    public delegate void CEventListenerDelegate (CEvent evt);

    public event CEventListenerDelegate OnEvent;

    public void Excute (CEvent evt) {
        if (OnEvent != null) {
            this.OnEvent (evt);
        }
    }
}
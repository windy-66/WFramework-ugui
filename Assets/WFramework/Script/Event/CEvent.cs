/************************************************************
    文件: CEvent.cs
    作者: jiaxiaocheng
    功能: 事件派发器
*************************************************************/
public class CEvent {
    public string eventName; //事件名称
    public object[] eventParams; //事件传的参数
    public object target; //事件抛出者
    public CEvent(string name, params object[] args )
    {     
        this.eventName = name;
        this.eventParams = args;
    }
}
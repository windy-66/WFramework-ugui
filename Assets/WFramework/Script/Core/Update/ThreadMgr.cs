using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;

public class NotiConst
{
    /// <summary>
    /// View层消息通知
    /// </summary>
    public const string UPDATE_MESSAGE = "UpdateMessage";           //更新消息
    public const string UPDATE_EXTRACT = "UpdateExtract";           //更新解包
    public const string UPDATE_DOWNLOAD = "UpdateDownload";         //更新下载
    public const string UPDATE_PROGRESS = "UpdateProgress";         //更新进度
}
public class NotiData
{
    public string evName;
    public object evParam;

    public NotiData(string name, object param)
    {
        this.evName = name;
        this.evParam = param;
    }
}
/// <summary>
/// 当前线程管理器，同时只能做一个任务
/// </summary>
/// 
public class ThreadEvent
{
    public string Key;
    public List<object> evParams = new List<object>();
}

public class ThreadMgr : SingletonUnity<ThreadMgr>
{
    private Thread thread;
    private Action<NotiData> func;
    private Stopwatch sw = new Stopwatch();
    private string currDownFile = string.Empty;

    static readonly object m_lockOnject = new object();
    static Queue<ThreadEvent> events = new Queue<ThreadEvent>();

    delegate void ThreadSyncEvent(NotiData data);
    private ThreadSyncEvent m_SyncEvent;

    // Start is called before the first frame update
    void Awake()
    {
        m_SyncEvent = OnSyncEvent;
        thread = new Thread(OnUpdate);
    }
    void Start()
    {
        thread.Start();
    }
    public void AddEvent(ThreadEvent ev, Action<NotiData> func)
    {
        lock (m_lockOnject)
        {
            this.func = func;
            events.Enqueue(ev);
        }
    }
    /// <summary>
    /// 通知事件
    /// </summary>
    /// <param name="data"></param>
    private void OnSyncEvent(NotiData data)
    {
        //todo
        // facade.SendMessageCommand(data.evName, data.evParam); //通知View层
        if (this.func != null) func(data);
    }
    // Update is called once per frame
    void OnUpdate()
    {
        while (true)
        {
            lock (m_lockOnject)
            {
                if (events.Count > 0)
                {
                    ThreadEvent evt = events.Dequeue();
                    try
                    {
                        switch (evt.Key)
                        {
                            case NotiConst.UPDATE_EXTRACT: //解压文件
                                OnExtractFile(evt.evParams);
                                break;
                            case NotiConst.UPDATE_DOWNLOAD: //下载文件
                                OnDownloadFile(evt.evParams);
                                break;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        UnityEngine.Debug.LogError(ex.Message);
                    }
                }
            }
            Thread.Sleep(1);
        }
    }

    void OnDownloadFile(List<object> evParams)
    {
        string url = evParams[0].ToString();
        currDownFile = evParams[1].ToString();

        using (WebClient client = new WebClient())
        {
            sw.Start();
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            client.DownloadFileAsync(new System.Uri(url), currDownFile);
        }
    }

    void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
        /*
       UnityEngine.Debug.Log(string.Format("{0} MB's / {1} MB's",
           (e.BytesReceived / 1024d / 1024d).ToString("0.00"),
           (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00")));
       */
        //float value = (float)e.ProgressPercentage / 100f;

        string value = string.Format("{0} kb/s", (e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds).ToString("0.00"));
        NotiData data = new NotiData(NotiConst.UPDATE_PROGRESS, value);
        if (m_SyncEvent != null) m_SyncEvent(data);

        if(e.ProgressPercentage == 100 && e.BytesReceived == e.TotalBytesToReceive)
        {
            sw.Reset();
            data = new NotiData(NotiConst.UPDATE_DOWNLOAD, currDownFile);
            if (m_SyncEvent != null) m_SyncEvent(data);
        }
    }
    /// <summary>
    /// 解压完成通知
    /// </summary>
    /// <param name="evParams"></param>
    void OnExtractFile(List<object> evParams)
    {
        UnityEngine.Debug.Log("Thread evParams: >> " + evParams.Count);

        //通知更新面板解压完成
        NotiData data = new NotiData(NotiConst.UPDATE_DOWNLOAD, null);
        if (m_SyncEvent != null) m_SyncEvent(data);
    }
    void OnDestroy()
    {
        thread.Abort();
    }
}

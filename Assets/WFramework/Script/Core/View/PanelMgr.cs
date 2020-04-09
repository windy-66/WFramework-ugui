using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 面板管理
/// </summary>
public class PanelMgr
{
    #region 初始化
    protected static PanelMgr mInstance;
    public static bool hasInstance
    {
        get
        {
            return mInstance != null;
        }
    }

    public static PanelMgr GetInstance
    {
        get
        {
            if (!hasInstance)
            {
                mInstance = new PanelMgr();
            }
            return mInstance;
        }

    }
    private PanelMgr()
    {
        panels = new Dictionary<PanelName, PanelBase>();
        panelsDethList = new List<PanelBase>();
    }
    #endregion

    #region 数据定义

    /// <summary>
    /// 面板显示方式
    /// </summary>
    public enum PanelShowStyle
    {
        /// <summary>
        /// //正常出现--
        /// </summary>
        Nomal,
        /// <summary>
        /// //中间变大--
        /// </summary>
        CenterScaleBigNomal,
        /// <summary>
        /// //上往中滑动--
        /// </summary>
        UpToSlide,
        /// <summary>
        /// //下往中滑动
        /// </summary>
        DownToSlide,
        /// <summary>
        /// //左往中--
        /// </summary>
        LeftToSlide,
        /// <summary>
        /// //右往中--
        /// </summary>
        RightToSlide,
        /// <summary>
        /// //从某点由小到大往中--
        /// </summary>
        //SomeplaceToSlide
    }

    /// <summary>
    /// 面板遮罩
    /// </summary>
    public enum PanelMaskSytle
    {
        /// <summary>
        /// 无背景
        /// </summary>
        None,
        /// <summary>
        /// 不透明背景，周围点击可关闭
        /// </summary>
        Opacity,
        /// <summary>
        /// 半透明，周围点击可关闭
        /// </summary>
        Translucence,
        /// <summary>
        /// 透明，周围点击可关闭
        /// </summary>
        Lucency,
    }

    /// <summary>
    /// 存储当前已经实例化的面板
    /// </summary>
    public Dictionary<PanelName, PanelBase> panels;

    /// <summary> 深度列表 </summary>
    public List<PanelBase> panelsDethList;

    #endregion


    /// <summary>
    /// 当前面板
    /// </summary>
    public PanelBase current;

    public void Destroy()
    {

    }

    /// <summary>
    /// 打开指定弹框
    /// </summary>
    /// <param name="sceneType"></param>
    /// <param name="sceneArgs">场景参数</param>
    public void ShowPanel(PanelName panelName, params object[] sceneArgs)
    {

        if (panels.ContainsKey(panelName))
        {
            //Debug.LogError("该面板已打开！");
            current = panels[panelName];
            current.gameObject.SetActive(false);
            current.OnInit(sceneArgs);
            current.OnShowing();
            //  LayerMgr.GetInstance.SetLayer(current.gameObject, LayerType.Panel);
            ViewMgr.GetInstance.SetLayer(current.gameObject, LayerType.Panel);
        }
        else
        {
            GameObject go = new GameObject(panelName.ToString());
            Type mType = Type.GetType(panelName.ToString());
            PanelBase pb = go.AddComponent(mType) as PanelBase; //sceneType.tostring等于该场景的classname
            //pb.gameObject.SetActive(false);

            pb.OnInit(sceneArgs);
            panels.Add(pb.type, pb);
            MaskStyle(pb);
            panelsDethList.Add(pb);
            ChangePanelDeth(pb);
            current = pb;
            pb.OnShowing();
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.transform.localRotation = Quaternion.identity;
        }
        StartShowPanel(current, current.PanelShowStyle, true);
    }

    /// <summary> 关闭所有面板 </summary>
    public void CloseAllPanel()
    {
        Dictionary<PanelName, PanelBase>.ValueCollection vs = panels.Values;
        foreach (PanelBase item in vs)
        {
            StartShowPanel(item, item.PanelShowStyle, false);
        }
        panelsDethList.Clear();
    }

    /// <summary> 打开关闭面板效果 </summary>
    private void StartShowPanel(PanelBase go, PanelShowStyle showStyle, bool isOpen)
    {
        switch (showStyle)
        {
            case PanelShowStyle.Nomal:
                ShowNomal(go, isOpen);
                break;
            case PanelShowStyle.CenterScaleBigNomal:
                CenterScaleBigNomal(go, isOpen);
                break;
            case PanelShowStyle.LeftToSlide:
                LeftAndRightToSlide(go, false, isOpen);
                break;
            case PanelShowStyle.RightToSlide:
                LeftAndRightToSlide(go, true, isOpen);
                break;
            case PanelShowStyle.UpToSlide:
                TopAndDownToSlide(go, true, isOpen);
                break;
            case PanelShowStyle.DownToSlide:
                TopAndDownToSlide(go, false, isOpen);
                break;
                //case PanelShowStyle.SomeplaceToSlide:
                //    SomeplaceToSlide(go, isOpen);
                //    break;
        }


    }

    #region 显示方式
    /// <summary> 默认显示 </summary>
    private void ShowNomal(PanelBase go, bool isOpen)
    {
        if (isOpen)
        {
            current.gameObject.SetActive(true);
            current.OnShowed();
        }
        else DestroyPanel(go.type);
    }

    /// <summary> 中间变大 </summary>
    private void CenterScaleBigNomal(PanelBase go, bool isOpen)
    {
        TweenScale ts = go.gameObject.GetComponent<TweenScale>();
        if (ts == null) ts = go.gameObject.AddComponent<TweenScale>();
        //
        ts.from = Vector3.zero;
        ts.to = Vector3.one;
        ts.duration = go.OpenDuration;
        ts.ease = Ease.InOutSine;
        ts.onFinished = () =>
        {
            if (isOpen) go.OnShowed();
            else DestroyPanel(go.type);
        };
        go.gameObject.SetActive(true);
        if (isOpen)
        {
            ts.Reset();
            ts.PlayForward();
        }
        else
        {
            ts.PlayReverse();
        }
    }

    /// <summary> 左右往中 </summary>
    private void LeftAndRightToSlide(PanelBase go, bool isRight, bool isOpen)
    {
        TweenPosition tp = go.gameObject.GetComponent<TweenPosition>();
        TweenAlpha ta = go.gameObject.GetComponent<TweenAlpha>();
        if (tp == null)
        {
            tp = go.gameObject.AddComponent<TweenPosition>();
            ta = go.gameObject.AddComponent<TweenAlpha>();
        }
        tp.from = isRight == true ? new Vector3(1000, 0, 0) : new Vector3(-1000, 0, 0);
        tp.to = Vector3.zero;
        ta.from = 0;
        ta.to = 1;
        ta.duration = go.OpenDuration;
        tp.duration = go.OpenDuration;
        tp.ease = Ease.InOutSine;
        tp.onFinished = () =>
        {
            if (isOpen) go.OnShowed();
            else DestroyPanel(go.type);
        };
        go.gameObject.SetActive(true);
        if (isOpen)
        {
            tp.Reset();
            tp.PlayForward();
            ta.Reset();
            ta.PlayForward();
        }
        else
        {
            tp.PlayReverse();
            ta.PlayReverse();
        }
    }

    /// <summary> 上下往中 </summary>
    private void TopAndDownToSlide(PanelBase go, bool isTop, bool isOpen)
    {
        TweenPosition tp = go.gameObject.GetComponent<TweenPosition>();
        if (tp == null) tp = go.gameObject.AddComponent<TweenPosition>();
        //
        tp.from = isTop == true ? new Vector3(0, 1000, 0) : new Vector3(0, -1000, 0);
        tp.to = Vector3.zero;
        tp.duration = go.OpenDuration;
        tp.ease = Ease.InOutSine;
        tp.onFinished = () =>
        {
            if (isOpen) go.OnShowed();
            else DestroyPanel(go.type);
        };
        go.gameObject.SetActive(true);
        if (isOpen)
        {
            tp.Reset();
            tp.PlayForward();
        }
        else
        {
            tp.PlayReverse();
        }
    }

    /// <summary> 从某个点往中 </summary>
    private void SomeplaceToSlide(PanelBase go, bool isOpen)
    {
        TweenPosition tp = go.gameObject.GetComponent<TweenPosition>();
        TweenScale ts = go.gameObject.GetComponent<TweenScale>();
        if (tp == null) tp = go.gameObject.AddComponent<TweenPosition>();
        if (ts == null) ts = go.gameObject.AddComponent<TweenScale>();
        AnimationCurve ac = new AnimationCurve();
        ac.AddKey(new Keyframe(0.05f, 0.01f));
        ac.AddKey(new Keyframe(0.4f, 0.03f));
        ac.AddKey(new Keyframe(0.7f, 1.1f));
        tp.from = (Vector3)go.panelArgs[0];
        tp.to = Vector3.zero;
        ts.from = new Vector3(0.1f, 0.1f, 0.1f);
        ts.to = new Vector3(1f, 1f, 1f);
        tp.duration = go.OpenDuration;
        ts.duration = go.OpenDuration;
        tp.ease = Ease.Linear;
        tp.onFinished = () =>
        {
            if (isOpen) go.OnShowed();
            else DestroyPanel(go.type);
        };
        go.gameObject.SetActive(true);
        if (isOpen)
        {
            tp.Reset();
            tp.PlayForward();
        }
        else
        {
            tp.PlayReverse();
        }
    }

    #endregion

    #region 遮罩方式

    private void MaskStyle(PanelBase go)
    {
        Transform mask = ResourceMgr.GetInstance.CreateTransform("PanelMask", true);
        mask.GetComponent<RectTransform>().sizeDelta = GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta;
        mask.GetComponent<ButtonEx>().onLeftClick = g => {
            HidePanel((PanelName)Enum.Parse(typeof(PanelName), mask.transform.parent.name));
        };
        float alpha = 0;
        switch (go.PanelMaskStyle)
        {
            case PanelMaskSytle.None:
                alpha = 0;
                mask.GetComponent<ButtonEx>().onLeftClick =g=>{ };
                break;
            case PanelMaskSytle.Lucency:
                alpha = 0;
                break;
            case PanelMaskSytle.Translucence:
                alpha = 0.5f;
                break;
            case PanelMaskSytle.Opacity:
                alpha = 1f;
                break;
        }
        mask.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
        mask.SetParent ( go.gameObject.transform);
        mask.localPosition(Vector3.zero).localEulerAngles( Vector3.zero).localScale(1);
       // LayerMgr.GetInstance.SetLayer(go.gameObject, LayerType.Panel);
       ViewMgr.GetInstance.SetLayer(go.gameObject, LayerType.Panel);
    }

    #endregion

    /// <summary>
    /// 发起关闭
    /// </summary>
    public void HidePanel(PanelName type)
    {
        if (panels.ContainsKey(type))
        {
            PanelBase pb = null;
            pb = panels[type];
            StartShowPanel(pb, pb.PanelShowStyle, false);
            panelsDethList.Remove(pb);
        }
        else Debug.LogError("关闭的面板不存在!");
    }



    // <summary> 改变面板的深度 </summary>
    public void ChangePanelDeth(PanelBase type)
    {
        if (panelsDethList.Contains(type))
        {
            if (current == type) return;
            panelsDethList.Remove(type);
            panelsDethList.Add(type);
        }
        else
        {
            Debug.LogError("面板不存在");
            return;
        }
        //  LayerMgr.GetInstance.SetPanelsLayer(panelsDethList);
         ViewMgr.GetInstance.SetPanelsLayer(panelsDethList);
    }


    /// <summary>
    /// 强制摧毁面板
    /// </summary>
    /// <param name="panel"></param>
    public void DestroyPanel(PanelName type)
    {
        if (panels.ContainsKey(type))
        {
            PanelBase pb = panels[type];
            if (!pb.cache)
            {
                GameObject.Destroy(pb.gameObject);
                panels.Remove(type);
                panelsDethList.Remove(pb);
            }
        }
    }
}

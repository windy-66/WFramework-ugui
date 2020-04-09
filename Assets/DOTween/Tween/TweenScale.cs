using UnityEngine;
using DG.Tweening;

/// <summary>
/// 动画之缩放类
/// </summary>
public class TweenScale : UITweener {
    /// <summary>
    /// 起始缩放
    /// </summary>
    public Vector3 from;

    /// <summary>
    /// 结束缩放
    /// </summary>
    public Vector3 to;

    Transform cacheTransform;

    /// <summary>
    /// 缓存transform
    /// </summary>
    Transform CacheTransform {
        get {
            if (cacheTransform == null)
                cacheTransform = transform;
            return cacheTransform;
        }
    }

    GameObject cacheGameObject;

    /// <summary>
    /// 缓存gameObject
    /// </summary>
    GameObject CacheGameObject {
        get {
            if (cacheGameObject == null)
                cacheGameObject = gameObject;
            return cacheGameObject;
        }
    }

    /// <summary>
    /// 本物体的缩放坐标
    /// </summary>
    Vector3 Scale {
        get {
            return CacheTransform.localScale;
        }
    }

    /// <summary>
    /// 顺序播放动画
    /// </summary>
    public override void PlayForward() {
        base.PlayForward();
    }

    /// <summary>
    /// 顺序播放动画 延迟
    /// </summary>
    public override void PlayForwardDelay() {
        CacheGameObject.SetActive(true);
        Play(from, to);
    }

    /// <summary>
    /// 倒序播放动画
    /// </summary>
    public override void PlayReverse() {
        base.PlayReverse();
    }

    /// <summary>
    /// 倒序播放动画 延迟
    /// </summary>
    public override void PlayReverseDelay() {
        CacheGameObject.SetActive(true);
        Play(to, from);
    }

    /// <summary>
    /// 播放动画
    /// </summary>
    void Play(Vector3 from, Vector3 to) {
        switch (style) {
            case TweenStyle.Once:
                Once(from, to);
                break;
            case TweenStyle.Loop:
                Loop(from, to);
                break;
            case TweenStyle.Repeatedly:
                Repeatedly(from, to);
                break;
            case TweenStyle.PingPong:
                PingPong(from, to);
                break;
        }
    }

    /// <summary>
    /// 一次
    /// </summary>
    void Once(Vector3 from, Vector3 to) {
        CacheTransform.localScale = from;
        CacheTransform.DOScale(to, duration).OnComplete(() => onFinished());
    }

    /// <summary>
    /// 循环
    /// </summary>
    void Loop(Vector3 from, Vector3 to) {
        CacheTransform.localScale = from;
        CacheTransform.DOScale(to, duration).OnComplete(() => Loop(from, to));
    }

    /// <summary>
    /// 一次来回
    /// </summary>
    void Repeatedly(Vector3 from, Vector3 to) {
        CacheTransform.localScale = from;
        CacheTransform.DOScale(to, duration).OnComplete(() => CacheTransform.DOScale(from, duration));
    }

    /// <summary>
    /// 循环来回
    /// </summary>
    void PingPong(Vector3 from, Vector3 to) {
        CacheTransform.DOScale(to, duration).OnComplete(() => PingPong(to, from));
    }

    /// <summary>
    /// 起始值
    /// </summary>
    protected override void StartValue() {
        from = Scale;
    }

    /// <summary>
    /// 结束值
    /// </summary>
    protected override void EndValue() {
        to = Scale;
    }
}
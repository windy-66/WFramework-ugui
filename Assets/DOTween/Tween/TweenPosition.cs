using UnityEngine;
using DG.Tweening;

/// <summary>
/// 动画之位移类
/// </summary>
public class TweenPosition : UITweener {
    /// <summary>
    /// 起始坐标
    /// </summary>
    public Vector3 from;

    /// <summary>
    /// 结束坐标
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
    /// 本物体的位移坐标
    /// </summary>
    Vector3 Position {
        get {
            return CacheTransform.localPosition;
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
        CacheTransform.localPosition = from;
        CacheTransform.DOLocalMove(to, duration).OnComplete(() => onFinished());
    }

    /// <summary>
    /// 循环
    /// </summary>
    void Loop(Vector3 from, Vector3 to) {
        CacheTransform.localPosition = from;
        CacheTransform.DOLocalMove(to, duration).OnComplete(() => Loop(this.from, to));
    }

    /// <summary>
    /// 一次来回
    /// </summary>
    void Repeatedly(Vector3 from, Vector3 to) {
        CacheTransform.localPosition = from;
        CacheTransform.DOLocalMove(to, duration).OnComplete(() => CacheTransform.DOLocalMove(this.from, duration));
    }

    /// <summary>
    /// 循环来回
    /// </summary>
    void PingPong(Vector3 from, Vector3 to) {
        CacheTransform.DOLocalMove(to, duration).OnComplete(() => PingPong(to, from));
    }

    /// <summary>
    /// 起始值
    /// </summary>
    protected override void StartValue() {
        from = Position;
    }

    /// <summary>
    /// 结束值
    /// </summary>
    protected override void EndValue() {
        to = Position;
    }
}

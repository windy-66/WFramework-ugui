using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEx : Button {
    public Action<Transform> onLeftClick { get; set; }
    public Action<Transform> onDoubleClick { get; set; }
    public Action<Transform> onMiddleClick { get; set; }
    public Action<Transform> OnRightClick { get; set; }
    public Action<Transform> onEnter { get; set; }
    public Action<Transform> onExit { get; set; }
    public Action<Transform> onUp { get; set; }
    public Action<Transform> onDown { get; set; }
    public Action<Transform> onDeselect { get; set; }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (eventData.clickCount == 1)
            {
                if (onLeftClick != null) onLeftClick(transform);
            }
            else if (eventData.clickCount == 2)
                if (onDoubleClick != null) onDoubleClick(transform);
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            if (onMiddleClick != null) onMiddleClick(transform);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (OnRightClick != null) OnRightClick(transform);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (onEnter != null) onEnter(transform);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if (onExit != null) onExit(transform);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if (onUp != null) onUp(transform);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (onDown != null) onDown(transform);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        if (onDeselect != null) onDeselect(transform);
    }


    //public Action<GameObject> onLeftClick { get; set; }
    //public Action<GameObject> onDoubleClick { get; set; }
    //public Action<GameObject> onMiddleClick { get; set; }
    //public Action<GameObject> OnRightClick { get; set; }
    //public Action<GameObject> onEnter { get; set; }
    //public Action<GameObject> onExit { get; set; }
    //public Action<GameObject> onUp { get; set; }
    //public Action<GameObject> onDown { get; set; }
    //public Action<GameObject> onDeselect { get; set; }

    //public override void OnPointerClick(PointerEventData eventData) {
    //    base.OnPointerClick(eventData);
    //    if (eventData.button == PointerEventData.InputButton.Left) {
    //        if (eventData.clickCount == 1) {
    //            if (onLeftClick != null)onLeftClick(gameObject);
    //        } else if (eventData.clickCount == 2)
    //            if (onDoubleClick != null) onDoubleClick(gameObject);
    //    } else if (eventData.button == PointerEventData.InputButton.Middle) {
    //        if (onMiddleClick != null) onMiddleClick(gameObject);
    //    } else if (eventData.button == PointerEventData.InputButton.Right) {
    //        if (OnRightClick != null) OnRightClick(gameObject);
    //    }
    //}

    //public override void OnPointerEnter(PointerEventData eventData) {
    //    base.OnPointerEnter(eventData);
    //    if (onEnter != null) onEnter(gameObject);
    //}

    //public override void OnPointerExit(PointerEventData eventData) {
    //    base.OnPointerExit(eventData);
    //    if (onExit != null) onExit(gameObject);
    //}

    //public override void OnPointerUp(PointerEventData eventData) {
    //    base.OnPointerUp(eventData);
    //    if (onUp != null) onUp(gameObject);
    //}

    //public override void OnPointerDown(PointerEventData eventData) {
    //    base.OnPointerDown(eventData);
    //    if (onDown != null) onDown(gameObject);
    //}

    //public override void OnDeselect(BaseEventData eventData) {
    //    base.OnDeselect(eventData);
    //    if (onDeselect != null) onDeselect(gameObject);
    //}
}

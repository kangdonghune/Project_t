using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, ISelectHandler, IDeselectHandler,IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler, IBeginDragHandler,IDragHandler, IEndDragHandler
{
    public Action<PointerEventData> OnSelectHandler = null;
    public Action<PointerEventData> OnDeselectHandler = null;
    public Action<PointerEventData> OnEnterHandler = null;
    public Action<PointerEventData> OnExitHandler = null;
    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnBeginDragHandler = null;
    public Action<PointerEventData> OnDragHandler = null;
    public Action<PointerEventData> OnEndDragHandler = null;

    public void OnSelect(BaseEventData eventData)
    {
        if (OnSelectHandler != null)
            OnSelectHandler.Invoke((PointerEventData)eventData);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (OnDeselectHandler != null)
            OnDeselectHandler.Invoke((PointerEventData)eventData);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (OnEnterHandler != null)
            OnEnterHandler.Invoke(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (OnExitHandler != null)
            OnExitHandler.Invoke(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClickHandler != null)
            OnClickHandler.Invoke(eventData);
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (OnBeginDragHandler != null)
            OnBeginDragHandler.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (OnDragHandler != null)
            OnDragHandler.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (OnEndDragHandler != null)
            OnEndDragHandler.Invoke(eventData);
    }


}

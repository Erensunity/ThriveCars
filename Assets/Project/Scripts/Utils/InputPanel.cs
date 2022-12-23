using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InputPanel :
    MonoSingleton<InputPanel>,
    IDragHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerExitHandler
{
    [Space(16f)]
    [Header("Full Info Events")]
    public PointerEventDataEvent OnDragFullInfo = new PointerEventDataEvent();
    public PointerEventDataEvent OnPointerDownFullInfo = new PointerEventDataEvent();
    public PointerEventDataEvent OnPointerUpFullInfo = new PointerEventDataEvent();
    public PointerEventDataEvent OnPointerExitFullInfo = new PointerEventDataEvent();
    
    [Space(16f)]
    [Header("Delta Event")]
    public PositionEvent OnDragDelta = new PositionEvent();
    [Space(16f)]
    [Header("Position Events")]
    public PositionEvent OnDragPosition = new PositionEvent();
    public PositionEvent OnPointDownPosition = new PositionEvent();
    public PositionEvent OnPointerUpPosition = new PositionEvent();
    public PositionEvent OnPointerExitPosition = new PositionEvent();
    
    [Space(16f)]
    [Header("Pointer Events")]
    public EmptyEvent OnPointerDownEvent = new EmptyEvent();
    public EmptyEvent OnPointerUpEvent = new EmptyEvent();
    public EmptyEvent OnPointerExitEvent = new EmptyEvent();
    private Image inputImage;

    private void Awake() => inputImage = GetComponent<Image>();

    public void OnDrag(PointerEventData eventData)
    {
        OnDragDelta?.Invoke(eventData.delta * (1536f / Screen.width));
        OnDragPosition?.Invoke(eventData.position * (1536f / Screen.width));
        OnDragFullInfo?.Invoke(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointDownPosition?.Invoke(eventData.position * (1536f / Screen.width));
        OnPointerDownEvent?.Invoke();
        OnPointerDownFullInfo?.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPointerUpPosition?.Invoke(eventData.position * (1536f / Screen.width));
        OnPointerUpEvent.Invoke();
        OnPointerUpFullInfo?.Invoke(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnPointerExitPosition?.Invoke(eventData.position * (1536f / Screen.width));
        OnPointerExitEvent?.Invoke();
        OnPointerExitFullInfo?.Invoke(eventData);
    }

    public void Enable() => inputImage.enabled = true;

    public void Disable() => inputImage.enabled = false;
}

[Serializable]
public class PointerEventDataEvent : UnityEvent<PointerEventData>
{
}

[Serializable]
public class PositionEvent : UnityEvent<Vector2>
{
}

[Serializable]
public class EmptyEvent : UnityEvent
{
}
using System;
using ThreeDent.Helpers.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

public class EdgeZoneHoverDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public event Action OnPointerHover;

    private Coroutine eventRaiseCoroutine;

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventRaiseCoroutine = this.InvokeRepeatedly(OnPointerHover.Invoke, 0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine(eventRaiseCoroutine);
    }
}

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EdgeZoneHoverDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public event Action OnPointerHover;

    private bool isHovered = false;

    public bool IsHovered => isHovered;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }
}

using System;
using System.Collections;
using ThreeDent.DevelopmentTools;
using ThreeDent.EventBroker;
using ThreeDent.Helpers.Extensions;
using TMPro;
using UnityEngine;

public class PointerController : Singleton<PointerController>
{
    [SerializeField] private RectTransform parentCanvasTransform;
    [SerializeField] private RectTransform pointer;
    [SerializeField] private float offset;
    [SerializeField] private RectTransform countdownPivot;
    [SerializeField] private TextMeshProUGUI countdown;

    private Transform currentActiveBlock;
    private Coroutine trackingCoroutine;

    protected override void Awake()
    {
        base.Awake();
        Deactivate();
        EventBroker.Subscribe<ActiveBlockSet>(Activate);
        EventBroker.Subscribe<ActiveBlockUnset>(Deactivate);
    }

    private void OnDestroy()
    {
        EventBroker.Unsubscribe<ActiveBlockSet>(Activate);
        EventBroker.Unsubscribe<ActiveBlockUnset>(Deactivate);
    }

    private void Activate()
    {
        Instance.currentActiveBlock = ActiveBlockManager.Instance.ActiveBlockTransform;
        Instance.pointer.gameObject.SetActive(true);
        Instance.countdown.gameObject.SetActive(true);
        Instance.trackingCoroutine = Instance.StartCoroutine(Instance.TrackingCycle());
    }

    private void Deactivate()
    {
        Instance.currentActiveBlock = null;
        pointer.gameObject.SetActive(false);
        countdown.gameObject.SetActive(false);
        if (trackingCoroutine != null)
            Instance.StopCoroutine(Instance.trackingCoroutine);
    }

    private IEnumerator TrackingCycle()
    {
        while (true)
        {
            // Calculate how much canvas pixels are in 1 world unit.
            //   Calculate aspect ratio.
            //   Find how much world units carema takes on X axis.
            //   Calcucale how much pixels it takes to pass 1 world unit.
            var cameraWorldUnitsOnX = Camera.main.orthographicSize * Camera.main.aspect * 2f;
            var canvasX = parentCanvasTransform.sizeDelta.x;
            var canvasPixelsPerUnit = canvasX / cameraWorldUnitsOnX;

            var maxX = canvasX / 2 - offset;
            var minX = -maxX;

            var cameraX = Camera.main.transform.position.x;
            var blockX = currentActiveBlock.position.x;
            var pixels = (blockX - cameraX) * canvasPixelsPerUnit;
            pointer.anchoredPosition = pointer.anchoredPosition.With(x: Mathf.Clamp(pixels, minX, maxX));
            if (pixels < minX)
                pointer.eulerAngles = new(0f, 0f, 45f);
            else if (pixels > maxX)
                pointer.eulerAngles = new(0f, 0f, -45f);
            else
                pointer.eulerAngles = Vector3.zero;

            var cameraUpperBoundY = Camera.main.transform.position.y + Camera.main.orthographicSize;
            var blockY = currentActiveBlock.position.y;
            var distanceToTravel = blockY - cameraUpperBoundY;
            countdown.text = $"{Math.Round(distanceToTravel, 1)}m";

            if (distanceToTravel <= 0f)
            {
                Deactivate();
                break;
            }

            countdown.transform.position = countdownPivot.position;

            yield return null;
        }
    }
}
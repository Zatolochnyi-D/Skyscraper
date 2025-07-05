using System.Collections;
using ThreeDent.Helpers.Extensions;
using ThreeDent.Helpers.Tools;
using UnityEngine;

public class PointerController : Singleton<PointerController>
{
    [SerializeField] private RectTransform parentCanvasTransform;
    [SerializeField] private RectTransform pointer;
    [SerializeField] private float offset;

    private Transform currentActiveBlock;
    private Coroutine trackingCoroutine;

    protected override void Awake()
    {
        base.Awake();
        pointer.gameObject.SetActive(false);
    }

    public static void SetActiveBlock(Transform newActiveBlock)
    {
        Instance.currentActiveBlock = newActiveBlock;
        Instance.pointer.gameObject.SetActive(true);
        Instance.trackingCoroutine = Instance.StartCoroutine(Instance.TrackingCycle());
    }

    public static void RemoveActiveBlock()
    {
        Instance.currentActiveBlock = null;
        Instance.pointer.gameObject.SetActive(false);
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

            yield return null;
        }
    }
}

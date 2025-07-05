using System.Collections;
using ThreeDent.Helpers.Extensions;
using ThreeDent.Helpers.Tools;
using UnityEngine;
using UnityEngine.UI;

public class PointerPositionController : Singleton<PointerPositionController>
{
    [SerializeField] private RectTransform parentCanvasTransform;
    [SerializeField] private RectTransform pointer;
    [SerializeField] private float offset;

    private Transform currentActiveBlock;
    private Coroutine trackingCoroutine;

    public static void SetActiveBlock(Transform newActiveBlock)
    {
        Instance.currentActiveBlock = newActiveBlock;
        Instance.trackingCoroutine = Instance.StartCoroutine(Instance.TrackingCycle());
    }

    public static void RemoveActiveBlock()
    {
        Instance.currentActiveBlock = null;
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
            pixels = Mathf.Min(maxX, pixels);
            pixels = Mathf.Max(minX, pixels);
            pointer.anchoredPosition = pointer.anchoredPosition.With(x: pixels);

            yield return null;
        }
    }
}

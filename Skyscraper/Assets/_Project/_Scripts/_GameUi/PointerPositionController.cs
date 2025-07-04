using System.Collections;
using ThreeDent.Helpers.Tools;
using UnityEngine;

public class PointerPositionController : Singleton<PointerPositionController>
{
    [SerializeField] private RectTransform pointer;

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
        yield return null;
    }
}

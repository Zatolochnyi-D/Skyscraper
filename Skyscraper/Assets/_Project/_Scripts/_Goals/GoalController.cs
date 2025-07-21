using ThreeDent.EventBroker;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    private float heighestPoint = 0f;

    private void Awake()
    {
        EventBroker.Subscribe<NewHeighestPointFoundEvent>(HandleNewHeighestPoint);
    }

    private void OnDestroy()
    {
        EventBroker.Unsubscribe<NewHeighestPointFoundEvent>(HandleNewHeighestPoint);
    }

    private void HandleNewHeighestPoint(NewHeighestPointFoundEvent args)
    {
        heighestPoint = args.heighestPoint;
        Debug.Log(heighestPoint);
    }
}

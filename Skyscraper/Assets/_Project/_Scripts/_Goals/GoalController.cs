using System;
using ThreeDent.DevelopmentTools;
using ThreeDent.EventBroker;

public class GoalController : Singleton<GoalController>
{
    public event Action OnHeighestPointChange;

    private float heighestPoint = 0f;

    public float HeighestPoint => (float)Math.Round(heighestPoint, 1);

    protected override void Awake()
    {
        base.Awake();
        EventBroker.Subscribe<NewHeighestPointFoundEvent>(HandleNewHeighestPoint);
    }

    private void OnDestroy()
    {
        EventBroker.Unsubscribe<NewHeighestPointFoundEvent>(HandleNewHeighestPoint);
    }

    private void HandleNewHeighestPoint(NewHeighestPointFoundEvent args)
    {
        heighestPoint = args.heighestPoint;
        OnHeighestPointChange?.Invoke();
    }
}

using System;
using ThreeDent.DevelopmentTools;
using ThreeDent.DevelopmentTools.Option;
using ThreeDent.EventBroker;
using ThreeDent.Helpers.Extensions;
using UnityEngine;

public class GoalController : Singleton<GoalController>
{
    public event Action OnHeighestPointChange;

    [SerializeField] private float heightToReach;
    [SerializeField] private int blocksToGive;
    [SerializeField] private float incrementWithEachGoal;
    [SerializeField] private SpriteRenderer dash;
    [SerializeField] private Transform zeroHeight;

    private float heighestPoint = 0f;
    private int goalsReached = 0;

    public float HeighestPoint => (float)Math.Round(heighestPoint, 1);

    protected override void Awake()
    {
        base.Awake();
        EventBroker.Subscribe<NewHeighestPointFoundEvent>(HandleNewHeighestPoint);
    }

    private void Update()
    {
        dash.transform.ChangePosition(x: Camera.main.transform.position.x, y: heightToReach * (1 + goalsReached) + zeroHeight.position.y);
        dash.size = dash.size.With(x: Camera.main.orthographicSize * Camera.main.aspect * 8f);
    }

    private void OnDestroy()
    {
        EventBroker.Unsubscribe<NewHeighestPointFoundEvent>(HandleNewHeighestPoint);
    }

    private void HandleNewHeighestPoint(NewHeighestPointFoundEvent args)
    {
        heighestPoint = args.heighestPoint;
        if (heighestPoint > heightToReach * (goalsReached + 1))
        {
            for (int i = 0; i < Mathf.Ceil(blocksToGive * (1 + incrementWithEachGoal * goalsReached)); i++)
                PlayerInventory.Instance.AddItem(i % PlayerInventory.Instance.ItemsCount);
            goalsReached++;
        }
        OnHeighestPointChange?.Invoke();
    }
}

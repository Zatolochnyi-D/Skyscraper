using System;
using ThreeDent.DevelopmentTools;
using ThreeDent.EventBroker;
using UnityEngine;

public class GoalController : Singleton<GoalController>
{
    public event Action OnHeighestPointChange;

    [SerializeField] private float heightToReach;
    [SerializeField] private int blocksToGive;
    [SerializeField] private float incrementWithEachGoal;

    private float heighestPoint = 0f;
    private int goalsReached = 0;

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
        if (heighestPoint > heightToReach * (goalsReached + 1))
        {
            goalsReached++;
            for (int i = 0; i < Mathf.Ceil(blocksToGive * (1 + incrementWithEachGoal * goalsReached)); i++)
            {
                PlayerInventory.Instance.AddItem(i % PlayerInventory.Instance.ItemsCount);
            }
        }
        OnHeighestPointChange?.Invoke();
    }
}

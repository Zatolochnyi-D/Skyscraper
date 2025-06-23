using Skyscraper.Inputs;
using ThreeDent.Helpers.Tools;
using UnityEngine;

public class BlockMovementController : Singleton<BlockMovementController>
{
    [SerializeField] private float rotationPerSecond = 180f;

    private BlockMover mover;

    protected override void Awake()
    {
        base.Awake();
        InputManager.OnSideMovement += RotateCurrentBlock;
    }

    private void RotateCurrentBlock(float controlValue)
    {
        mover.RotateContinious(controlValue * rotationPerSecond * Time.deltaTime);
    }

    public static void SetCurrentBlock(BlockMover newMover)
    {
        Instance.mover = newMover;
    }
}

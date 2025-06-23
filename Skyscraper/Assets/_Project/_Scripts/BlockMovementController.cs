using Skyscraper.Inputs;
using ThreeDent.Helpers.Tools;
using UnityEngine;

public class BlockMovementController : Singleton<BlockMovementController>
{
    [SerializeField] private float rotationPerSecond = 180f;
    [SerializeField] private float movementPerSecond = 4f;

    private BlockMover mover;

    protected override void Awake()
    {
        base.Awake();
        InputManager.OnMovement += MoveCurrentBlock;
        InputManager.OnRotation += RotateCurrentBlock;
    }

    private void MoveCurrentBlock(float controlValue)
    {
        if (mover != null)
            mover.MoveContinious(controlValue * Time.deltaTime * new Vector2(movementPerSecond, 0f));
    }

    private void RotateCurrentBlock(float controlValue)
    {
        if (mover != null)
            mover.RotateContinious(controlValue * Time.deltaTime * rotationPerSecond);
    }

    public static void SetCurrentBlock(BlockMover newMover)
    {
        Instance.mover = newMover;
    }

    public static void RemoveCurrentBlock()
    {
        Instance.mover = null;
    }
}

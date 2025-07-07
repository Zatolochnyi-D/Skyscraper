using Skyscraper.Inputs;
using ThreeDent.DevelopmentTools;
using UnityEngine;

public class BlockMovementController : Singleton<BlockMovementController>
{
    [SerializeField] private float rotationPerSecond = 180f;
    [SerializeField] private float movementPerSecond = 4f;
    [SerializeField] private float additionalFallPerSecond = 10f;

    private BlockMover mover;

    protected override void Awake()
    {
        base.Awake();
        InputManager.OnMovement += MoveCurrentBlock;
        InputManager.OnRotation += RotateCurrentBlock;
        InputManager.OnSpeedup += SpeedUpBlockFall;
    }

    private void MoveCurrentBlock(float controlValue)
    {
        if (mover != null)
            mover.MoveContinuous(controlValue * Time.deltaTime * new Vector2(movementPerSecond, 0f));
    }

    private void RotateCurrentBlock(float controlValue)
    {
        if (mover != null)
            mover.RotateContinuous(controlValue * Time.deltaTime * rotationPerSecond);
    }

    private void SpeedUpBlockFall()
    {
        if (mover != null)
            mover.MoveContinuous(Time.deltaTime * new Vector2(0f, -additionalFallPerSecond));
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

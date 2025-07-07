using Skyscraper.Inputs;
using ThreeDent.DevelopmentTools.Attributes;
using UnityEngine;

public class BlockMover : MonoBehaviour
{
    [OnThis, SerializeField] private Rigidbody2D physicalBody;
    [SerializeField] private float rotationPerSecond = 180f;
    [SerializeField] private float movementPerSecond = 4f;
    [SerializeField] private float additionalFallPerSecond = 10f;

    private float previousGravityScale;

    public void MoveContinuous(Vector2 step)
    {
        physicalBody.position += step;
    }

    public void RotateContinuous(float angle)
    {
        physicalBody.SetRotation(physicalBody.rotation + angle);
    }

    private void MoveCurrentBlock(float controlValue)
    {
        MoveContinuous(controlValue * Time.deltaTime * new Vector2(movementPerSecond, 0f));
    }

    private void RotateCurrentBlock(float controlValue)
    {
        RotateContinuous(controlValue * Time.deltaTime * rotationPerSecond);
    }

    private void SpeedUpBlockFall()
    {
        MoveContinuous(Time.deltaTime * new Vector2(0f, -additionalFallPerSecond));
    }

    public void Activate()
    {
        InputManager.OnMovement += MoveCurrentBlock;
        InputManager.OnRotation += RotateCurrentBlock;
        InputManager.OnSpeedup += SpeedUpBlockFall;

        physicalBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        previousGravityScale = physicalBody.gravityScale;
        physicalBody.gravityScale = 0f;
    }

    public void Deactivate()
    {
        InputManager.OnMovement -= MoveCurrentBlock;
        InputManager.OnRotation -= RotateCurrentBlock;
        InputManager.OnSpeedup -= SpeedUpBlockFall;

        physicalBody.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        physicalBody.gravityScale = previousGravityScale;
    }
}

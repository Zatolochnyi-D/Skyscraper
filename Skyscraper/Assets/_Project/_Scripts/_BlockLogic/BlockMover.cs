using Skyscraper.Inputs;
using ThreeDent.DevelopmentTools.Attributes;
using UnityEngine;

public class BlockMover : MonoBehaviour
{
    [OnThis, SerializeField] private Rigidbody2D physicalBody;
    [SerializeField] private float fallSpeed;
    [SerializeField] private float rotationPerSecond = 180f;
    [SerializeField] private float speedUpMultiplication = 2f;
    [SerializeField] private float additionalFallPerSecond = 10f;

    private float previousGravityScale;
    private Vector2 previousFallSpeed;

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
        MoveContinuous(controlValue * Time.deltaTime * new Vector2(speedUpMultiplication, 0f));
    }

    private void RotateCurrentBlock(float controlValue)
    {
        RotateContinuous(controlValue * Time.deltaTime * rotationPerSecond);
    }

    private void SpeedUpBlockFall()
    {
        previousFallSpeed = physicalBody.linearVelocity;
        physicalBody.linearVelocity *= speedUpMultiplication;
    }

    private void SlowDownBlockFall()
    {
        physicalBody.linearVelocity = previousFallSpeed;
    }

    public void Activate()
    {
        InputManager.OnMovement += MoveCurrentBlock;
        InputManager.OnRotation += RotateCurrentBlock;
        InputManager.OnSpeedupStarted += SpeedUpBlockFall;
        InputManager.OnSpeedupCanceled += SlowDownBlockFall;

        physicalBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        previousGravityScale = physicalBody.gravityScale;
        physicalBody.gravityScale = 0f;
        physicalBody.linearVelocity = new(0f, -fallSpeed);
    }

    public void Deactivate()
    {
        InputManager.OnMovement -= MoveCurrentBlock;
        InputManager.OnRotation -= RotateCurrentBlock;
        InputManager.OnSpeedupStarted -= SpeedUpBlockFall;
        InputManager.OnSpeedupCanceled -= SlowDownBlockFall;

        physicalBody.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        physicalBody.gravityScale = previousGravityScale;
    }
}

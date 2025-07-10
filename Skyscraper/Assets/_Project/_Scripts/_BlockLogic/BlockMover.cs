using Skyscraper.Inputs;
using ThreeDent.DevelopmentTools.Attributes;
using UnityEngine;

public class BlockMover : MonoBehaviour
{
    [OnThis, SerializeField] private Rigidbody2D physicalBody;
    [OnThis, SerializeField] private BlockLandingDetector landingDetector;
    [SerializeField] private float rotationPerSecond = 180f;
    [SerializeField] private float speedUpMultiplication = 2f;
    [SerializeField] private float massOnLand = 20f;

    private float previousGravityScale;
    private Vector2 previousFallSpeed;
    private bool isSpedUp;

    public Rigidbody2D PhysicalBody => physicalBody;

    private void Awake()
    {
        landingDetector.OnBlockLanded += SetMass;
    }

    private void SetMass()
    {
        physicalBody.mass = massOnLand;
    }

    private void MoveContinuous(Vector2 step)
    {
        physicalBody.position += step;
    }

    private void RotateContinuous(float angle)
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
        isSpedUp = true;
        previousFallSpeed = physicalBody.linearVelocity;
        physicalBody.linearVelocity *= speedUpMultiplication;
    }

    private void SlowDownBlockFall()
    {
        if (isSpedUp)
        {
            physicalBody.linearVelocity = previousFallSpeed;
            isSpedUp = false;
        }
    }

    public void Activate(float fallSpeed)
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

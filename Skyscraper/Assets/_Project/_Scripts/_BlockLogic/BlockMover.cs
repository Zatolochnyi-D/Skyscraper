using UnityEngine;

public class BlockMover : MonoBehaviour
{
    [OnThis, SerializeField] private Rigidbody2D physicalBody;

    public void MoveContinuous(Vector2 step)
    {
        physicalBody.position += step;
    }

    public void RotateContinuous(float angle)
    {
        physicalBody.SetRotation(physicalBody.rotation + angle);
    }
}

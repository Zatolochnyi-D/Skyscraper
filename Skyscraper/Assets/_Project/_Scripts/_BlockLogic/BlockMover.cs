using UnityEngine;

public class BlockMover : MonoBehaviour
{
    [OnThis, SerializeField] private Rigidbody2D physicalBody;

    public void MoveContinious(Vector2 step)
    {
        physicalBody.position += step;
    }

    public void RotateContinious(float angle)
    {
        physicalBody.SetRotation(physicalBody.rotation + angle);
    }
}

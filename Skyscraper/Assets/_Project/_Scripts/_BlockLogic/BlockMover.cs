using UnityEngine;

public class BlockMover : MonoBehaviour
{
    [OnThis, SerializeField] private Rigidbody2D physicalBody;

    public void RotateContinious(float angle)
    {
        physicalBody.SetRotation(physicalBody.rotation + angle);
    }
}

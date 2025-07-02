using Skyscraper.Inputs;
using UnityEngine;

public class CameraPositionController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 3f;

    private void Awake()
    {
        InputManager.OnCameraMovement += Move;
    }

    private void Move(Vector2 direction)
    {
        transform.position += (Vector3)(Time.deltaTime * movementSpeed * direction);
    }
}

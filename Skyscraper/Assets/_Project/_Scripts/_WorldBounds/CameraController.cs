using Skyscraper.Inputs;
using Unity.Cinemachine;
using UnityEngine;

namespace Skyscraper.WorldBounds
{
    public class CameraController : MonoBehaviour
    {
        [OnThis, SerializeField] private CinemachineCamera sceneCamera;
        [SerializeField] private float movementSpeed = 10f;
        [SerializeField] private float minZoom = 2f;
        [SerializeField] private float maxZoom = 8f;
        [SerializeField] private float zoomFactor = 0.035f;

        private void Awake()
        {
            InputManager.OnCameraMovement += Move;
            InputManager.OnScroll += Zoom;
        }

        private void Move(Vector2 direction)
        {
            transform.position += (Vector3)(Time.deltaTime * movementSpeed * direction);
        }

        private void Zoom(float direction)
        {
            var newSize = sceneCamera.Lens.OrthographicSize + direction * zoomFactor;
            sceneCamera.Lens.OrthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
        }
    }
}
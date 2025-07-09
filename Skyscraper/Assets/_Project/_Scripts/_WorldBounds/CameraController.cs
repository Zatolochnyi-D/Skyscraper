using Skyscraper.Inputs;
using ThreeDent.DevelopmentTools.Attributes;
using Unity.Cinemachine;
using UnityEngine;

namespace Skyscraper.WorldBounds
{
    public class CameraController : MonoBehaviour
    {
        [OnThis, SerializeField] private CinemachineCamera sceneCamera;
        [OnThis, SerializeField] private CinemachineConfiner2D confiner;
        [SerializeField] private float minimalMovementSpeed = 5f;
        [SerializeField] private float zoomSpeedInfluence = 0.2f; // how much % each point of zoom increases speed
        [SerializeField] private float minZoom = 2f;
        [SerializeField] private float zoomFactor = 0.035f;

        private void Awake()
        {
            InputManager.OnCameraMovement += Move;
            InputManager.OnScroll += Zoom;
        }

        private void Move(Vector2 direction)
        {
            var movementSpeed = minimalMovementSpeed * (1f + zoomSpeedInfluence * (sceneCamera.Lens.OrthographicSize - minZoom));
            transform.position += (Vector3)(Time.deltaTime * movementSpeed * direction);
        }

        private void Zoom(float direction)
        {
            var newSize = sceneCamera.Lens.OrthographicSize + direction * zoomFactor;

            float maxZoom;
            if (WorldBoundsController.BoundSize.x / WorldBoundsController.BoundSize.y > sceneCamera.Lens.Aspect)
                maxZoom = WorldBoundsController.BoundSize.y / 2;
            else
                maxZoom = WorldBoundsController.BoundSize.x / 2 / sceneCamera.Lens.Aspect;

            var previousPosition = sceneCamera.transform.position;
            sceneCamera.Lens.OrthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
            sceneCamera.transform.position = previousPosition;
            confiner.InvalidateBoundingShapeCache();
        }
    }
}
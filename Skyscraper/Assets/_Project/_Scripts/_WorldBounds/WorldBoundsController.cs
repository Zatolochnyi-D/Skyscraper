using ThreeDent.EventBroker;
using ThreeDent.Helpers.Extensions;
using Unity.Cinemachine;
using UnityEngine;

namespace Skyscraper.WorldBounds
{
    public class WorldBoundsController : MonoBehaviour
    {
        private const float TopRaycasterGap = 2f;

        [OnThis, SerializeField] private BoxCollider2D bounds;
        [SerializeField] private CinemachineConfiner2D confiner;
        [SerializeField] private Transform lowerBound;
        [SerializeField] private Collider2D topRaycaster;
        [SerializeField] private float additionalHeight;

        private void Awake()
        {
            EventBroker.Subscribe<BlockLandedEvent>(RecalculateUpperBound);
        }

        private void RecalculateUpperBound()
        {
            var raycastResults = new RaycastHit2D[1];
            topRaycaster.Cast(Vector2.down, raycastResults);
            var heighestPoint = raycastResults[0].point.y;

            var newUpperPoint = heighestPoint + additionalHeight;
            var newHeight = newUpperPoint - lowerBound.position.y;
            bounds.size = bounds.size.With(y: newHeight);
            bounds.offset = bounds.offset.With(y: newHeight / 2 + lowerBound.position.y);
            topRaycaster.transform.position = new Vector3(0f, newUpperPoint + TopRaycasterGap, 0f);

            confiner.InvalidateBoundingShapeCache();
        }
    }
}
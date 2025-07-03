using ThreeDent.EventBroker;
using ThreeDent.Helpers.Extensions;
using Unity.Cinemachine;
using UnityEngine;

namespace Skyscraper.WorldBounds
{
    public class WorldBoundsController : MonoBehaviour
    {
        private const float RaycasterGap = 3f;

        [OnThis, SerializeField] private BoxCollider2D bounds;
        [SerializeField] private CinemachineConfiner2D confiner;
        [SerializeField] private Transform lowerBound;
        [SerializeField] private EdgeCollider2D topRaycaster;
        [SerializeField] private float additionalHeight;
        [SerializeField] private EdgeCollider2D leftRaycaster;
        [SerializeField] private EdgeCollider2D rightRaycaster;
        [SerializeField] private float additionalDistanceFromSide;
        [SerializeField] private Transform floor;

        private void Awake()
        {
            EventBroker.Subscribe<BlockLandedEvent>(RecalculateBounds);
        }

        private void OnDestroy()
        {
            EventBroker.Unsubscribe<BlockLandedEvent>(RecalculateBounds);
        }

        private void RecalculateBounds()
        {
            var raycastResults = new RaycastHit2D[1];
            topRaycaster.Cast(Vector2.down, raycastResults);
            var heighestPoint = raycastResults[0].point.y;
            leftRaycaster.Cast(Vector2.right, raycastResults);
            var leftestPoint = raycastResults[0].point.x;
            rightRaycaster.Cast(Vector2.left, raycastResults);
            var rightestPoint = raycastResults[0].point.x;

            var newUpperPoint = heighestPoint + additionalHeight;
            var newHeight = newUpperPoint - lowerBound.position.y;
            var previousLeftPoint = -(bounds.size.x / 2 - bounds.offset.x);
            var newLeftPoint = Mathf.Min(leftestPoint - additionalDistanceFromSide, previousLeftPoint);
            var previousRightPoint = bounds.size.x / 2 + bounds.offset.x;
            var newRightPoint = Mathf.Max(rightestPoint + additionalDistanceFromSide, previousRightPoint);

            var newBoundsSize = new Vector2(newRightPoint - newLeftPoint, newHeight);
            var newBoundsOffset = new Vector2(newBoundsSize.x / 2 + newLeftPoint, newHeight / 2 + lowerBound.position.y);
            bounds.size = newBoundsSize;
            bounds.offset = newBoundsOffset;

            var points = topRaycaster.points;
            points[0] = points[0].With(x: newLeftPoint);
            points[1] = points[1].With(x: newRightPoint);
            topRaycaster.points = points;
            topRaycaster.transform.position = new Vector3(0f, newUpperPoint + RaycasterGap, 0f);

            points = leftRaycaster.points;
            points[1] = points[1].With(y: newHeight);
            leftRaycaster.points = points;
            leftRaycaster.transform.position = leftRaycaster.transform.position.With(x: newLeftPoint - RaycasterGap);

            points = rightRaycaster.points;
            points[1] = points[1].With(y: newHeight);
            rightRaycaster.points = points;
            rightRaycaster.transform.position = rightRaycaster.transform.position.With(x: newRightPoint + RaycasterGap);

            floor.localScale = floor.localScale.With(x: newBoundsSize.x + RaycasterGap * 2f);
            floor.position = floor.position.With(x: newBoundsOffset.x);

            confiner.InvalidateBoundingShapeCache();
        }
    }
}
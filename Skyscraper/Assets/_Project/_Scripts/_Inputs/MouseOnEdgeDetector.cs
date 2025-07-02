using ThreeDent.Helpers.Extensions;
using UnityEngine;

public class MouseOnEdgeDetector : MonoBehaviour
{
    [SerializeField] private EdgeZoneHoverDetector upDetector;
    [SerializeField] private EdgeZoneHoverDetector downDetector;
    [SerializeField] private EdgeZoneHoverDetector leftDetector;
    [SerializeField] private EdgeZoneHoverDetector rightDetector;

    private Vector2 directionVector;

    public Vector2 DirectionVector => directionVector;

    void Update()
    {
        directionVector = Vector2.zero;
        if (upDetector.IsHovered)
            directionVector = directionVector.With(y: 1);
        if (downDetector.IsHovered)
            directionVector = directionVector.With(y: -1);
        if (leftDetector.IsHovered)
            directionVector = directionVector.With(x: -1);
        if (rightDetector.IsHovered)
            directionVector = directionVector.With(x: 1);
        directionVector.Normalize();
    }
}

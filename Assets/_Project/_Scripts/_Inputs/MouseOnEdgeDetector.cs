using UnityEngine;

public class MouseOnEdgeDetector : MonoBehaviour
{
    [SerializeField] private EdgeZoneHoverDetector upDetector;
    [SerializeField] private EdgeZoneHoverDetector downDetector;
    [SerializeField] private EdgeZoneHoverDetector leftDetector;
    [SerializeField] private EdgeZoneHoverDetector rightDetector;
    [SerializeField] private EdgeZoneHoverDetector upLeftDetector;
    [SerializeField] private EdgeZoneHoverDetector upRightDetector;
    [SerializeField] private EdgeZoneHoverDetector downRightDetector;
    [SerializeField] private EdgeZoneHoverDetector downLeftDetector;

    private Vector2 directionVector;

    public Vector2 DirectionVector => directionVector;

    void Update()
    {
        directionVector = Vector2.zero;
        if (upDetector.IsHovered)
            directionVector = Vector2.up;
        if (downDetector.IsHovered)
            directionVector = Vector2.down;
        if (leftDetector.IsHovered)
            directionVector = Vector2.left;
        if (rightDetector.IsHovered)
            directionVector = Vector2.right;
        if (upLeftDetector.IsHovered)
            directionVector = new(-1, 1);
        if (upRightDetector.IsHovered)
            directionVector = new(1, 1);
        if (downRightDetector.IsHovered)
            directionVector = new(1, -1);
        if (downLeftDetector.IsHovered)
            directionVector = new(-1, -1);
        directionVector.Normalize();
    }
}

using UnityEngine;


public class WallInstanceData
{
    public Vector2Int GridPosition;
    public WallType WallType;
    public Transform WallTransform;
    public WallOrientation Orientation;
    public CornerType Corner;

    public WallInstanceData(Vector2Int gridPosition, WallType wallType, Transform wallTransform, WallOrientation orientation, CornerType corner = CornerType.None)
    {
        GridPosition = gridPosition;
        WallType = wallType;
        WallTransform = wallTransform;
        Orientation = orientation;
        Corner = corner;
    }
}

public enum CornerType
{
    None,
    TopLeft,
    TopRight,
    BottomRight,
    BottomLeft
}

public enum WallType
{
    Edge,
    Corner
}

public enum WallOrientation
{
    Right,
    Top,
    Left,
    Bottom
}

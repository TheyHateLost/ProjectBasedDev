using UnityEngine;

[System.Serializable]
public class WindowPlacementData
{
    public Transform WindowPrefab;
    public Vector2Int LocalPosition;
    public WallOrientation Orientation;
    public Quaternion Rotation;
}

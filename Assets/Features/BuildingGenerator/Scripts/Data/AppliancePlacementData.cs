using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AppliancePlacementData
{
    public Appliance Prefab;
    public Vector2Int LocalPosition;
    public Quaternion Rotation;
    public List<Vector2Int> Footprint = new();
}

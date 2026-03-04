using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "FloorPlan", menuName = "FloorPlan")]
public class FloorPlanSO : ScriptableObject
{
    [field: SerializeField] public List<RoomData> RoomDataList { get; private set; } = new();

    [ShowInInspector] public IntRange TotalTileAreaRange => GetTotalTileAreaRange();

    private IntRange GetTotalTileAreaRange()
    {
        IntRange totalRange = new IntRange(0, 0);
        foreach (RoomData roomData in RoomDataList)
        {
            totalRange.Min += roomData.TileAreaRange.Min;
            totalRange.Max += roomData.TileAreaRange.Max;
        }

        return totalRange;
    }
    
    public IntRange GetSizeFromTotalTileAreaRange()
    {
        return new IntRange(Mathf.CeilToInt(Mathf.Sqrt(TotalTileAreaRange.Min)),
            Mathf.CeilToInt(Mathf.Sqrt(TotalTileAreaRange.Max)));
    }
}
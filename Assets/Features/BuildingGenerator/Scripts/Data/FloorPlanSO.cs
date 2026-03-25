using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "FloorPlan", menuName = "FloorPlan")]
public class FloorPlanSO : ScriptableObject
{
    [field: SerializeField] public List<RoomGenerationData> RoomDataList { get; private set; } = new();

    [ShowInInspector] private IntRange _sizeRange => GetSizeFromTotalTileAreaRange();

    public List<RuntimeRoomData> GenerateRoomDataList()
    {
        List<RuntimeRoomData> result = new List<RuntimeRoomData>();
        foreach (RoomGenerationData data in RoomDataList)
            result.Add(data.GenerateRoom());
        return result;
    }
    
    private IntRange GetTotalTileAreaRange()
    {
        IntRange totalRange = new IntRange(0, 0);
        foreach (RoomGenerationData roomData in RoomDataList)
        {
            totalRange.Min += roomData.GetBaseRoomSize() + roomData.AdditionalSizeRange.Min;
            totalRange.Max += roomData.GetBaseRoomSize() + roomData.AdditionalSizeRange.Max;
        }

        return totalRange;
    }
    
    private IntRange GetSizeFromTotalTileAreaRange()
    {
        IntRange totalAreaRange = GetTotalTileAreaRange();
        return new IntRange(Mathf.CeilToInt(Mathf.Sqrt(totalAreaRange.Min)),
            Mathf.CeilToInt(Mathf.Sqrt(totalAreaRange.Max)));
    }
}
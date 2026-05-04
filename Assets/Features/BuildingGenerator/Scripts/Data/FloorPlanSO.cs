using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "FloorPlan", menuName = "FloorPlan")]
public class FloorPlanSO : ScriptableObject
{
    [field: SerializeField] public List<RoomGenerationData> RoomDataList { get; private set; } = new();

    [field: SerializeField] public SerializedDictionary<RoomType, float> RoomGlazeValues { get; private set; } = new();
    
    [ShowInInspector] private IntRange _sizeRange => GetSizeFromTotalTileAreaRange();

    private void OnValidate()
    {
        foreach (RoomGenerationData roomData in RoomDataList)
            roomData.OnValidate();
    }
    
    public List<GeneratedRoomData> GenerateRoomDataList()
    {
        List<GeneratedRoomData> result = new List<GeneratedRoomData>();
        foreach (RoomGenerationData data in RoomDataList)
            result.Add(data.GenerateRoom());
        return result;
    }
    
    private IntRange GetTotalTileAreaRange()
    {
        IntRange totalRange = new IntRange(0, 0);
        foreach (RoomGenerationData roomData in RoomDataList)
        {
            totalRange.Min += roomData.RoomSizeRange.Min;
            totalRange.Max += roomData.RoomSizeRange.Max;
        }

        return totalRange;
    }
    
    private IntRange GetSizeFromTotalTileAreaRange()
    {
        IntRange totalAreaRange = GetTotalTileAreaRange();
        return new IntRange(Mathf.CeilToInt(Mathf.Sqrt(totalAreaRange.Min)),
            Mathf.CeilToInt(Mathf.Sqrt(totalAreaRange.Max)));
    }

    public float GetGlaze(RoomType roomType)
    {
        return RoomGlazeValues.TryGetValue(roomType, out float value) ? value : 1;
    }
}
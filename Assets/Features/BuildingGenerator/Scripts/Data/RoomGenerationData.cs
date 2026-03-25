using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomGenerationData
{
    [field: SerializeField] public RoomType Type { get; private set; }
    [field: SerializeField] public IntRange AdditionalSizeRange { get; private set; } = new IntRange(0, 1);
    [field: SerializeField] public List<Appliance> RequiredAppliances { get; private set; } = new();
    // [field: SerializeField] public List<Appliance> OptionalAppliances { get; private set; } = new();

    public RuntimeRoomData GenerateRoom()
    {
        RuntimeRoomData roomData = new RuntimeRoomData();
        roomData.Type = Type;
        roomData.Size = GetBaseRoomSize() + AdditionalSizeRange.RandomValue();

        roomData.AppliancePrefabsToSpawn.AddRange(RequiredAppliances);
        
        return roomData;
    }

    public int GetBaseRoomSize()
    {
        return Mathf.CeilToInt(Mathf.Sqrt(RequiredAppliances.Count));
    }
}
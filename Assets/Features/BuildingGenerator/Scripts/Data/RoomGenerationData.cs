using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class RoomGenerationData
{
    [field: SerializeField] public RoomType Type { get; private set; }
    [field: SerializeField] public IntRange AdditionalAreaRange { get; private set; } = new IntRange(0, 1);
    [field: SerializeField] public List<Appliance> RequiredAppliances { get; private set; } = new();
    // [field: SerializeField] public List<Appliance> OptionalAppliances { get; private set; } = new();

    [ShowInInspector] public IntRange RoomSizeRange => new IntRange(CustomUtils.GetSizeFromArea(RequiredAppliances.Count + AdditionalAreaRange.Min), CustomUtils.GetSizeFromArea(RequiredAppliances.Count + AdditionalAreaRange.Max)); 

    public RuntimeRoomData GenerateRoom()
    {
        RuntimeRoomData roomData = new RuntimeRoomData();
        roomData.Type = Type;
        roomData.Size = CustomUtils.GetSizeFromArea(RequiredAppliances.Count + AdditionalAreaRange.RandomValue());

        roomData.AppliancePrefabsToSpawn.AddRange(RequiredAppliances);
        
        return roomData;
    }
}
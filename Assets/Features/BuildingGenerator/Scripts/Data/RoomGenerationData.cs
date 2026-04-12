using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class RoomGenerationData
{
    [field: SerializeField] public RoomType Type { get; private set; }
    [field: SerializeField] public List<Appliance> RequiredAppliances { get; private set; } = new();

    [SerializeField] private IntRange _roomSizeRange;
    public IntRange RoomSizeRange => _roomSizeRange;

    public void OnValidate()
    {
        int minSize = CustomUtils.GetSizeFromArea(RequiredAppliances.Count);
        if (_roomSizeRange.Min < minSize)
            _roomSizeRange.Min = minSize;
        if (_roomSizeRange.Max < _roomSizeRange.Min)
            _roomSizeRange.Max = _roomSizeRange.Min;
    }

    public RuntimeRoomData GenerateRoom()
    {
        RuntimeRoomData roomData = new RuntimeRoomData();
        roomData.Type = Type;
        roomData.Size = _roomSizeRange.RandomValueWithBounds();
        roomData.AppliancePrefabsToSpawn.AddRange(RequiredAppliances);
        return roomData;
    }
}
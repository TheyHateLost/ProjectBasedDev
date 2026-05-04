using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class RoomGenerationData
{
    [field: SerializeField] public RoomType Type { get; private set; }
    [field: SerializeField] public List<Appliance> RequiredAppliances { get; private set; } = new();
    
    [field: Header("Window")]
    [field: SerializeField] public List<Transform> WindowPrefabs { get; private set; } = new();
    [field: SerializeField] public IntRange WindowCountRange { get; private set; } = new IntRange(1, 3);

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

    public GeneratedRoomData GenerateRoom()
    {
        return RoomPlacementPlanner.GenerateRoomPlan(this);
    }
}
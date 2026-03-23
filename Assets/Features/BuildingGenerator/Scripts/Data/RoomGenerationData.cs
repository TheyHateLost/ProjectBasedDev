using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomGenerationData
{
    [field: SerializeField] public RoomType Type { get; private set; }
    [field: SerializeField] public IntRange SizeRange { get; private set; } = new IntRange(1, 2);
    [field: SerializeField] public List<Appliance> RequiredAppliances { get; private set; } = new();
    [field: SerializeField] public List<Appliance> OptionalAppliances { get; private set; } = new();

    public RoomData GenerateRoom()
    {
        RoomData roomData = new RoomData();
        roomData.Type = Type;
        roomData.Size = SizeRange.RandomValue();

        foreach (var requiredAppliance in RequiredAppliances)
        {
            
        }
        
        return roomData;
    }
}

[System.Serializable]
public class RoomData
{
    public RoomType Type;
    public int Size;
    public List<Appliance> SpawnedAppliances = new();

    public void Clear()
    {
        foreach(var appliance in SpawnedAppliances)
            GameObject.Destroy(appliance.gameObject);
        SpawnedAppliances.Clear();
    }
}
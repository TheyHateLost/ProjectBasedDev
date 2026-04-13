using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class BTUCalculator : MonoBehaviour
{
    [Header("References")]
    public BuildingGenerator generator;

    [Header("Results")]
    [ReadOnly] public RoomType roomType;
    [ReadOnly] public float roomLength;
    [ReadOnly] public float roomWidth;
    [ReadOnly] public float roomHeight;
    [ReadOnly] public float roomGlaze;
    [ReadOnly] public float firstRoomBTU;
    [ReadOnly] public float totalBuildingBTU;

    private void OnEnable()
    {
        generator.OnBuildingGenerated += OnBuildingGenerated;
    }

    private void OnDisable()
    {
        generator.OnBuildingGenerated -= OnBuildingGenerated;
    }

    private void OnBuildingGenerated()
    {
        CalculateFirstRoomThermalData();
        ProcessBuildingThermalData();
    }
    
    private void CalculateFirstRoomThermalData()
    {
        if (generator.CurrentRooms.Count == 0)
            return;

        roomType = generator.CurrentRooms[0].Type;
        roomLength = generator.CurrentRooms[0].Size;
        roomWidth = generator.CurrentRooms[0].Size;
        roomHeight = generator.RealWallHeight;
        roomGlaze = generator.FloorPlan.GetGlaze(generator.CurrentRooms[0].Type);
        firstRoomBTU = CustomUtils.CalculateBTU(roomWidth,  roomLength, roomHeight, roomGlaze);
    }
    
    private void ProcessBuildingThermalData()
    {
        totalBuildingBTU = 0;
        
        foreach (var room in generator.CurrentRooms)
        {
            float roomW = room.Size * generator.transform.localScale.x; 
            float roomL = room.Size * generator.transform.localScale.z;
            float roomH = generator.RealWallHeight;
            
            float roomBTU = CustomUtils.CalculateBTU(roomW, roomL, roomH, generator.FloorPlan.GetGlaze(room.Type));
            totalBuildingBTU += roomBTU;

            Debug.Log($"Calculated {room.Type}: {roomBTU} BTUs");
        }

        Debug.Log($"<b>Total Building BTU:</b> {totalBuildingBTU}");
    }
}
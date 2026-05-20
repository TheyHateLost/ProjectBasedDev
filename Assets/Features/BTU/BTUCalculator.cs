using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class BTUCalculator : MonoBehaviour
{
    [Header("References")]
    [Required] public BuildingGenerator Generator;

    [Header("Results")]
    [ReadOnly] public RoomType RoomType;
    [ReadOnly] public float RoomLength;
    [ReadOnly] public float RoomWidth;
    [ReadOnly] public float RoomHeight;
    [ReadOnly] public float RoomGlaze;
    [ReadOnly] public float CurrentRoomBTU;
    [ReadOnly] public float TotalBuildingBTU;

    private void OnEnable()
    {
        Generator.OnBuildingGenerated += OnBuildingGenerated;
        Generator.OnRoomChanged += OnRoomChanged;
    }

    private void OnDisable()
    {
        Generator.OnBuildingGenerated -= OnBuildingGenerated;
        Generator.OnRoomChanged -= OnRoomChanged;
    }

    private void OnBuildingGenerated()
    {
        CalculateCurrentRoomThermalData();
        ProcessBuildingThermalData();
    }
    
    private void OnRoomChanged(GeneratedRoomData newRoomData)
    {
        CalculateCurrentRoomThermalData();
    }
    
    private void CalculateCurrentRoomThermalData()
    {
        if (Generator.CurrentRooms.Count == 0)
            return;

        GeneratedRoomData currRoom = Generator.CurrentRooms[Generator.CurrentRoomIndex];
        
        RoomType = currRoom.Type;
        RoomLength = currRoom.Size;
        RoomWidth = currRoom.Size;
        RoomHeight = Generator.RealWallHeight;
        RoomGlaze = Generator.FloorPlan.GetGlaze(currRoom.Type);
        CurrentRoomBTU = CustomUtils.CalculateBTU(RoomWidth,  RoomLength, RoomHeight, RoomGlaze);
    }
    
    private void ProcessBuildingThermalData()
    {
        TotalBuildingBTU = 0;
        
        foreach (var room in Generator.CurrentRooms)
        {
            float roomW = room.Size * Generator.transform.localScale.x; 
            float roomL = room.Size * Generator.transform.localScale.z;
            float roomH = Generator.RealWallHeight;
            
            float roomBTU = CustomUtils.CalculateBTU(roomW, roomL, roomH, Generator.FloorPlan.GetGlaze(room.Type));
            TotalBuildingBTU += roomBTU;

            Debug.Log($"Calculated {room.Type}: {roomBTU} BTUs");
        }

        Debug.Log($"<b>Total Building BTU:</b> {TotalBuildingBTU}");
    }
}
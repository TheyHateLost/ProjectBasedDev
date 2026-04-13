using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class ThermalBuildingManager : MonoBehaviour
{
    [Header("References")]
    public BuildingGenerator generator;

    [Header("Results")]
    [ReadOnly] public float totalBuildingBTU;

    private void OnEnable()
    {
        generator.OnBuildingGenerated += ProcessBuildingThermalData;
    }

    private void OnDisable()
    {
        generator.OnBuildingGenerated -= ProcessBuildingThermalData;
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
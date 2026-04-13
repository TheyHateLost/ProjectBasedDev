using UnityEngine;
using System.Collections.Generic;

public class ThermalBuildingManager : MonoBehaviour
{
    [Header("References")]
    public BuildingGenerator generator;
    public BTUCalculator calculator;

    [Header("Results")]
    public float totalBuildingBTU;

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

            BTUCalculator.RoomType type = MapStringToEnum(room.Type.ToString());


            float roomBTU = calculator.CalculateBTU(type, roomW, roomL, roomH);
            totalBuildingBTU += roomBTU;

            Debug.Log($"Calculated {room.Type}: {roomBTU} BTUs");
        }

        Debug.Log($"<b>Total Building BTU:</b> {totalBuildingBTU}");
    }

    private BTUCalculator.RoomType MapStringToEnum(string typeName)
    {
        if (typeName.Contains("Bathroom")) return BTUCalculator.RoomType.Bathroom;
        return BTUCalculator.RoomType.Bathroom; // Default
    }
}
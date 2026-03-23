using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] private RoomGenerationData _roomGenerationData;
    private List<Appliance> _spawnedAppliances = new List<Appliance>();

    [Button("Generate Room", ButtonSizes.Large)]
    public void GenerateRoom()
    {
        ClearRoom();

        // Spawn all required appliances
        foreach (var appliance in _roomGenerationData.RequiredAppliances)
        {
            Appliance newAppliance = Instantiate(appliance, Vector3.zero, Quaternion.identity, transform);
            _spawnedAppliances.Add(newAppliance);
        }
    }

    private void ClearRoom()
    {
        foreach (var appliance in _spawnedAppliances)
            Destroy(appliance.gameObject);
        
        _spawnedAppliances.Clear();
    }
}
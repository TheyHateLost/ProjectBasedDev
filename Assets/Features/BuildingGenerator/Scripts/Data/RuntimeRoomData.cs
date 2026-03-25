using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RuntimeRoomData
{
    public RoomType Type;
    public int Size;
    public int Area => Size * Size;
    public List<Appliance> AppliancePrefabsToSpawn = new();
    public List<Appliance> SpawnedAppliances = new();

    public bool TrySpawnNextAppliance(Vector3 pos, Transform parent)
    {
        if(AppliancePrefabsToSpawn.Count <= 0)
            return false;
        
        Appliance appliance = GameObject.Instantiate(AppliancePrefabsToSpawn[0], pos, Quaternion.identity);
        SpawnedAppliances.Add(appliance);
        
        appliance.transform.SetParent(parent, true);
        
        AppliancePrefabsToSpawn.RemoveAt(0);
        
        return true;
    }
    
    public void Clear()
    {
        AppliancePrefabsToSpawn.Clear();
        
        foreach(var appliance in SpawnedAppliances)
            GameObject.Destroy(appliance.gameObject);
        SpawnedAppliances.Clear();
    }
}
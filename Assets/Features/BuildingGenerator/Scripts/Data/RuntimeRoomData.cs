using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class RuntimeRoomData
{
    public RoomType Type;
    public int Size;
    public int Area => Size * Size;
    public List<Appliance> AppliancePrefabsToSpawn = new();
    public List<Appliance> SpawnedAppliances = new();
    private HashSet<Vector2Int> _occupiedTiles = new();

    public bool TrySpawnNextAppliance(Vector2Int localPos, int roomSize, Transform tile, float floorWidth, float floorHeight)
    {
        if (AppliancePrefabsToSpawn.Count <= 0)
            return false;

        Appliance.Orientation wallOrientation = GetWallOrientation(localPos, roomSize);
        if (wallOrientation == Appliance.Orientation.None)
            return false;
        
        // Try each appliance (randomly ordered) until one fits
        List<int> indices = Enumerable.Range(0, AppliancePrefabsToSpawn.Count).OrderBy(_ => Random.value).ToList();

        foreach (int i in indices)
        {
            Appliance prefab = AppliancePrefabsToSpawn[i];

            // Remove the AllowedOrientations position filter entirely
            // if ((prefab.AllowedOrientations & wallOrientation) == 0)
            //     continue;

            Vector2Int axis = GetWallAxis(wallOrientation);
            Vector2Int inward = GetInwardDirection(wallOrientation);
            List<Vector2Int> footprint = GetFootprint(localPos, axis, inward, prefab.Size, roomSize);

            if (footprint == null || footprint.Any(t => _occupiedTiles.Contains(t)))
                continue;

            foreach (var t in footprint)
                _occupiedTiles.Add(t);

            Vector3 spawnPos = tile.position + Vector3.up * (floorHeight / 2f);
            if (footprint.Count > 1)
            {
                Vector3 axisOffset  = new Vector3(axis.x, 0, axis.y) * (floorWidth * (prefab.Size.x - 1));
                Vector3 depthOffset = new Vector3(inward.x, 0, inward.y) * (floorWidth * (prefab.Size.y - 1));
                Vector3 otherWorld  = tile.position + axisOffset + depthOffset;
                spawnPos = (tile.position + otherWorld) / 2f + Vector3.up * (floorHeight / 2f);
            }

            // wallOrientation drives position, AllowedOrientations drives which face touches the wall
            Quaternion rotation = GetRotationForOrientation(wallOrientation, prefab.AllowedOrientations);
            Appliance spawned = GameObject.Instantiate(prefab, spawnPos, rotation);
            spawned.transform.SetParent(tile, true);
            SpawnedAppliances.Add(spawned);
            AppliancePrefabsToSpawn.RemoveAt(i);
            return true;
        }
        
        return false;
    }

    private Appliance.Orientation GetWallOrientation(Vector2Int localPos, int roomSize)
    {
        if (localPos.x == 0)            return Appliance.Orientation.Back;
        if (localPos.x == roomSize - 1) return Appliance.Orientation.Front;
        if (localPos.y == 0)            return Appliance.Orientation.Left;
        if (localPos.y == roomSize - 1) return Appliance.Orientation.Right;
        return Appliance.Orientation.None;
    }

    private Vector2Int GetWallAxis(Appliance.Orientation orientation)
    {
        return orientation switch
        {
            Appliance.Orientation.Front or Appliance.Orientation.Back => new Vector2Int(0, 1),
            Appliance.Orientation.Left or Appliance.Orientation.Right => new Vector2Int(1, 0),
            _ => Vector2Int.zero
        };
    }

    private Vector2Int GetInwardDirection(Appliance.Orientation orientation)
    {
        return orientation switch
        {
            Appliance.Orientation.Back  => new Vector2Int(1, 0),
            Appliance.Orientation.Front => new Vector2Int(-1, 0),
            Appliance.Orientation.Left  => new Vector2Int(0, 1),
            Appliance.Orientation.Right => new Vector2Int(0, -1),
            _ => Vector2Int.zero
        };
    }
    
    private List<Vector2Int> GetFootprint(Vector2Int origin, Vector2Int axis, Vector2Int inward, Vector2Int size, int roomSize)
    {
        var tiles = new List<Vector2Int>();
        for (int x = 0; x < size.x; x++)       // X: along the wall
        {
            for (int y = 0; y < size.y; y++)    // Y: into the room (Z axis)
            {
                Vector2Int t = origin + axis * x + inward * y;
                if (t.x < 0 || t.x >= roomSize || t.y < 0 || t.y >= roomSize)
                    return null;
                tiles.Add(t);
            }
        }
        return tiles;
    }

    private Quaternion GetRotationForOrientation(Appliance.Orientation wallOrientation, Appliance.Orientation allowedOrientations)
    {
        // Pick a random allowed appliance face
        var allowed = new List<Appliance.Orientation>();
        if ((allowedOrientations & Appliance.Orientation.Back)  != 0) allowed.Add(Appliance.Orientation.Back);
        if ((allowedOrientations & Appliance.Orientation.Front) != 0) allowed.Add(Appliance.Orientation.Front);
        if ((allowedOrientations & Appliance.Orientation.Left)  != 0) allowed.Add(Appliance.Orientation.Left);
        if ((allowedOrientations & Appliance.Orientation.Right) != 0) allowed.Add(Appliance.Orientation.Right);

        Appliance.Orientation applianceFace = allowed[Random.Range(0, allowed.Count)];

        // What direction does the wall face inward (the direction the appliance should face away from)?
        // i.e. the wall normal pointing into the room
        float wallYaw = wallOrientation switch
        {
            Appliance.Orientation.Back  => 90f,
            Appliance.Orientation.Front => 270f,
            Appliance.Orientation.Left  => 0f,
            Appliance.Orientation.Right => 180f,
            _ => 0f
        };

        // How much to rotate the prefab so the chosen face points toward the wall
        float faceYaw = applianceFace switch
        {
            Appliance.Orientation.Back  => 0f,    // -Z already faces wall direction by default
            Appliance.Orientation.Front => 180f,  // rotate 180 so +Z faces wall
            Appliance.Orientation.Left  => 90f,   // rotate 90 so -X faces wall
            Appliance.Orientation.Right => 270f,  // rotate 270 so +X faces wall
            _ => 0f
        };

        return Quaternion.Euler(0, wallYaw + faceYaw, 0);
    }

    public void Clear()
    {
        Debug.Log($"Clearing room, occupied tiles was: {_occupiedTiles.Count}");
        _occupiedTiles.Clear();
        AppliancePrefabsToSpawn.Clear();

        foreach (var appliance in SpawnedAppliances)
            GameObject.Destroy(appliance.gameObject);
        SpawnedAppliances.Clear();
    }
}
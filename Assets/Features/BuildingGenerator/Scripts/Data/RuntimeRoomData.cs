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

    public List<Transform> WindowPrefabs = new();
    public int WindowCount;

    public bool TrySpawnNextAppliance(Vector2Int localPos, int roomSize, Transform tile, float floorWidth, float floorHeight)
    {
        if (AppliancePrefabsToSpawn.Count <= 0)
            return false;

        Appliance.Orientation wallOrientation = GetWallOrientation(localPos, roomSize);
        if (wallOrientation == Appliance.Orientation.None)
            return false;

        List<int> indices = Enumerable.Range(0, AppliancePrefabsToSpawn.Count).OrderBy(_ => Random.value).ToList();

        foreach (int i in indices)
        {
            Appliance prefab = AppliancePrefabsToSpawn[i];

            if (_occupiedTiles.Contains(localPos))
                continue;

            // Rotate first, then derive footprint directions.
            Quaternion rotation = GetRotationForOrientation(wallOrientation, prefab.AllowedOrientations);
            Vector3 worldRight   = rotation * Vector3.right;
            Vector3 worldForward = rotation * Vector3.forward;
            Vector2Int extendRight   = new Vector2Int(Mathf.RoundToInt(worldRight.x),   Mathf.RoundToInt(worldRight.z));
            Vector2Int extendForward = new Vector2Int(Mathf.RoundToInt(worldForward.x), Mathf.RoundToInt(worldForward.z));

            bool fits = true;
            List<Vector2Int> footprint = new List<Vector2Int> { localPos };

            // Check width along local right.
            for (int x = 1; x < prefab.Size.x; x++)
            {
                Vector2Int t = localPos + extendRight * x;
                if (t.x < 0 || t.x >= roomSize || t.y < 0 || t.y >= roomSize || _occupiedTiles.Contains(t))
                {
                    fits = false;
                    break;
                }
                footprint.Add(t);
            }

            if (!fits) continue;

            // Check depth along local forward.
            for (int y = 1; y < prefab.Size.y; y++)
            {
                Vector2Int t = localPos + extendForward * y;
                if (t.x < 0 || t.x >= roomSize || t.y < 0 || t.y >= roomSize || _occupiedTiles.Contains(t))
                {
                    fits = false;
                    break;
                }
                footprint.Add(t);

                for (int x = 1; x < prefab.Size.x; x++)
                {
                    Vector2Int ti = t + extendRight * x;
                    if (ti.x < 0 || ti.x >= roomSize || ti.y < 0 || ti.y >= roomSize || _occupiedTiles.Contains(ti))
                    {
                        fits = false;
                        break;
                    }
                    footprint.Add(ti);
                }

                if (!fits) break;
            }

            if (!fits) continue;

            foreach (var t in footprint)
                _occupiedTiles.Add(t);

            Vector3 spawnPos = tile.position + Vector3.up * (floorHeight / 2f);
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
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector2Int t = origin + inward * x + axis * y;
                if (t.x < 0 || t.x >= roomSize || t.y < 0 || t.y >= roomSize)
                    return null;
                tiles.Add(t);
            }
        }
        return tiles;
    }

    private Quaternion GetRotationForOrientation(Appliance.Orientation wallOrientation, Appliance.Orientation allowedOrientations)
    {
        // Pick a random allowed face.
        var allowed = new List<Appliance.Orientation>();
        if ((allowedOrientations & Appliance.Orientation.Back)  != 0) allowed.Add(Appliance.Orientation.Back);
        if ((allowedOrientations & Appliance.Orientation.Front) != 0) allowed.Add(Appliance.Orientation.Front);
        if ((allowedOrientations & Appliance.Orientation.Left)  != 0) allowed.Add(Appliance.Orientation.Left);
        if ((allowedOrientations & Appliance.Orientation.Right) != 0) allowed.Add(Appliance.Orientation.Right);

        Appliance.Orientation applianceFace = allowed[Random.Range(0, allowed.Count)];

        // Wall normal pointing into the room.
        float wallYaw = wallOrientation switch
        {
            Appliance.Orientation.Back  => 90f,
            Appliance.Orientation.Front => 270f,
            Appliance.Orientation.Left  => 0f,
            Appliance.Orientation.Right => 180f,
            _ => 0f
        };

        // Rotate so the chosen face points toward the wall.
        float faceYaw = applianceFace switch
        {
            Appliance.Orientation.Back  => 0f,
            Appliance.Orientation.Front => 180f,
            Appliance.Orientation.Left  => 90f,
            Appliance.Orientation.Right => 270f,
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
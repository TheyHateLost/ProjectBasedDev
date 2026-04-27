using System.Collections.Generic;
using UnityEngine;

public static class RoomPlacementPlanner
{
    private struct WallSlot
    {
        public Vector2Int Position;
        public WallOrientation Orientation;

        public WallSlot(Vector2Int position, WallOrientation orientation)
        {
            Position = position;
            Orientation = orientation;
        }
    }

    public static GeneratedRoomData GenerateRoomPlan(RoomGenerationData source)
    {
        // Build the room as pure data first so the renderer stays replaceable.
        GeneratedRoomData room = new GeneratedRoomData
        {
            Type = source.Type,
            Size = source.RoomSizeRange.RandomValueWithBounds()
        };

        List<Transform> availableWindowPrefabs = new List<Transform>(source.WindowPrefabs);
        List<Appliance> remainingAppliances = new List<Appliance>(source.RequiredAppliances);
        HashSet<Vector2Int> occupiedTiles = new HashSet<Vector2Int>();

        List<Vector2Int> edgeWallTiles = GetEdgeWallTiles(room.Size);
        Shuffle(edgeWallTiles);

        // Use all wall slots for window placement. Corners contribute two slots (one per wall).
        List<WallSlot> allWallSlots = GetAllWallSlots(room.Size);
        Shuffle(allWallSlots);
        int windowsToPlace = Mathf.Min(source.WindowCountRange.RandomValueWithBounds(), allWallSlots.Count);
        for (int i = 0; i < windowsToPlace && availableWindowPrefabs.Count > 0; i++)
        {
            WallSlot selectedSlot = allWallSlots[i];
            Transform windowPrefab = availableWindowPrefabs[Random.Range(0, availableWindowPrefabs.Count)];

            room.WindowPlacements.Add(new WindowPlacementData
            {
                WindowPrefab = windowPrefab,
                LocalPosition = selectedSlot.Position,
                Orientation = selectedSlot.Orientation,
                Rotation = GetWallRotation(selectedSlot.Orientation)
            });
        }

        // Place appliances after windows so the room layout can respect both sets of constraints.
        List<Vector2Int> wallTiles = GetAllWallTiles(room.Size);
        Shuffle(wallTiles);

        foreach (Vector2Int localPos in wallTiles)
        {
            if (remainingAppliances.Count == 0)
                break;

            TryPlaceAppliance(room, localPos, room.Size, remainingAppliances, occupiedTiles);
        }

        return room;
    }

    private static bool TryPlaceAppliance(GeneratedRoomData room, Vector2Int localPos, int roomSize, List<Appliance> remainingAppliances, HashSet<Vector2Int> occupiedTiles)
    {
        ApplianceOrientation wallOrientation = GetWallOrientation(localPos, roomSize);
        if (wallOrientation == ApplianceOrientation.None)
            return false;

        List<int> shuffledIndices = GetShuffledIndices(remainingAppliances.Count);

        foreach (int index in shuffledIndices)
        {
            Appliance prefab = remainingAppliances[index];

            if (occupiedTiles.Contains(localPos))
                continue;

            Quaternion rotation = GetRotationForOrientation(wallOrientation, prefab.AllowedOrientations);
            Vector3 worldRight = rotation * Vector3.right;
            Vector3 worldForward = rotation * Vector3.forward;
            Vector2Int extendRight = new Vector2Int(Mathf.RoundToInt(worldRight.x), Mathf.RoundToInt(worldRight.z));
            Vector2Int extendForward = new Vector2Int(Mathf.RoundToInt(worldForward.x), Mathf.RoundToInt(worldForward.z));

            bool fits = true;
            List<Vector2Int> footprint = new List<Vector2Int> { localPos };

            for (int x = 1; x < prefab.Size.x; x++)
            {
                Vector2Int tile = localPos + extendRight * x;
                if (!IsInsideRoom(tile, roomSize) || occupiedTiles.Contains(tile))
                {
                    fits = false;
                    break;
                }
                footprint.Add(tile);
            }

            if (!fits)
                continue;

            for (int y = 1; y < prefab.Size.y; y++)
            {
                Vector2Int tile = localPos + extendForward * y;
                if (!IsInsideRoom(tile, roomSize) || occupiedTiles.Contains(tile))
                {
                    fits = false;
                    break;
                }

                footprint.Add(tile);

                for (int x = 1; x < prefab.Size.x; x++)
                {
                    Vector2Int innerTile = tile + extendRight * x;
                    if (!IsInsideRoom(innerTile, roomSize) || occupiedTiles.Contains(innerTile))
                    {
                        fits = false;
                        break;
                    }

                    footprint.Add(innerTile);
                }

                if (!fits)
                    break;
            }

            if (!fits)
                continue;

            foreach (Vector2Int tile in footprint)
                occupiedTiles.Add(tile);

            room.AppliancePlacements.Add(new AppliancePlacementData
            {
                Prefab = prefab,
                LocalPosition = localPos,
                Rotation = rotation,
                Footprint = footprint
            });

            remainingAppliances.RemoveAt(index);
            return true;
        }

        return false;
    }

    private static List<Vector2Int> GetEdgeWallTiles(int roomSize)
    {
        List<Vector2Int> edgeWallTiles = new List<Vector2Int>();
        for (int r = 0; r < roomSize; r++)
        {
            for (int c = 0; c < roomSize; c++)
            {
                bool isEdge = r == 0 || r == roomSize - 1 || c == 0 || c == roomSize - 1;
                bool isCorner = (r == 0 || r == roomSize - 1) && (c == 0 || c == roomSize - 1);

                if (isEdge && !isCorner)
                    edgeWallTiles.Add(new Vector2Int(r, c));
            }
        }

        return edgeWallTiles;
    }

    private static List<Vector2Int> GetAllWallTiles(int roomSize)
    {
        List<Vector2Int> wallTiles = new List<Vector2Int>();
        for (int r = 0; r < roomSize; r++)
        {
            for (int c = 0; c < roomSize; c++)
            {
                if (r == 0 || r == roomSize - 1 || c == 0 || c == roomSize - 1)
                    wallTiles.Add(new Vector2Int(r, c));
            }
        }

        return wallTiles;
    }

    private static List<WallSlot> GetAllWallSlots(int roomSize)
    {
        List<WallSlot> wallSlots = new List<WallSlot>();
        for (int r = 0; r < roomSize; r++)
        {
            for (int c = 0; c < roomSize; c++)
            {
                bool isTop = r == 0;
                bool isBottom = r == roomSize - 1;
                bool isLeft = c == 0;
                bool isRight = c == roomSize - 1;

                if (!(isTop || isBottom || isLeft || isRight))
                    continue;

                Vector2Int pos = new Vector2Int(r, c);

                if (isTop)
                    wallSlots.Add(new WallSlot(pos, WallOrientation.Top));
                if (isBottom)
                    wallSlots.Add(new WallSlot(pos, WallOrientation.Bottom));
                if (isLeft)
                    wallSlots.Add(new WallSlot(pos, WallOrientation.Left));
                if (isRight)
                    wallSlots.Add(new WallSlot(pos, WallOrientation.Right));
            }
        }

        return wallSlots;
    }

    private static bool IsInsideRoom(Vector2Int tile, int roomSize)
    {
        return tile.x >= 0 && tile.x < roomSize && tile.y >= 0 && tile.y < roomSize;
    }

    private static List<int> GetShuffledIndices(int count)
    {
        List<int> indices = new List<int>(count);
        for (int i = 0; i < count; i++)
            indices.Add(i);

        Shuffle(indices);
        return indices;
    }

    private static void Shuffle<T>(IList<T> values)
    {
        for (int i = values.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (values[i], values[j]) = (values[j], values[i]);
        }
    }

    private static ApplianceOrientation GetWallOrientation(Vector2Int localPos, int roomSize)
    {
        if (localPos.x == 0)
            return ApplianceOrientation.Back;
        if (localPos.x == roomSize - 1)
            return ApplianceOrientation.Front;
        if (localPos.y == 0)
            return ApplianceOrientation.Left;
        if (localPos.y == roomSize - 1)
            return ApplianceOrientation.Right;

        return ApplianceOrientation.None;
    }

    private static Quaternion GetWallRotation(WallOrientation orientation)
    {
        return orientation switch
        {
            WallOrientation.Top => Quaternion.Euler(0, 180, 0),
            WallOrientation.Bottom => Quaternion.identity,
            WallOrientation.Left => Quaternion.Euler(0, 90, 0),
            WallOrientation.Right => Quaternion.Euler(0, 270, 0),
            _ => Quaternion.identity
        };
    }

    private static Quaternion GetRotationForOrientation(ApplianceOrientation wallOrientation, ApplianceOrientation allowedOrientations)
    {
        List<ApplianceOrientation> allowed = new List<ApplianceOrientation>();
        if ((allowedOrientations & ApplianceOrientation.Back) != 0)
            allowed.Add(ApplianceOrientation.Back);
        if ((allowedOrientations & ApplianceOrientation.Front) != 0)
            allowed.Add(ApplianceOrientation.Front);
        if ((allowedOrientations & ApplianceOrientation.Left) != 0)
            allowed.Add(ApplianceOrientation.Left);
        if ((allowedOrientations & ApplianceOrientation.Right) != 0)
            allowed.Add(ApplianceOrientation.Right);

        ApplianceOrientation applianceFace = allowed[Random.Range(0, allowed.Count)];

        float wallYaw = wallOrientation switch
        {
            ApplianceOrientation.Back => 90f,
            ApplianceOrientation.Front => 270f,
            ApplianceOrientation.Left => 0f,
            ApplianceOrientation.Right => 180f,
            _ => 0f
        };

        float faceYaw = applianceFace switch
        {
            ApplianceOrientation.Back => 0f,
            ApplianceOrientation.Front => 180f,
            ApplianceOrientation.Left => 90f,
            ApplianceOrientation.Right => 270f,
            _ => 0f
        };

        return Quaternion.Euler(0, wallYaw + faceYaw, 0);
    }
}

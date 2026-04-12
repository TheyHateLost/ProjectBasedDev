using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private bool _willGenerateOnStart = true;

    [Header("Template")]
    [SerializeField, Required] private Transform _floorPrefab;
    [SerializeField, Required] private Transform _rightEdgeWallPrefab;
    [SerializeField, Required] private Transform _topRightCornerWallPrefab;
    [SerializeField, ReadOnly] private float _floorWidth;
    [SerializeField, ReadOnly] private float _floorHeight;
    [SerializeField, ReadOnly] private float _wallHeight;
    [field: SerializeField, ReadOnly] public float RealWallHeight { get; private set; }

    [Header("Floor Plan")]
    [SerializeField] private FloorPlanSO _floorPlan;

    [Header("Building Size")]
    [SerializeField] private IntRange _buildingSizeRange = new IntRange(10, 20);
    [SerializeField, Required] private Material _ghostMaterial;
    [SerializeField, ReadOnly] private int _currentBuildingSize;

    [Header("Current Buildings")]
    public List<GameObject> SpawnedBuildings { get; private set; } = new();
    public event Action OnBuildingGenerated = delegate { };

    [field: Header("Current Rooms")]
    [field: SerializeField, ReadOnly] public List<RuntimeRoomData> CurrentRooms { get; private set; }

    private HashSet<Vector2Int> _occupiedTiles = new();

    private void Awake()
    {
        _floorWidth = _floorPrefab.localScale.x;
        _floorHeight = _floorPrefab.localScale.y;
        _wallHeight = _rightEdgeWallPrefab.GetChild(0).localScale.y;
        RealWallHeight = _wallHeight / 2f;
    }

    private void Start()
    {
        if (_willGenerateOnStart)
            GenerateNewBuilding();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            GenerateNewBuilding();
    }

    [Button("Generate Building", ButtonSizes.Large)]
    public void GenerateNewBuilding()
    {
        DestroyRooms();
        ClearCurrentBuildings();
        _occupiedTiles.Clear();

        _currentBuildingSize = _buildingSizeRange.RandomValue();

        GenerateRooms();
        SpawnBuildings();
        SpawnGhostTiles();

        OnBuildingGenerated?.Invoke();
    }

    private void ClearCurrentBuildings()
    {
        foreach (var building in SpawnedBuildings)
            Destroy(building);

        SpawnedBuildings.Clear();
    }

    private void SpawnBuildings()
    {
        Vector2Int nextOrigin = Vector2Int.zero;

        foreach (var room in CurrentRooms)
        {
            int roomSize = room.Size;

            GameObject buildingParent = new GameObject($"Room_{room.Type}");
            buildingParent.transform.parent = transform;

            Transform[,] tiles = new Transform[roomSize, roomSize];
            for (int r = 0; r < roomSize; r++)
            {
                for (int c = 0; c < roomSize; c++)
                {
                    Vector2Int tilePos = nextOrigin + new Vector2Int(r, c);
                    Transform tile = SpawnFloorObject(tilePos, roomSize, r, c);
                    tile.SetParent(buildingParent.transform, true);
                    tiles[r, c] = tile;
                    _occupiedTiles.Add(tilePos);
                }
            }

            List<(Vector2Int local, Transform tile)> wallTiles = new();
            for (int r = 0; r < roomSize; r++)
            {
                for (int c = 0; c < roomSize; c++)
                {
                    if (r == 0 || r == roomSize - 1 || c == 0 || c == roomSize - 1)
                        wallTiles.Add((new Vector2Int(r, c), tiles[r, c]));
                }
            }

            for (int i = wallTiles.Count - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                (wallTiles[i], wallTiles[j]) = (wallTiles[j], wallTiles[i]);
            }

            foreach (var (local, tile) in wallTiles)
                room.TrySpawnNextAppliance(local, roomSize, tile, _floorWidth, _floorHeight);

            SpawnedBuildings.Add(buildingParent);
            nextOrigin += new Vector2Int(roomSize, 0);
        }
    }

    private void SpawnGhostTiles()
    {
        int buildingSize = _currentBuildingSize;

        GameObject ghostParent = new GameObject("Room_Ghost");
        ghostParent.transform.parent = transform;

        for (int r = 0; r < buildingSize; r++)
        {
            for (int c = 0; c < buildingSize; c++)
            {
                Vector2Int tilePos = new Vector2Int(r, c);
                if (_occupiedTiles.Contains(tilePos))
                    continue;

                bool isEdgeR0 = r == 0;
                bool isEdgeR1 = r == buildingSize - 1;
                bool isEdgeC0 = c == 0;
                bool isEdgeC1 = c == buildingSize - 1;
                bool isWall = isEdgeR0 || isEdgeR1 || isEdgeC0 || isEdgeC1;

                int localRoomSize = buildingSize;
                int localR = isWall ? r : -1;
                int localC = isWall ? c : -1;

                Transform tile = SpawnFloorObject(tilePos, localRoomSize, r, c, isGhost: true);
                tile.SetParent(ghostParent.transform, true);
            }
        }

        SpawnedBuildings.Add(ghostParent);
    }

    private Transform SpawnFloorObject(Vector2Int targetGridPos, int roomSize, int localR, int localC, bool isGhost = false)
    {
        Vector3 targetWorldPos = GetWorldPosition(targetGridPos);
        Transform spawnedFloor = Instantiate(_floorPrefab, targetWorldPos, Quaternion.identity);

        bool isCornerTL = localR == 0 && localC == 0;
        bool isCornerBL = localR == roomSize - 1 && localC == 0;
        bool isCornerBR = localR == roomSize - 1 && localC == roomSize - 1;
        bool isCornerTR = localR == 0 && localC == roomSize - 1;

        Transform wall = null;

        if (isCornerTL)
            wall = Instantiate(_topRightCornerWallPrefab, spawnedFloor.position, Quaternion.Euler(0, 180, 0));
        else if (isCornerBL)
            wall = Instantiate(_topRightCornerWallPrefab, spawnedFloor.position, Quaternion.Euler(0, 90, 0));
        else if (isCornerBR)
            wall = Instantiate(_topRightCornerWallPrefab, spawnedFloor.position, Quaternion.identity);
        else if (isCornerTR)
            wall = Instantiate(_topRightCornerWallPrefab, spawnedFloor.position, Quaternion.Euler(0, 270, 0));
        else if (localR == 0)
            wall = Instantiate(_rightEdgeWallPrefab, spawnedFloor.position, Quaternion.Euler(0, 180, 0));
        else if (localR == roomSize - 1)
            wall = Instantiate(_rightEdgeWallPrefab, spawnedFloor.position, Quaternion.identity);
        else if (localC == 0)
            wall = Instantiate(_rightEdgeWallPrefab, spawnedFloor.position, Quaternion.Euler(0, 90, 0));
        else if (localC == roomSize - 1)
            wall = Instantiate(_rightEdgeWallPrefab, spawnedFloor.position, Quaternion.Euler(0, 270, 0));

        if (wall != null)
            wall.SetParent(spawnedFloor, true);

        if (isGhost)
            CustomUtils.SetMaterialRecursive(spawnedFloor.gameObject, _ghostMaterial);
        
        return spawnedFloor;
    }

    public Vector3 GetWorldPosition(Vector2Int gridPosition)
    {
        return new Vector3(_floorWidth * gridPosition.x, 0, _floorWidth * gridPosition.y);
    }
    
    public Vector3 GetFirstRoomCenterPosition()
    {
        if (SpawnedBuildings.Count == 0)
            return Vector3.zero;

        GameObject firstRoom = SpawnedBuildings[0];
        Bounds bounds = new Bounds();
        bool initialized = false;

        foreach (Transform tile in firstRoom.transform)
        {
            if (!initialized)
            {
                bounds = new Bounds(tile.position, Vector3.zero);
                initialized = true;
            }
            else
            {
                bounds.Encapsulate(tile.position);
            }
        }

        return bounds.center;
    }

    public Vector3 GetBuildingCenterWorldPosition()
    {
        if (SpawnedBuildings.Count == 0)
            return Vector3.zero;

        Vector3 min = new Vector3(float.MaxValue, 0, float.MaxValue);
        Vector3 max = new Vector3(float.MinValue, 0, float.MinValue);

        foreach (var building in SpawnedBuildings)
        {
            foreach (Transform tile in building.transform)
            {
                Vector3 pos = tile.position;
                min = Vector3.Min(min, pos);
                max = Vector3.Max(max, pos);
            }
        }

        return (min + max) / 2f;
    }

    private void DestroyRooms()
    {
        foreach (var rooms in CurrentRooms)
            rooms.Clear();
        CurrentRooms.Clear();
    }

    private void GenerateRooms()
    {
        CurrentRooms = _floorPlan.GenerateRoomDataList();
    }
}
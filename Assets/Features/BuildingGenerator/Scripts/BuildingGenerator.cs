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

    [SerializeField] private FloorPlanSO _floorPlan;

    [Header("Current Buildings")]
    public List<GameObject> SpawnedBuildings { get; private set; } = new();
    public event Action OnBuildingGenerated = delegate { };

    [Header("Current Rooms")]
    [SerializeField, ReadOnly] private List<RuntimeRoomData> _currentRooms;

    private void Awake()
    {
        _floorWidth = _floorPrefab.localScale.x;
        _floorHeight = _floorPrefab.localScale.y;
        _wallHeight = _rightEdgeWallPrefab.GetChild(0).localScale.y;
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

        GenerateRooms();
        SpawnBuildings();

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

        foreach (var room in _currentRooms)
        {
            int roomSize = Mathf.CeilToInt(Mathf.Sqrt(room.Area));

            GameObject buildingParent = new GameObject($"Room_{room.Type}");
            buildingParent.transform.parent = transform;

            for (int r = 0; r < roomSize; r++)
            {
                for (int c = 0; c < roomSize; c++)
                {
                    Vector2Int tilePos = nextOrigin + new Vector2Int(r, c);

                    // Pass local r/c for wall type calculation
                    Transform tile = SpawnFloorObject(tilePos, roomSize, r, c);

                    tile.SetParent(buildingParent.transform, true);

                    if (r == 0 || r == roomSize - 1 || c == 0 || c == roomSize - 1)
                        room.TrySpawnNextAppliance(tile.position + Vector3.up * (_floorHeight / 2f), tile);
                }
            }

            SpawnedBuildings.Add(buildingParent);

            // Update origin for next room to be adjacent on the X axis
            nextOrigin += new Vector2Int(roomSize, 0);
        }
    }

    private Transform SpawnFloorObject(Vector2Int targetGridPos, int roomSize, int localR, int localC)
    {
        Vector3 targetWorldPos = GetWorldPosition(targetGridPos);
        Transform spawnedFloor = Instantiate(_floorPrefab, targetWorldPos, Quaternion.identity);

        // Use localR/localC to decide wall type
        if (localR == 0 && localC == 0)
            Instantiate(_topRightCornerWallPrefab, spawnedFloor.position, Quaternion.Euler(0, 180, 0)).SetParent(spawnedFloor, true);
        else if (localR == roomSize - 1 && localC == 0)
            Instantiate(_topRightCornerWallPrefab, spawnedFloor.position, Quaternion.Euler(0, 90, 0)).SetParent(spawnedFloor, true);
        else if (localR == roomSize - 1 && localC == roomSize - 1)
            Instantiate(_topRightCornerWallPrefab, spawnedFloor.position, Quaternion.identity).SetParent(spawnedFloor, true);
        else if (localR == 0 && localC == roomSize - 1)
            Instantiate(_topRightCornerWallPrefab, spawnedFloor.position, Quaternion.Euler(0, 270, 0)).SetParent(spawnedFloor, true);
        else if (localR == 0)
            Instantiate(_rightEdgeWallPrefab, spawnedFloor.position, Quaternion.Euler(0, 180, 0)).SetParent(spawnedFloor, true);
        else if (localR == roomSize - 1)
            Instantiate(_rightEdgeWallPrefab, spawnedFloor.position, Quaternion.identity).SetParent(spawnedFloor, true);
        else if (localC == 0)
            Instantiate(_rightEdgeWallPrefab, spawnedFloor.position, Quaternion.Euler(0, 90, 0)).SetParent(spawnedFloor, true);
        else if (localC == roomSize - 1)
            Instantiate(_rightEdgeWallPrefab, spawnedFloor.position, Quaternion.Euler(0, 270, 0)).SetParent(spawnedFloor, true);

        return spawnedFloor;
    }

    public Vector3 GetWorldPosition(Vector2Int gridPosition)
    {
        return new Vector3(_floorWidth * gridPosition.x, 0, _floorWidth * gridPosition.y);
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
        foreach (var rooms in _currentRooms)
            rooms.Clear();
        _currentRooms.Clear();
    }

    private void GenerateRooms()
    {
        _currentRooms = _floorPlan.GenerateRoomDataList();
    }
}
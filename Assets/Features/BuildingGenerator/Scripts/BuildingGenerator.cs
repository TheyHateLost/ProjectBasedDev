using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private IntRange _floorCountRange = new IntRange(1, 3);
    [SerializeField] private IntRange _sizeRange = new IntRange(3, 5);
    [SerializeField] private bool _willGenerateOnStart = true;
    
    [Header("Template")]
    [SerializeField, Required] private Transform _floorPrefab;
    [SerializeField, Required] private Transform _rightEdgeWallPrefab;
    [SerializeField, Required] private Transform _topRightCornerWallPrefab;
    [SerializeField, ReadOnly] private float _floorWidth;
    [SerializeField, ReadOnly] private float _wallHeight;
    [SerializeField, ReadOnly] private float _floorYOffset;
    
    [Header("Current Building")]
    [SerializeField, ReadOnly] private int _floors;
    [SerializeField, ReadOnly] private int _size;
    public List<Dictionary<Vector2Int, GameObject>> SpawnedFloors { get; private set; } = new();
    public event Action OnBuildingGenerated = delegate { }; 

    private void Awake()
    {
        _floorWidth = _floorPrefab.localScale.x;
        _wallHeight = _rightEdgeWallPrefab.GetChild(0).localScale.y;
        _floorYOffset = _wallHeight + _floorPrefab.localScale.y;
    }

    private void Start()
    {
        if(_willGenerateOnStart)
            GenerateNewBuilding();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
            GenerateNewBuilding();
    }

    [Button("Generate Building", ButtonSizes.Large)]
    public void GenerateNewBuilding()
    {
        ClearCurrentBuilding();
        
        _floors = _floorCountRange.RandomValue();
        _size = _sizeRange.RandomValue();
        SpawnFloors();
        
        OnBuildingGenerated?.Invoke();
    }

    private void ClearCurrentBuilding()
    {
        foreach (var spawnedFloor in SpawnedFloors)
        {
            foreach (GameObject floorObject in spawnedFloor.Values)
            {
                Destroy(floorObject);
            }
        }
        SpawnedFloors.Clear();
    }

    private void SpawnFloors()
    {
        for (int floor = 0; floor < _floors + 1; floor++)
        {
            SpawnedFloors.Add(new Dictionary<Vector2Int, GameObject>());

            for (int r = 0; r < _size; r++)
            {
                for (int c = 0; c < _size; c++)
                {
                    Vector2Int targetGridPos = new Vector2Int(r, c);
                    SpawnedFloors[floor].Add(targetGridPos, SpawnFloorObject(targetGridPos, floor, floor != _floors));
                }
            }
        }
    }
    
    private GameObject SpawnFloorObject(Vector2Int targetGridPos, int floor, bool willSpawnWalls = true)
    {
        int r = targetGridPos.x;
        int c = targetGridPos.y;
        
        Vector3 targetWorldPos = GetWorldPosition(targetGridPos).WithY(floor * _floorYOffset);
        Transform spawnedFloor = Instantiate(_floorPrefab, targetWorldPos, Quaternion.identity, transform);

        if (willSpawnWalls) // not ceiling
        {
            if (r == 0 && c == 0) // bottom left corner
            {
                Instantiate(_topRightCornerWallPrefab, spawnedFloor.position, Quaternion.Euler(0, 180, 0)).SetParent(spawnedFloor, true);
            }
            else if (r == _size - 1 && c == 0) // top left
            {
                Instantiate(_topRightCornerWallPrefab, spawnedFloor.position, Quaternion.Euler(0, 90, 0)).SetParent(spawnedFloor, true);
            }
            else if (r == _size - 1 && c == _size - 1) // top right
            {
                Instantiate(_topRightCornerWallPrefab, spawnedFloor.position, Quaternion.identity).SetParent(spawnedFloor, true);
            }
            else if (r == 0 && c == _size - 1) // bottom right
            {
                Instantiate(_topRightCornerWallPrefab, spawnedFloor.position, Quaternion.Euler(0, 270, 0)).SetParent(spawnedFloor, true);
            }
            else if (r == 0) // left edge
            {
                Instantiate(_rightEdgeWallPrefab, spawnedFloor.position, Quaternion.Euler(0, 180, 0)).SetParent(spawnedFloor, true);
            }
            else if (r == _size - 1) // right edge
            {
                Instantiate(_rightEdgeWallPrefab, spawnedFloor.position, Quaternion.identity).SetParent(spawnedFloor, true);
            }
            else if (c == 0) // bottom edge
            {
                Instantiate(_rightEdgeWallPrefab, spawnedFloor.position, Quaternion.Euler(0, 90, 0)).SetParent(spawnedFloor, true);
            }
            else if (c == _size - 1) // top edge
            {
                Instantiate(_rightEdgeWallPrefab, spawnedFloor.position, Quaternion.Euler(0, 270, 0)).SetParent(spawnedFloor, true);
            }
        }

        return spawnedFloor.gameObject;
    }
    
    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        return new Vector2Int(Mathf.RoundToInt(worldPosition.x), Mathf.RoundToInt(worldPosition.z));
    }
    
    public Vector3 GetWorldPosition(Vector2Int gridPosition)
    {
        return new Vector3(_floorWidth * gridPosition.x, 0, _floorWidth * gridPosition.y);
    }

    public Vector3 GetBuildingCenterWorldPosition()
    {
        return GetWorldPosition(_size * Vector2Int.one).WithY(_floors * _floorYOffset) / 2f - (_floorWidth/2  * new Vector3(1,0,1));
    }
}
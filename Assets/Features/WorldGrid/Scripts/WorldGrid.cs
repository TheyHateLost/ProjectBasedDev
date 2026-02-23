using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldGrid : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private Transform _gridTilePrefab;
    private float _gridSpacing;
    
    private Dictionary<Vector2Int, Transform> _spawnedGridTiles = new Dictionary<Vector2Int, Transform>();

    private void Awake()
    {
        _gridSpacing = _gridTilePrefab.localScale.x;
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        return new Vector2Int(Mathf.RoundToInt(worldPosition.x), Mathf.RoundToInt(worldPosition.z));
    }
    
    public Vector3 GetWorldPosition(Vector2Int gridPosition)
    {
        return new Vector3(_gridSpacing * gridPosition.x, 0, _gridSpacing * gridPosition.y);
    }

    [Button("Generate Floor", ButtonSizes.Large)]
    public void GenerateFloor(int tileCount)
    {
        ClearGridTiles();
        
        if (tileCount <= 0)
            return;
        
        SpawnGridTile(0, 0);
        for (int i = 0; i < tileCount - 1; i++)
        {
            Vector2Int randomGridPosition = GetRandomAvailableEdgeGridPosition();
            SpawnGridTile(randomGridPosition);
        }
    }

    public void ClearGridTiles()
    {
        foreach (var kvp in _spawnedGridTiles)
        {
            Destroy(kvp.Value.gameObject);
        }
        _spawnedGridTiles.Clear();
    }

    private void SpawnGridTile(Vector2Int gridPosition)
    {
        if (_spawnedGridTiles.ContainsKey(gridPosition))
        {
            Debug.LogWarning($"Grid already spawned at {gridPosition}");
            return;
        }
        
        Transform spawnedTile = Instantiate(_gridTilePrefab, GetWorldPosition(gridPosition), Quaternion.identity);
        _spawnedGridTiles.Add(gridPosition, spawnedTile);
    }
    
    private void SpawnGridTile(int x, int y) => SpawnGridTile(new Vector2Int(x, y));

    /// <summary>
    /// Random EMPTY 4-adjacent candidate, biased toward being closer to (0,0).
    /// Bias approach: weight = 1 / (1 + distance^2). Closer => higher weight.
    /// </summary>
    private Vector2Int GetRandomAvailableEdgeGridPosition()
    {
        HashSet<Vector2Int> candidateSet = new HashSet<Vector2Int>();

        foreach (var pos in _spawnedGridTiles.Keys)
        {
            Vector2Int up = pos + Vector2Int.up;
            Vector2Int down = pos + Vector2Int.down;
            Vector2Int left = pos + Vector2Int.left;
            Vector2Int right = pos + Vector2Int.right;

            if (!_spawnedGridTiles.ContainsKey(up)) candidateSet.Add(up);
            if (!_spawnedGridTiles.ContainsKey(down)) candidateSet.Add(down);
            if (!_spawnedGridTiles.ContainsKey(left)) candidateSet.Add(left);
            if (!_spawnedGridTiles.ContainsKey(right)) candidateSet.Add(right);
        }

        if (candidateSet.Count == 0)
            return Vector2Int.zero;

        // Convert to list for indexed access.
        List<Vector2Int> candidates = new List<Vector2Int>(candidateSet);

        // Weighted random: closer to (0,0) = more likely.
        float totalWeight = 0f;
        float[] weights = new float[candidates.Count];

        for (int i = 0; i < candidates.Count; i++)
        {
            Vector2Int p = candidates[i];
            // squared distance to (0,0)
            int d2 = p.x * p.x + p.y * p.y;

            // Weight: 1 / (1 + d^2)
            float w = 1f / (1f + d2);
            weights[i] = w;
            totalWeight += w;
        }

        float r = Random.value * totalWeight;
        for (int i = 0; i < candidates.Count; i++)
        {
            r -= weights[i];
            if (r <= 0f)
                return candidates[i];
        }

        // Fallback (due to float precision)
        return candidates[candidates.Count - 1];
    }
}

using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class Appliance : MonoBehaviour
{
    [SerializeField, ReadOnly] private SelectableObject _selectableObject;

    [field: SerializeField] public ApplianceOrientation AllowedOrientations { get; private set; } = ApplianceOrientation.Any;
    [field: SerializeField] public Vector2Int Size { get; private set; } = Vector2Int.one;
    
    [field: Header("Minigame")]
    [field: SerializeField] public bool IsUsingMinigame { get; private set; } = true;
    [SerializeField, Required, ShowIf("IsUsingMinigame")] private GameObject _miniGame;
    [field: SerializeField, ReadOnly, ShowIf("IsUsingMinigame")] public bool IsMinigameFinished { get; private set; }

    private void Awake()
    {
        _selectableObject = GetComponentInChildren<SelectableObject>();
    }

    private void Start()
    {
        // Register only when this appliance participates in the minigame loop.
        if(IsUsingMinigame)
            MinigameManager.Instance.RegisterAppliance();
    }

    public void StartMinigame()
    {
        if (!IsUsingMinigame)
            return;
        
        if (IsMinigameFinished)
            return;
        
        if (_miniGame == null)
        {
            Debug.LogWarning($"Failed to start minigame for Appliance {gameObject.name}. Missing minigame prefab");
            return;
        }

        // Start the minigame only after the appliance has passed its basic checks.
        MinigameManager.Instance.StartMinigame(this, _miniGame);
    }

    [Button("Cheat: Finish Minigame", ButtonSizes.Large), ShowIf("IsUsingMinigame")]
    public void CheatFinishMinigame()
    {
        if (!IsUsingMinigame)
            return;
        
        // Keep the cheat path obvious so testing can skip the full loop.
        MinigameManager.Instance.StartMinigame(this, _miniGame);
        MinigameManager.Instance.FinishCurrentMinigame();
    }
    
    public void FinishMinigame()
    {
        IsMinigameFinished = true;
        // Disable interaction once the appliance is complete.
        _selectableObject.enabled = false;
        CustomUtils.SetMaterialRecursive(gameObject, MinigameManager.Instance.ApplianceFinishedMaterial);
        _selectableObject.RebindOriginalMaterials();
    }
    
#if UNITY_EDITOR
    // Draw the appliance footprint in the editor as green boxes on the XZ plane.
    private void OnDrawGizmos()
    {
        if (UnityEditor.EditorApplication.isPlaying)
            return;
        
        Vector3 basePos = transform.position;
        Vector3 scale = transform.localScale;
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                // Root tile is red
                if (x == 0 && y == 0)
                    Gizmos.color = Color.red;
                else
                    Gizmos.color = Color.green;
                
                Vector3 offset = new Vector3(x * scale.x, 0, y * scale.z);
                Vector3 center = basePos + offset;
                Vector3 boxSize = new Vector3(scale.x, scale.y, scale.z);
                Gizmos.DrawWireCube(center + (Vector3.up * scale.y / 2f), boxSize);
            }
        }
    }
#endif
}
using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class Appliance : MonoBehaviour
{
    [SerializeField, ReadOnly] private SelectableObject _selectableObject;
    [SerializeField, Required] private GameObject _miniGame;
    [field: SerializeField, Required] public bool IsMinigameFinished { get; private set; }

    [System.Flags]
    public enum Orientation
    {
        None  = 0,
        Right = 1 << 0,
        Left  = 1 << 1,
        Front   = 1 << 2,
        Back  = 1 << 3,
        Any   = Right | Left | Front | Back
    }
    
    [field: SerializeField] public Orientation AllowedOrientations { get; private set; } = Orientation.Any;
    [field: SerializeField] public Vector2Int Size { get; private set; } = Vector2Int.one;

    private void Awake()
    {
        _selectableObject = GetComponentInChildren<SelectableObject>();
    }

    private void Start()
    {
        MinigameManager.Instance.RegisterAppliance();
    }

    public void StartMinigame()
    {
        if (IsMinigameFinished)
            return;
        
        if (_miniGame == null)
        {
            Debug.LogWarning($"Failed to start minigame for Appliance {gameObject.name}. Missing minigame prefab");
            return;
        }
        MinigameManager.Instance.StartMinigame(this, _miniGame);
    }

    [Button("Cheat: Finish Minigame", ButtonSizes.Large)]
    public void CheatFinishMinigame()
    {
        MinigameManager.Instance.StartMinigame(this, _miniGame);
        MinigameManager.Instance.FinishCurrentMinigame();
    }
    
    public void FinishMinigame()
    {
        IsMinigameFinished = true;
        CustomUtils.SetMaterialRecursive(gameObject, MinigameManager.Instance.ApplianceFinishedMaterial);
        _selectableObject.RebindOriginalMaterials();
    }
}
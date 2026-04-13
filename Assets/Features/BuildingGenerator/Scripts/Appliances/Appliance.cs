using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class Appliance : MonoBehaviour
{
    [SerializeField, ReadOnly] private SelectableObject _selectableObject;

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
    
    [field: Header("Minigame")]
    [field: SerializeField] public bool IsUsingMinigame { get; private set; } = true;
    [SerializeField, Required, ShowIf("IsUsingMinigame")] private GameObject _miniGame;
    [field: SerializeField, Required, ShowIf("IsUsingMinigame")] public bool IsMinigameFinished { get; private set; }

    private void Awake()
    {
        _selectableObject = GetComponentInChildren<SelectableObject>();
    }

    private void Start()
    {
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
        MinigameManager.Instance.StartMinigame(this, _miniGame);
    }

    [Button("Cheat: Finish Minigame", ButtonSizes.Large), ShowIf("IsUsingMinigame")]
    public void CheatFinishMinigame()
    {
        if (!IsUsingMinigame)
            return;
        
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
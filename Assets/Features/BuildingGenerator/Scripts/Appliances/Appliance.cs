using Sirenix.OdinInspector;
using UnityEngine;

public class Appliance : MonoBehaviour
{
    [SerializeField, Required] private GameObject _miniGame;

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
    
    public void StartMinigame()
    {
        if (_miniGame == null)
        {
            Debug.LogWarning($"Failed to start minigame for Appliance {gameObject.name}. Missing minigame prefab");
            return;
        }
        Instantiate(_miniGame);
    }
}
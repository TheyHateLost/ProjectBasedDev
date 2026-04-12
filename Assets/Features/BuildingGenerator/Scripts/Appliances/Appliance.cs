using UnityEngine;

public class Appliance : MonoBehaviour
{
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
        
    }
}
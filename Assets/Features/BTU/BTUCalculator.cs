using UnityEngine;

public class BTUCalculator : MonoBehaviour
{
    public enum RoomType { Bathroom} 
    
    public float CalculateBTU(RoomType type, float w, float l, float h)
    {
        float volume = w * l * h;
        float multiplier = GetMultiplier(type);
        return volume * multiplier;
    }

    private float GetMultiplier(RoomType type)
    {
        switch (type)
        {
            case RoomType.Bathroom: return 148.5f;
            default: return 0f;
        }
    }
}
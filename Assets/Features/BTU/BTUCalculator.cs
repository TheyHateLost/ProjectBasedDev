using UnityEngine;  

public class BTUCalculator : MonoBehaviour
{
   public enum RoomType {Bathroom}
   
   [Header("Room Type")]
   public RoomType roomType;
   public float width; 
   public float length;
   public float height;

   [Header("BTU Result")]
   public float btuResult;
private void Start()
{
    btuResult = CalculateBTU();
}
private float GetMultiplier(RoomType type)
{
    switch (type)
    {
        case RoomType.Bathroom: return 148.5f;
        default: return 0;
    }

}

    public float CalculateBTU()
    {
        float volume = width * length * height;
        float multiplier = GetMultiplier(roomType);
        float btu = volume * multiplier;
        return btu;
    }

}

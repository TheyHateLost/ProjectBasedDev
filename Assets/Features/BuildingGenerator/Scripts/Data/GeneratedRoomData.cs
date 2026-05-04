using System.Collections.Generic;

[System.Serializable]
public class GeneratedRoomData
{
    public RoomType Type;
    public int Size;
    public int Area => Size * Size;
    public List<WindowPlacementData> WindowPlacements = new();
    public List<AppliancePlacementData> AppliancePlacements = new();
}

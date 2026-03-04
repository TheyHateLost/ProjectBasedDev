using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomData
{
    [field: SerializeField] public RoomType Type { get; private set; }
    [field: SerializeField] public IntRange TileAreaRange { get; private set; } = new IntRange(1, 2);
    [field: SerializeField] public List<RoomType> PossibleAdjacentRoomTypes { get; private set; } = new();
}
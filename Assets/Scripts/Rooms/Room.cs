using UnityEngine;

public abstract class Room : ScriptableObject
{
    public int roomId;
    
    public abstract bool IsRoomPossible(int[,] layout, LevelSize currentLocation, int enterDirection, int exitDirection);
}

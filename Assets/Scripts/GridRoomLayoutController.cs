using UnityEngine;

public abstract class GridRoomLayoutController : ScriptableObject
{
    public abstract int[,] Init(Room[] rooms, LevelSize levelSize, int defaultValue);
    public abstract int[,] GenerateRoomLayout(int[,] layout, LevelSize levelSize, LevelSize startingLocation);
}

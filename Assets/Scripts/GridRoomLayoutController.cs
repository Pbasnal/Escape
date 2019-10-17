using UnityEngine;
using System.Collections;

public abstract class GridRoomLayoutController : ScriptableObject
{
    public abstract void Init();
    public abstract int[,] GenerateRoomLayout(LevelSize levelSize);
    public abstract GameObject GetRoom(int roomId);
}

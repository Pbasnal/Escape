using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public abstract class Room : ScriptableObject
{
    public int roomId;
    public List<GameObject> roomPrefab;

    public abstract bool IsRoomPossible(int[,] layout, LevelSize currentLocation, int enterDirection, int exitDirection);
    public virtual GameObject GetARoom()
    {
        return roomPrefab[Random.Range(0, roomPrefab.Count)];
    }
}

using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "RoomLRBT", menuName = "Level Gen/Rooms/Room LRBT", order = 51)]
public class RoomLRBT : Room
{
    public override bool IsRoomPossible(int[,] layout, LevelSize currentLocation, int enterDirection, int exitDirection)
    {
        return true;
    }
}
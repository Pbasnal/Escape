using UnityEngine;

[CreateAssetMenu(fileName = "RoomLRB", menuName = "Level Gen/Rooms/Room LRB", order = 51)]
public class RoomLRB : Room
{
    public override bool IsRoomPossible(int[,] layout, LevelSize currentLocation, int enterDirection, int exitDirection)
    {
        return exitDirection != 3 ? true : false;
    }
}

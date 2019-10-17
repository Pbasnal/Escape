using UnityEngine;

[CreateAssetMenu(fileName = "RoomLRT", menuName = "Level Gen/Rooms/Room LRT", order = 51)]
public class RoomLRT : Room
{
    public override bool IsRoomPossible(int[,] layout, LevelSize currentLocation, int enterDirection, int exitDirection)
    {
        return enterDirection != 3 ? true : false;
    }
}

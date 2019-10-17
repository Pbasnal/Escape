using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "RoomLR", menuName = "Level Gen/Rooms/Room LR", order = 51)]
public class RoomLR : Room
{
    public override bool IsRoomPossible(int[,] layout, LevelSize currentLocation, int enterDirection, int exitDirection)
    {
        if (enterDirection == 3 || exitDirection == 3)
        {
            return false;
        }

        return true;
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "LRTB Room", menuName = "Base Room Types/LRTB Room Type", order = 51)]
public class LeftRightTopBottomRoomType : ARoomType
{
    public override bool IsRoomPossible(int enterDirection, int exitDirection)
    {
        return true;
    }
}
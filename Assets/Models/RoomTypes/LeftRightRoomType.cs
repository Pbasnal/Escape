using UnityEngine;

[CreateAssetMenu(fileName = "LR Room", menuName = "Base Room Types/LR Room Type", order = 51)]
public class LeftRightRoomType : ARoomType
{
    public override bool IsRoomPossible(int enterDirection, int exitDirection)
    {
        return true;
    }
}

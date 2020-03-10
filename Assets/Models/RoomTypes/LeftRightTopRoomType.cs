using UnityEngine;

[CreateAssetMenu(fileName = "LRT Room", menuName = "Base Room Types/LRT Room Type", order = 51)]
public class LeftRightTopRoomType : ARoomType
{
    public override bool IsRoomPossible(int enterDirection, int exitDirection)
    {
        return true;
    }
}

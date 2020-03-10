using UnityEngine;

public abstract class ARoomType: ScriptableObject
{
    public abstract bool IsRoomPossible(int enterDirection, int exitDirection);
}
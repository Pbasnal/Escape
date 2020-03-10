using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Grid))]
[RequireComponent(typeof(TilemapRenderer))]
public class RoomBuilder : MonoBehaviour
{
    public SizeObject roomSize;
    public ARoomType roomType;
}
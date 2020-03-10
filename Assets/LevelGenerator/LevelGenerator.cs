using Assets.Scripts.LayoutGenerator;
using Assets.Scripts.LevelRenderer;
using UnityEngine;

[RequireComponent(typeof(RoomCollection))]
[RequireComponent(typeof(LevelRenderer))]
public class LevelGenerator : MonoBehaviour
{
    public SizeObject levelSize;
    private RoomCollection roomCollection;
    private RoomBuilder _startingRoom;
    private Bounds bounds;
    
    private ILayoutCreator layoutCreator;
    private LevelRenderer levelRenderer;

    private void Start()
    {
        if (levelSize == null)
        {
            return;
        }

        var startingPoint = new LevelCoordinate
        {
            Height = 0,
            Width = Random.Range(0, levelSize.Width)
        };

        levelRenderer = GetComponent<LevelRenderer>();
        roomCollection = GetComponent<RoomCollection>();

        layoutCreator = new FourByFourLayout(roomCollection.GetRoomTypes(), startingPoint);
        
        bounds = levelRenderer.RenderBaseLevel(layoutCreator);
    }
}

public class Bounds
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
}

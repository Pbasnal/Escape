using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator2 : MonoBehaviour
{
    public LevelSize levelSize;
    public RoomCollection roomCollection;

    private Vector3 _cellSize;
    private int[,] _levelLayout;



    // Start is called before the first frame update
    void Start()
    {
        var grid = GetComponent<Grid>();
        _cellSize = grid.cellSize;
        _levelLayout = new int[levelSize.heightInGrids, levelSize.widthInGrids];
        roomCollection.InitRooms();
        var room = roomCollection.GetARandomRoomWithConstraints(new RoomType2[] { /*how do you pass types?*/ });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator GenerateMainPath()
    {
        int startingPointOnGround = Random.Range(0, levelSize.widthInGrids);

        int i = levelSize.heightInGrids;
        int j = startingPointOnGround;

        /* 1 - Left
         * 2 - Right
         * 3 - Up
         */
        int[] directions = new int[] { 1, 1, 2, 2, 3};

        int prevDirection = 0;
        int direction = Random.Range(0, directions.Length);

        int roomType = 0;

        //switch (direction)
        //{
        //    case 1: 

        //}

        yield return null;
    }

    private RoomType GetStartingRoom(int exitDirection)
    {
        switch (exitDirection)
        {
            case 1:
            case 2: return RoomType.RoomLR;
            case 3: return RoomType.RoomLRBT;
        }

        return RoomType.RoomLRBT;
    }
}

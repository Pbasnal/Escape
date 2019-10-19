using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpelunkyRoomLayoutGenerator", menuName = "Level Gen/Layout/SpelunkyLike", order = 51)]
public class SpelunkyRoomLayoutGenerator : GridRoomLayoutController
{
    public Room[] rooms;
    private List<Room> possibleRooms = new List<Room>();
    
    public override int[,] Init(Room[] rooms, LevelSize levelSize, int defaultValue)
    {
        this.rooms = rooms;
        
        var layout = new int[levelSize.heightInGrids, levelSize.widthInGrids];

        for (int i = 0; i < levelSize.heightInGrids; i++)
        {
            for (int j = 0; j < levelSize.widthInGrids; j++)
            {
                layout[i, j] = defaultValue;
            }
        }
        return layout;
    }

    private int[,][] GetDirectionsMap()
    {
        /* 1 - Left
         * 2 - Right
         * 3 - Up
         */
        var directions = new int[4, 4][];

        directions[0, 0] = directions[3, 0] = new int[] { 2, 2, 2, 3 };
        directions[0, 1] = directions[3, 1] = directions[0, 2] = directions[3, 2] = new int[] { 1, 1, 1, 2, 2, 2, 3 };
        directions[0, 3] = directions[3, 3] = new int[] { 1, 1, 1, 3 };

        directions[1, 0] = new int[] { 3 };
        directions[1, 1] = directions[1, 2] = directions[0, 3];
        directions[1, 3] = new int[] { }; // this is not a possible situation

        directions[2, 0] = directions[1, 3]; // this is not a possible situation
        directions[2, 1] = directions[2, 2] = directions[0, 0];
        directions[2, 3] = directions[1, 0];

        return directions;
    }

    private List<Room> GetPossibleRooms(int[,] layout, LevelSize currentLocation, int enterDirection, int exitDirection)
    {
        possibleRooms.Clear();
        foreach (var room in rooms)
        {
            var isRoomPossible = room.IsRoomPossible(layout, currentLocation, enterDirection, exitDirection);
            if (isRoomPossible)
            {
                possibleRooms.Add(room);
            }
        }

        return possibleRooms;
    }

    public override int[,] GenerateRoomLayout(int[,] layout, LevelSize levelSize, LevelSize startingLocation)
    {
        int enterDirection = 0;
        var currentLocation = new LevelSize
        {
            heightInGrids = startingLocation.heightInGrids,
            widthInGrids = startingLocation.widthInGrids
        };

        var directions = GetDirectionsMap();
        var generationStartTime = DateTime.UtcNow;
        var retries = 0;
        while (currentLocation.heightInGrids >= 0 && retries < 5)
        {
            int selectedRoom = 0;
            int i = enterDirection;
            int j = currentLocation.widthInGrids;
            int exitDirection = 0;
            try
            {
                exitDirection = directions[i, j][UnityEngine.Random.Range(0, directions[i, j].Length)];
                possibleRooms = GetPossibleRooms(layout, currentLocation, enterDirection, exitDirection);

                if (possibleRooms.Count == 0)
                {
                    retries++;
                    Debug.LogWarning(String.Format("i: {0}  j: {1}  In: {2}  Out: {3}", i, j, enterDirection, exitDirection));
                    continue;
                }
                retries = 0;

                selectedRoom = possibleRooms[UnityEngine.Random.Range(0, possibleRooms.Count)].roomId;
                layout[currentLocation.heightInGrids, currentLocation.widthInGrids] = selectedRoom;
            }
            catch (Exception ex)
            {
            }
            Debug.Log(String.Format("i: {0}  j: {1}  SelectedRoom: {2}  In: {3}  Out: {4}", i, j, selectedRoom, enterDirection, exitDirection));

            switch (exitDirection)
            {
                case 1: currentLocation.widthInGrids--; break;
                case 2: currentLocation.widthInGrids++; break;
                case 3: currentLocation.heightInGrids--; break;
            }
            enterDirection = exitDirection;
        }

        Debug.Log("Time to generate the level: " + (DateTime.UtcNow - generationStartTime).TotalMilliseconds);
        return layout;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpelunkyRoomLayoutGenerator", menuName = "Level Gen/Layout/SpelunkyLike", order = 51)]
public class SpelunkyRoomLayoutGenerator : GridRoomLayoutController
{
    public List<Room> rooms;
    private IDictionary<int, Room> _roomsHash;

    public override int[,] GenerateRoomLayout(LevelSize levelSize)
    {
        _roomsHash = new Dictionary<int, Room>();
        for (int i = 0; i < rooms.Count; i++)
        {
            rooms[i].roomId = i;
            _roomsHash.Add(rooms[i].roomId, rooms[i]);
        }

        var layout = new int[levelSize.heightInGrids, levelSize.widthInGrids];

        for (int i = 0; i < levelSize.heightInGrids; i++)
        {
            for (int j = 0; j < levelSize.widthInGrids; j++)
            {
                layout[i, j] = -1;
            }
        }

        int enterDirection = 0;
        var currentLocation = new LevelSize
        {
            heightInGrids = levelSize.heightInGrids - 1,
            widthInGrids = UnityEngine.Random.Range(0, levelSize.widthInGrids)
        };

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

        var possibleRooms = new List<Room>();

        var generationStartTime = DateTime.UtcNow;
        var retries = 0;
        while (currentLocation.heightInGrids >= 0 && retries < 5)
        {
            int selectedRoom;
            try
            {
                var i = enterDirection;
                var j = currentLocation.widthInGrids;
                int exitDirection = directions[i, j][UnityEngine.Random.Range(0, directions[i, j].Length)];

                foreach (var room in rooms)
                {
                    var isRoomPossible = room.IsRoomPossible(layout, currentLocation, enterDirection, exitDirection);
                    if (isRoomPossible)
                    {
                        possibleRooms.Add(room);
                    }
                }

                if (possibleRooms.Count == 0)
                {
                    retries++;
                    Debug.LogWarning(String.Format("i: {0}  j: {1}  In: {2}  Out: {3}", i, j, enterDirection, exitDirection));
                    continue;
                }
                retries = 0;

                selectedRoom = UnityEngine.Random.Range(0, possibleRooms.Count);
                selectedRoom = possibleRooms[selectedRoom].roomId;
                layout[currentLocation.heightInGrids, currentLocation.widthInGrids] = selectedRoom;

                Debug.Log(String.Format("i: {0}  j: {1}  SelectedRoom: {2}  In: {3}  Out: {4}", i, j, selectedRoom, enterDirection, exitDirection));

                switch (exitDirection)
                {
                    case 1: currentLocation.widthInGrids--; break;
                    case 2: currentLocation.widthInGrids++; break;
                    case 3: currentLocation.heightInGrids--; break;
                }
                possibleRooms.Clear();
                enterDirection = exitDirection;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }

        Debug.Log("Time to generate the level: " + (DateTime.UtcNow - generationStartTime).TotalMilliseconds);
        return layout;
    }

    public override GameObject GetRoom(int roomId)
    {
        if (roomId == -1)
        {
            return rooms[UnityEngine.Random.Range(0, rooms.Count)].GetARoom();
        }
        return _roomsHash[roomId].GetARoom();
    }
}

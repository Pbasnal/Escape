using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum RoomType
{
    RoomLR,
    RoomLRB,
    RoomLRT,
    RoomLRBT
}

[Serializable]
public class Room
{
    public RoomType roomType;
    public GameObject room;
}

public class LevelGenerator : MonoBehaviour
{
    public int roomDistance;
    public int roomSize;
    public GameObject defatultRoom;
    public Transform[] startingPositions;
    public Room[] rooms;
    public Transform[] borders;

    private List<GameObject> mainPathLocations = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        int randomStartingPosition = UnityEngine.Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randomStartingPosition].position;
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        GenerateMainPath();

        var startx = ((int)borders[2].position.x / roomSize) * roomSize + (roomDistance / 2);
        var starty = ((int)borders[0].position.y / roomSize) * roomSize - (roomDistance / 2);

        for (float x = startx; x < borders[3].position.x; x += roomDistance)
        {
            for (float y = starty; y > borders[1].position.y; y -= roomDistance)
            {
                var createRoom = true;
                foreach (var mainPathPosition in mainPathLocations)
                {
                    if (mainPathPosition.transform.position.x == x && mainPathPosition.transform.position.y == y)
                    {
                        Debug.Log("MainPath at " + x + "    " + y);
                        createRoom = false;
                        break;
                    }
                }
                if (!createRoom)
                {
                    continue;
                }

                int roomIndex = UnityEngine.Random.Range(-1, rooms.Length);
                GameObject room;
                if (roomIndex == -1)
                {
                    room = defatultRoom;
                }
                else
                {
                    room = rooms[roomIndex].room;
                }
                Instantiate(room, new Vector3(x, y, 0), Quaternion.identity);
            }
        }
    }

    private void GenerateMainPath()
    {
        var inDirection = 0;
        var roomIndex = 0;
        var roomIndexOfLRBT = 0;
        var currentRoomPosition = transform.position;
        var exitDirection = GenerateNextRoom(inDirection);

        int[] roomsForB = new int[2];
        int[] roomsForT = new int[2];

        int bIndex = 0, tIndex = 0;
        for (int i = 0; i < rooms.Length; i++)
        {
            switch (rooms[i].roomType)
            {
                case RoomType.RoomLRBT:
                    roomsForB[bIndex++] = i;
                    roomsForT[tIndex++] = i;
                    roomIndexOfLRBT = i;
                    break;
                case RoomType.RoomLRB: roomsForB[bIndex++] = i; break;
                case RoomType.RoomLRT: roomsForT[tIndex++] = i; break;
            }
        }

        switch (exitDirection)
        {
            case 1:
            case 2: roomIndex = UnityEngine.Random.Range(0, rooms.Length); break;
            case 3: roomIndex = roomsForB[UnityEngine.Random.Range(0, roomsForB.Length)]; break;
        }
        var newRoom = Instantiate(rooms[roomIndex].room, currentRoomPosition, Quaternion.identity);
        mainPathLocations.Add(newRoom);

        inDirection = exitDirection;
        while (transform.position.y > borders[1].position.y)
        {
            currentRoomPosition = transform.position;
            exitDirection = GenerateNextRoom(inDirection);

            if (transform.position.y < borders[1].position.y - roomDistance) break;

            if (exitDirection == -1) continue;

            switch (inDirection * exitDirection)
            {
                case 1:
                case 4: roomIndex = UnityEngine.Random.Range(0, rooms.Length); break;
                case 9: roomIndex = roomIndexOfLRBT; break;
                case 3:
                case 6:
                    if (inDirection < 3)
                    {
                        roomIndex = roomsForB[UnityEngine.Random.Range(0, roomsForB.Length)];
                    }
                    else
                    {
                        roomIndex = roomsForT[UnityEngine.Random.Range(0, roomsForT.Length)];
                    }
                    break;
            }

            newRoom = Instantiate(rooms[roomIndex].room, currentRoomPosition, Quaternion.identity);
            mainPathLocations.Add(newRoom);
            inDirection = exitDirection;
        }
    }

    private int GenerateNextRoom(int inDirection)
    {
        int newRoomDirection = UnityEngine.Random.Range(1, 6);
        float x = transform.position.x;
        float y = transform.position.y;

        if (newRoomDirection == 1 || newRoomDirection == 2)
        {
            if (inDirection == 2) return -1;
            x = transform.position.x + roomDistance;
            y = transform.position.y;
            newRoomDirection = 1;
        }
        else if (newRoomDirection == 3 || newRoomDirection == 4)
        {
            if (inDirection == 1) return -1;
            x = transform.position.x - roomDistance;
            y = transform.position.y;
            newRoomDirection = 2;
        }

        if (x < borders[2].position.x ||
            x > borders[3].position.x || newRoomDirection == 5)
        {
            newRoomDirection = 3;
            x = transform.position.x;
            y = transform.position.y - roomDistance;
        }

        transform.position = new Vector3(x, y, 0);
        return newRoomDirection;
    }
}

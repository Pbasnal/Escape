using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class RoomCollection
{
    public Room2[] rooms;
    public RoomType2[] supportedRoomTypes;

    private IDictionary<RoomType2, List<int>> roomTypeMapping;

    public bool InitRooms()
    {
        if (rooms == null || rooms.Length == 0)
        {
            return false;
        }

        roomTypeMapping = new Dictionary<RoomType2, List<int>>();

        foreach (var roomType in supportedRoomTypes)
        {
            roomTypeMapping.Add(roomType, new List<int>());
        }

        // something is wrong. A room should have list of supported types in it. One type should not define the room
        for (int i = 0; i < rooms.Length; i++)
        {
            foreach (var roomType in rooms[i].roomTypes)
            {
                if (roomTypeMapping.ContainsKey(roomType))
                {
                    roomTypeMapping[roomType].Add(i);
                }
            }
        }

        return true;
    }

    public Room2 GetARandomRoomWithConstraints(RoomType2[] roomTypes)
    {
        if (roomTypes == null || roomTypes.Length == 0)
        {
            return null;
        }

        var filteredRooms = new HashSet<int>(roomTypeMapping[roomTypes[0]]);

        for (int i = 1; i < roomTypes.Length; i++)
        {
            filteredRooms.IntersectWith(roomTypeMapping[roomTypes[i]]);
        }

        if (filteredRooms.Count == 0)
        {
            return null;
        }

        var chosenRoom = UnityEngine.Random.Range(0, filteredRooms.Count);

        return rooms[filteredRooms.ToArray()[chosenRoom]];
    }
}

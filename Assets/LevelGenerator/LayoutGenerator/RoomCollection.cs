using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.LayoutGenerator
{
    public class RoomCollection : MonoBehaviour
    {
        public RoomBuilder[] rooms;

        public SizeObject RoomSize => rooms[0].roomSize;

        private IDictionary<ARoomType, List<RoomBuilder>> roomTypeMap;

        private void Awake()
        {
            roomTypeMap = new Dictionary<ARoomType, List<RoomBuilder>>();

            foreach (var room in rooms)
            {
                if (!roomTypeMap.ContainsKey(room.roomType))
                {
                    roomTypeMap.Add(room.roomType, new List<RoomBuilder>());
                }
                roomTypeMap[room.roomType].Add(room);
            }
        }

        public List<ARoomType> GetRoomTypes()
        {
            return roomTypeMap.Keys.ToList();
        }

        public RoomBuilder GetARoom(ARoomType roomType)
        {
            if (!roomTypeMap.ContainsKey(roomType))
            {
                return null;
            }

            var selectedRooms = roomTypeMap[roomType];
            return selectedRooms[UnityEngine.Random.Range(0, selectedRooms.Count)];
        }
    }
}

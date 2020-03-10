using Assets.Scripts.LayoutGenerator;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.LevelRenderer
{
    [RequireComponent(typeof(RoomCollection))]
    public class LevelRenderer : MonoBehaviour
    {
        public GameObject mainPathMarker;
        public GameObject startingRoomMarker;
        public GameObject wall;

        private RoomCollection roomCollection;
        private ILayoutCreator layoutGenerator;

        private void Start()
        {
            roomCollection = GetComponent<RoomCollection>();
        }
        
        public Bounds RenderBaseLevel(ILayoutCreator layoutCreator)
        {
            layoutGenerator = layoutCreator;
            
            var roomTypeLayout = layoutGenerator.GenerateRoomLayout();

            var x = transform.position.x;
            var y = transform.position.y;

            var levelSize = roomTypeLayout.LevelSize;

            for (int i = 0; i < levelSize.Height; i++)
            {
                for (int j = 0; j < levelSize.Width; j++)
                {
                    GameObject marker = null;
                    var room = RenderRoom(roomTypeLayout.TypeLayout[i, j], i, j);

                    if (roomTypeLayout.StartingPostion.Height == i && roomTypeLayout.StartingPostion.Width == j)
                    {
                        marker = Instantiate(startingRoomMarker, room.transform.position, Quaternion.identity);
                        marker.transform.localScale = new Vector3(room.roomSize.Width, room.roomSize.Height, 1);
                        marker.transform.parent = room.transform;
                    }
                    else if(roomTypeLayout.MainPath.Any(p => p.Height == i && p.Width == j))
                    {
                        marker = Instantiate(mainPathMarker, room.transform.position, Quaternion.identity);
                        marker.transform.localScale = new Vector3(room.roomSize.Width, room.roomSize.Height, 1);
                        marker.transform.parent = room.transform;
                    }                    
                }
            }

            return GenerateWalls(roomTypeLayout);
        }

        private RoomBuilder RenderRoom(ARoomType roomType, int i, int j)
        {
            var room = roomCollection.GetARoom(roomType);
            var roomSizeX = room.roomSize.Width;
            var roomSizeY = room.roomSize.Height;

            Vector2 spawnPosition = new Vector2(
                transform.position.x + (j * roomSizeX),
                transform.position.y - (i * roomSizeY));

            room = Instantiate(room, spawnPosition, Quaternion.identity);
            room.transform.parent = transform;

            return room;
        }

        private Bounds GenerateWalls(LevelLayout layout)
        {
            var scale = layout.LevelSize.Height * roomCollection.RoomSize.Height;

            var minX = transform.position.x - (roomCollection.RoomSize.Width / 2) + 0.5f;
            var midX = (transform.position.x + scale) / 2 - (roomCollection.RoomSize.Width / 2);
            var maxX = transform.position.x + scale - (roomCollection.RoomSize.Width / 2) - 0.5f;

            var minY = transform.position.y - (roomCollection.RoomSize.Height / 2) + 0.5f;
            var midY = (transform.position.y + scale) / 2 - (roomCollection.RoomSize.Height / 2);
            var maxY = transform.position.y + scale - (roomCollection.RoomSize.Height / 2) - 0.5f;

            var ground = Instantiate(wall, transform);
            var roof = Instantiate(wall, transform);
            var leftWall = Instantiate(wall, transform);
            var rightWall = Instantiate(wall, transform);

            ground.transform.position = new Vector3(midX, -minY, 0);
            ground.transform.localScale = new Vector3(scale, 1, 1);

            roof.transform.position = new Vector3(midX, -maxY, 0);
            roof.transform.localScale = new Vector3(scale, 1, 1);

            leftWall.transform.position = new Vector3(minX, -midY, 0);
            leftWall.transform.localScale = new Vector3(1, scale, 1);

            rightWall.transform.position = new Vector3(maxX, -midY, 0);
            rightWall.transform.localScale = new Vector3(1, scale, 1);

            return new Bounds
            {
                minX = minX,
                maxX = maxX,
                minY = minY,
                maxY = maxY
            };
        }
    }
}

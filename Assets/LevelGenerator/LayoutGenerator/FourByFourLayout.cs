using System.Collections.Generic;

namespace Assets.Scripts.LayoutGenerator
{
    public class FourByFourLayout : ILayoutCreator
    {
        public Size LevelSize { get; private set; }
        public LevelCoordinate StartingPoint { get; private set; }
        private IList<ARoomType> roomTypes;
        private int[,][] directionsMap;

        public FourByFourLayout(IList<ARoomType> roomTypes, LevelCoordinate startingPoint)
        {
            this.roomTypes = roomTypes;
            LevelSize = new Size { Height = 4, Width = 4 };
            directionsMap = GetDirectionsMap();
            StartingPoint = startingPoint;
        }

        public LevelLayout GenerateRoomLayout()
        {
            LevelLayout levelLayout = new LevelLayout(LevelSize, StartingPoint);

            int enterDirection = 0;
            var selectedPos = new LevelCoordinate
            {
                Height = StartingPoint.Height,
                Width = StartingPoint.Width
            };

            while (selectedPos.Height < LevelSize.Height)
            {
                var roomTypeSelection = SelectABaseRoomType(enterDirection, selectedPos.Width);
                levelLayout.TypeLayout[selectedPos.Height, selectedPos.Width] = roomTypeSelection.SelectedBaseType;
                levelLayout.MainPath.Add(selectedPos.Clone());
                enterDirection = roomTypeSelection.ExitDirection;

                switch (roomTypeSelection.ExitDirection)
                {
                    case 1: selectedPos.Width--; break;
                    case 2: selectedPos.Width++; break;
                    case 3: selectedPos.Height++; break;
                }
            }

            for (int i = 0; i < LevelSize.Height; i++)
            {
                for (int j = 0; j < LevelSize.Width; j++)
                {
                    if (levelLayout.TypeLayout[i, j] != null)
                    {
                        continue;
                    }

                    levelLayout.TypeLayout[i, j] = roomTypes[UnityEngine.Random.Range(0, roomTypes.Count)];
                    continue;
                }
            }

            return levelLayout;
        }

        private int[,][] GetDirectionsMap()
        {
            /* 1 - Left
             * 2 - Right
             * 3 - Up
             */
            var directions = new int[4, 4][];

            directions[0, 0] = directions[3, 0] = new int[] { 2, 2, 3 };
            directions[0, 1] = directions[3, 1] = directions[0, 2] = directions[3, 2] = new int[] { 1, 1, 2, 2, 3 };
            directions[0, 3] = directions[3, 3] = new int[] { 1, 1, 3 };

            directions[1, 0] = new int[] { 3 };
            directions[1, 1] = directions[1, 2] = directions[0, 3];
            directions[1, 3] = new int[] { }; // this is not a possible situation

            directions[2, 0] = directions[1, 3]; // this is not a possible situation
            directions[2, 1] = directions[2, 2] = directions[0, 0];
            directions[2, 3] = directions[1, 0];

            return directions;
        }

        private SelectedRoomTypeResponse SelectABaseRoomType(int enterDirection, int width)
        {
            var selectedRoomIndex = UnityEngine.Random.Range(0, directionsMap[enterDirection, width].Length);
            var exitDirection = directionsMap[enterDirection, width][selectedRoomIndex];
            var possibleRooms = new List<ARoomType>();
            foreach (var room in roomTypes)
            {
                var isRoomPossible = room.IsRoomPossible(enterDirection, exitDirection);
                if (!isRoomPossible)
                {
                    continue;
                }

                possibleRooms.Add(room);
            }

            return new SelectedRoomTypeResponse
            {
                SelectedBaseType = possibleRooms[UnityEngine.Random.Range(0, possibleRooms.Count)],
                ExitDirection = exitDirection
            };
        }
    }

    public class SelectedRoomTypeResponse
    {
        public ARoomType SelectedBaseType;
        public int ExitDirection;
    }

    public class LevelLayout
    {
        public Size LevelSize;
        public LevelCoordinate StartingPostion;
        public ARoomType[,] TypeLayout;
        public List<LevelCoordinate> MainPath;

        public LevelLayout(Size levelSize, LevelCoordinate startingPostion)
        {
            LevelSize = levelSize;
            TypeLayout = new ARoomType[levelSize.Height, levelSize.Width];
            MainPath = new List<LevelCoordinate>();
            StartingPostion = startingPostion;
        }
    }
}



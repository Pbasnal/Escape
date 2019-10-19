using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoomPrefabs
{
    public Room room;
    public GameObject[] roomPrefabs;
}

public class GridLevelGenerator : MonoBehaviour
{
    public LevelSize levelSize;
    public RoomPrefabs[] roomPrefabs;
    public int defaultRoomId;
    public GameObject player;
    public GameObject wall;

    public Cinemachine.CinemachineVirtualCamera camera;

    public GridRoomLayoutController gridRoomLayoutController;
    public GameObject mainPathMarker;

    private Vector3 _cellSize;
    private int[,] _levelLayout;
    private LevelSize _startingLocation;
    private GameObject _startingRoom;
    private IDictionary<int, GameObject[]> roomIdPrefabsMap;
    private Room[] allRooms;

    private void Awake()
    {
        roomIdPrefabsMap = new Dictionary<int, GameObject[]>();
        allRooms = new Room[roomPrefabs.Length - 1];

        int j = 0;
        for (int i = 0; i < roomPrefabs.Length; i++)
        {
            roomIdPrefabsMap.Add(roomPrefabs[i].room.roomId, roomPrefabs[i].roomPrefabs);

            if (roomPrefabs[i].room.roomId == defaultRoomId)
            {
                continue;
            }
            allRooms[j++] = roomPrefabs[i].room;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("Started");

        _levelLayout = gridRoomLayoutController.Init(allRooms, levelSize, defaultRoomId);

        GenerateLevel();
        SpawnPlayer();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Started");
            ClearLevel();
            Debug.Log("Cleared");
            GenerateLevel();
        }
    }

    private void ClearLevel()
    {
        _levelLayout = gridRoomLayoutController.Init(allRooms, levelSize, defaultRoomId);
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void GenerateLevel()
    {
        if (gridRoomLayoutController == null)
        {
            return;
        }

        _startingLocation = new LevelSize { heightInGrids = levelSize.heightInGrids - 1, widthInGrids = UnityEngine.Random.Range(0, levelSize.widthInGrids) };
        _levelLayout = gridRoomLayoutController.GenerateRoomLayout(_levelLayout, levelSize, _startingLocation);

        var grid = GetComponent<Grid>();
        _cellSize = grid.cellSize;

        Vector3 pos = new Vector3();

        var x = -_cellSize.x * (levelSize.heightInGrids / 2);
        var y = _cellSize.y * (levelSize.widthInGrids / 2);
        for (int i = 0; i < levelSize.heightInGrids; i++)
        {
            for (int j = 0; j < levelSize.widthInGrids; j++)
            {
                pos.Set(x + (j * _cellSize.x), y - (i * _cellSize.y), 0);
                var room = Instantiate(GetARoomPrefab(_levelLayout[i, j]), pos, Quaternion.identity);
                room.transform.parent = transform;

                if (_startingLocation.heightInGrids == i && _startingLocation.widthInGrids == j)
                {
                    _startingRoom = room;
                }

                if (_levelLayout[i, j] == 0)
                {
                    continue;
                }

                var marker = Instantiate(mainPathMarker, pos, Quaternion.identity);
                marker.transform.parent = room.transform;
            }
        }
        GenerateWalls();
    }

    private void GenerateWalls()
    {
        var scale = levelSize.heightInGrids * _cellSize.x;

        var ground = Instantiate(wall, transform);
        var roof = Instantiate(wall, transform);
        var leftWall = Instantiate(wall, transform);
        var rightWall = Instantiate(wall, transform);

        ground.transform.position = new Vector3(transform.position.x - 5, transform.position.y - 14.5f, 0);
        ground.transform.localScale = new Vector3(scale, 1, 1);

        roof.transform.position = new Vector3(transform.position.x - 5, transform.position.y + 25f, 0);
        roof.transform.localScale = new Vector3(scale, 1, 1);

        leftWall.transform.position = new Vector3(transform.position.x - 24.5f, transform.position.y + 5, 0);
        leftWall.transform.localScale = new Vector3(1, scale, 1);

        rightWall.transform.position = new Vector3(transform.position.x + 14.5f, transform.position.y + 5, 0);
        rightWall.transform.localScale = new Vector3(1, scale, 1);
    }

    private void SpawnPlayer()
    {
        List<Transform> playerSpawnPoints = new List<Transform>();
        foreach (Transform child in _startingRoom.transform)
        {
            if (!child.CompareTag("Player Spawn Points"))
            {
                continue;
            }
            foreach (Transform point in child)
            {
                playerSpawnPoints.Add(point);
            }
        }

        var spawnPoint = playerSpawnPoints[UnityEngine.Random.Range(0, playerSpawnPoints.Count)]; ;

        var spawnedPlayer = Instantiate(player, spawnPoint.position, Quaternion.identity);
        spawnedPlayer.transform.parent = transform;

        camera.Follow = spawnedPlayer.transform;
    }

    private GameObject GetARoomPrefab(int roomId)
    {
        var prefabs = roomIdPrefabsMap[roomId];
        return prefabs[UnityEngine.Random.Range(0, prefabs.Length)];
    }
}

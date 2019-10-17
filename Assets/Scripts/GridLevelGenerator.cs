using UnityEngine;

public class GridLevelGenerator : MonoBehaviour
{
    public LevelSize levelSize;

    public GameObject levelRoot;

    public GridRoomLayoutController gridRoomLayoutController;
    public GameObject mainPathMarker;

    private Vector3 _cellSize;
    private int[,] _levelLayout;

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("Started");
        gridRoomLayoutController.Init();
        GenerateLevel();        
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

    void ClearLevel()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    void GenerateLevel()
    {
        if (gridRoomLayoutController == null)
        {
            return;
        }

        _levelLayout = gridRoomLayoutController.GenerateRoomLayout(levelSize);

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
                var room = GameObject.Instantiate(gridRoomLayoutController.GetRoom(_levelLayout[i, j]), pos, Quaternion.identity);
                room.transform.parent = transform;
                if (_levelLayout[i, j] != -1)
                {
                    var marker = GameObject.Instantiate(mainPathMarker, pos, Quaternion.identity);
                    marker.transform.parent = room.transform;
                }
            }
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomGenerator : MonoBehaviour
{
    public Transform tileSpawnPoints;
    public Transform playerSpawnPoints;
    public Room roomGenerator;

    private List<Transform> playerSpawnLocations;


    private void Awake()
    {
        List<Transform> playerSpawnLocations = new List<Transform>();
        foreach (Transform point in playerSpawnPoints)
        {
            playerSpawnLocations.Add(point);
        }

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

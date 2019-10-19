using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbabilitySpawn : MonoBehaviour
{
    [Range(0, 1)] public float probabilityToSpawn;
    public GameObject prefabToSpawn;

    private GameObject spawnedObject;
    private int[] numberRange = new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

    // Start is called before the first frame update
    private void Start()
    {
        if (Random.Range(0, 11) <= probabilityToSpawn * 10)
        {
            spawnedObject = Instantiate(prefabToSpawn, transform);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Destroy(spawnedObject);
            var probability = Random.Range(0, 11);

            Debug.Log("Prob: " + probability + "   prob to spawn: " + probabilityToSpawn * 10);
            if (probability <= probabilityToSpawn * 10)
            {
                Debug.Log("Spawned");
                spawnedObject = Instantiate(prefabToSpawn, transform);
            }
        }
    }
}

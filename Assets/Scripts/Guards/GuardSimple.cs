using UnityEngine;

public class GuardSimple : MonoBehaviour
{
    public Transform pathHolder;
    
    public float moveSpeed = 1f;
    public float turnSpeed = 1.0f;

    public int currentWaypoint = 0;
    public Vector3[] waypoints;

    void OnDrawGizmos()
    {
        var startPosition = pathHolder.GetChild(0).position;
        var previousPosition = startPosition;
        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, 0.3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);
    }

    void Awake()
    {
        waypoints = new Vector3[pathHolder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
        }
    }

    // Start is called before the first frame update
    void Start()
    {   
    }

    // Update is called once per frame
    void Update()
    {
        DrawOrientationLines(gameObject);
    }

    public void PlayerFound(Transform player)
    {
        Debug.Log("PlayerFound");
    }

    private void DrawOrientationLines(GameObject gameObject)
    {
        Debug.DrawRay(gameObject.transform.position, gameObject.transform.up * 5, Color.green);
        Debug.DrawRay(gameObject.transform.position, gameObject.transform.right * 5, Color.red);
    }

    
}

using UnityEngine;

public class GuardSimple : MonoBehaviour
{
    public Transform pathHolder;
    
    public float moveSpeed = 1f;
    public float turnSpeed = 1.0f;
    public int currentWaypoint = 0;
    public int alertDurationInSecs = 5;
    public Vector3[] waypoints;

    [HideInInspector]
    public Transform target;
    public FieldOfView fieldOfView;

    private Animator animator;

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

        fieldOfView = gameObject.GetComponent<FieldOfView>();
        animator = gameObject.GetComponent<Animator>();
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
        target = player;
        animator.SetBool("TargetInView", true);
        animator.SetBool("TargetLost", false);
    }

    public void PlayerLost()
    {
        animator.SetBool("TargetInView", false);
    }

    public void TurnToTarget(Vector3 lookTarget)
    {
        var dir = (lookTarget - transform.position).normalized;
        var targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

        var deltaAngle = Mathf.DeltaAngle(transform.eulerAngles.z, targetAngle);
        if (deltaAngle > 0.05f || deltaAngle < -0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z,
                targetAngle,
                turnSpeed * Time.deltaTime * Mathf.Abs(deltaAngle));
            transform.eulerAngles = transform.forward * angle;
        }
        else
        {
            transform.eulerAngles = transform.forward * targetAngle;
        }
    }

    public void MoveForward()
    {
        transform.position = transform.position + transform.up * moveSpeed * Time.deltaTime;

        var distanceFromCurrentWaypoint = (transform.position - waypoints[currentWaypoint]).magnitude;
        if (distanceFromCurrentWaypoint < 0.1f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }

    private void DrawOrientationLines(GameObject gameObject)
    {
        Debug.DrawRay(gameObject.transform.position, gameObject.transform.up * 5, Color.green);
        Debug.DrawRay(gameObject.transform.position, gameObject.transform.right * 5, Color.red);
    }

    
}

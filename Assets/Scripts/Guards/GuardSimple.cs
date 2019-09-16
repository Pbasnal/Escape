using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSimple : MonoBehaviour
{
    public Transform pathHolder;
    public Transform torch;

    public float speed = 1f;
    public float waitTime = 0.3f;
    public float turnSpeed = 1.0f;

    private int currentWaypoint = 0;

    private Vector3[] waypoints;

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

    // Start is called before the first frame update
    void Start()
    {
        waypoints = new Vector3[pathHolder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
        }

        torch.position = new Vector3(transform.position.x, transform.position.y, -0.01f);
        torch.rotation = new Quaternion(transform.rotation.z, -90, -90, transform.rotation.w);

        torch.parent = transform;

        StartCoroutine(FollowPath(waypoints, 0));
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator TurnToFace(Vector3 lookTarget)
    {
        var dir = (lookTarget - transform.position).normalized;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

        var deltaAngle = Mathf.DeltaAngle(transform.eulerAngles.z, targetAngle);
        while (deltaAngle > 0.05f || deltaAngle < -0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = transform.forward * angle;
            Debug.DrawRay(transform.position, transform.forward, Color.green);
            deltaAngle = Mathf.DeltaAngle(transform.eulerAngles.z, targetAngle);
            yield return null;
        }
    }

    IEnumerator FollowPath(Vector3[] waypoints, int startingIndex)
    {
        int targetWaypointIndex = startingIndex;
        transform.Translate(waypoints[targetWaypointIndex]);
        var targetWaypoint = waypoints[targetWaypointIndex];

        var dir = (targetWaypoint - transform.position).normalized;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

        float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, 100);
        transform.eulerAngles = Vector3.forward * angle;

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
            if (transform.position == targetWaypoint)
            {
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                targetWaypoint = waypoints[targetWaypointIndex];
                yield return new WaitForSeconds(waitTime);
                yield return StartCoroutine(TurnToFace(targetWaypoint));
            }
            yield return null;
        }
    }
}

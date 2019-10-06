using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    public float meshResolution;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask playerMask;
    public LayerMask obstacleMask;

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    [HideInInspector]
    public GuardSimple guard;
    public Transform player;

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool hit, Vector3 point, float dst, float angle)
        {
            this.hit = hit;
            this.point = point;
            this.dst = dst;
            this.angle = angle;
        }
    }

    private void Awake()
    {
        guard = gameObject.GetComponent<GuardSimple>();
    }

    void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
        StartCoroutine("FindPlayerWithDelay", .2f);
    }

    IEnumerator FindPlayerWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindPlayer();
        }
    }

    void LateUpdate()
    {
        DrawFieldOfView();
    }

    private void DrawFieldOfView()
    {
        var stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        var stepAngleSize = viewAngle / stepCount;

        var viewPoints = new List<Vector3>();

        for (int i = 0; i <= stepCount; i++)
        {
            var angle = -transform.eulerAngles.z - viewAngle / 2 + stepAngleSize * i;
            //Debug.DrawLine(transform.position, transform.position + DirFromAngle(-angle, true) * viewRadius, Color.green);
            var viewCastInfo = ViewCast(angle);
            viewPoints.Add(viewCastInfo.point);
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    private void FindPlayer()
    {
        player = null;
        var playerInRadius = Physics2D.OverlapCircle(transform.position, viewRadius, playerMask);

        if (playerInRadius == null)
        {
            return;
        }
        player = playerInRadius.transform;
        var playerDirection = (player.position - transform.position).normalized;
        if (Vector3.Angle(transform.up, playerDirection) >= viewAngle / 2)
        {
            return;
        }
        var distToPlayer = Vector3.Distance(transform.position, player.position);
        var obstacleInBetween = Physics2D.Raycast(transform.position, playerDirection, distToPlayer, obstacleMask);
        if (obstacleInBetween)
        {
            return;
        }
        guard.PlayerFound(player);
    }

    private ViewCastInfo ViewCast(float globalAngle)
    {
        var dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool isAngleGlobal)
    {
        if (!isAngleGlobal)
        {
            angleInDegrees -= transform.eulerAngles.z;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    }

}

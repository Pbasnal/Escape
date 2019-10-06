using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float meshResolution;
    
    public float viewRadiusMin;
    [Range(0, 360)]
    public float viewAngleMin;

    public float viewRadiusMax;
    [Range(0, 360)]
    public float viewAngleMax;

    public LayerMask playerMask;
    public LayerMask obstacleMask;

    public MeshFilter viewMeshFilter;

    [HideInInspector]
    public GuardSimple guard;
    [HideInInspector]
    public Transform player;


    [HideInInspector]
    public float viewRadius;
    [HideInInspector]
    public float viewAngle;

    private Mesh viewMesh;
    private bool playerFoundCalled = false;
    private bool playerLostCalled = false;

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
        viewRadius = viewRadiusMin;
        viewAngle = viewAngleMin;

        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
    }


    void Update()
    {
        FindPlayer();

        if (player != null && !playerFoundCalled)
        {
            guard.PlayerFound(player);

            playerFoundCalled = true;
            playerLostCalled = false;            
        }
        else if (player == null && !playerLostCalled)
        {
            guard.PlayerLost();

            playerLostCalled = true;
            playerFoundCalled = false;
        }
    }

    public void IncreaseFieldOfView()
    {
        viewRadius = viewRadiusMax;
        viewAngle = viewAngleMax;
    }

    public void DeccreaseFieldOfView()
    {
        viewRadius = viewRadiusMin;
        viewAngle = viewAngleMin;
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

    private float FindPlayer()
    {
        player = null;
        var playerInRadius = Physics2D.OverlapCircle(transform.position, viewRadius, playerMask);

        if (playerInRadius == null)
        {
            return float.MaxValue;
        }
        
        var playerDistance = playerInRadius.transform.position - transform.position;
        var playerDirection = playerDistance.normalized;
        if (Vector3.Angle(transform.up, playerDirection) >= viewAngle / 2)
        {
            return float.MaxValue;
        }
        var distToPlayer = Vector3.Distance(transform.position, playerInRadius.transform.position);
        var obstacleInBetween = Physics.Raycast(transform.position, playerDirection, distToPlayer, obstacleMask);
        if (obstacleInBetween)
        {
            return float.MaxValue;
        }

        player = playerInRadius.transform;
        return playerDistance.magnitude;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask playerMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public Transform player = null;

    void Start()
    {
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

    private void FindPlayer()
    {
        var playersInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, playerMask);

        player = null;
        for (int i = 0; i < playersInViewRadius.Length; i++)
        {
            var player = playersInViewRadius[i].transform;
            var playerDirection = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.up, playerDirection) < viewAngle / 2)
            {
                var distToPlayer = Vector3.Distance(transform.position, player.position);
                var rayCastHit = Physics2D.Raycast(transform.position, playerDirection, distToPlayer, obstacleMask);
                if (!rayCastHit)
                {
                    this.player = player;
                }
            }
        }
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

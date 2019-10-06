using UnityEngine;

public class EnemyPatrol : StateMachineBehaviour
{
    public float turnSpeed = 2.0f;
    public float moveSpeed = 1.0f;

    private GuardSimple guard;
    private Transform transform;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        guard = animator.gameObject.GetComponent<GuardSimple>();
        transform = guard.transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        TurnToWaypoint(guard.waypoints[guard.currentWaypoint]);
        MoveForward();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    private void TurnToWaypoint(Vector3 lookTarget)
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

    private void MoveForward()
    {
        transform.position = transform.position + transform.up * moveSpeed * Time.deltaTime;

        var distanceFromCurrentWaypoint = (transform.position - guard.waypoints[guard.currentWaypoint]).magnitude;
        if (distanceFromCurrentWaypoint < 0.1f)
        {
            guard.currentWaypoint = (guard.currentWaypoint + 1) % guard.waypoints.Length;
        }
    }
}

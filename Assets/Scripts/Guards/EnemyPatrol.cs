﻿using UnityEngine;

public class EnemyPatrol : StateMachineBehaviour
{
    protected GuardSimple guard;
 
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        guard = animator.gameObject.GetComponent<GuardSimple>();

        var minDist = float.MaxValue;
        int closestWaypoint = 0;

        for (int i = 0; i < guard.waypoints.Length; i++)
        {
            var distance = (guard.transform.position - guard.waypoints[i]).magnitude;
            if (distance < minDist)
            {
                closestWaypoint = i;
                minDist = distance;
            }
        }

        guard.currentWaypoint = closestWaypoint;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        guard.TurnToTarget(guard.waypoints[guard.currentWaypoint]);
        guard.MoveForward();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    
}

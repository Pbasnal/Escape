using UnityEngine;

public class AlertPatrol : EnemyPatrol
{
    private float alertTime = 0.0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        alertTime = 0.0f;
        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        alertTime += Time.deltaTime;
        if ((int)alertTime >= guard.alertDurationInSecs)
        {
            animator.SetBool("TargetLost", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        guard.fieldOfView.DeccreaseFieldOfView();
        animator.SetBool("TargetLost", false);
    }    
}

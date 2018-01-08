using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingController : StateMachineBehaviour
{
    private bool waiting = false;
    private float waitingBeginAt;
    private GameObject navigator;
    private GameObject player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navigator = GameObject.FindGameObjectWithTag("Navigator");
        player = GameObject.FindGameObjectWithTag("Player");
        waiting = true;
        waitingBeginAt = Time.time;
        animator.SetFloat("waiting_time", 0);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (waiting)
        {
            animator.SetFloat("waiting_time", Time.time - waitingBeginAt);
        }

        // look at player
        var KITTEN_ROTATION_OFFSET = new Vector3(0, 20, 0); // kitten モデルは向きが傾いているのでそれを補正する
        var n2p = new Vector3(player.transform.position.x - navigator.transform.position.x, navigator.transform.forward.y, player.transform.position.z - navigator.transform.position.z).normalized;
        navigator.transform.forward = n2p;
        navigator.transform.Rotate(KITTEN_ROTATION_OFFSET);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        waiting = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}

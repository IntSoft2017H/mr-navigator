using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayerController : StateMachineBehaviour
{
    public float transitionTime = 2;

    private GameObject navigator;
    private GameObject player;
    private float startAt;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navigator = GameObject.FindGameObjectWithTag("Navigator");
        player = GameObject.FindGameObjectWithTag("Player");
        startAt = Time.time;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var KITTEN_ROTATION_OFFSET = new Vector3(0, 20, 0); // kitten モデルは向きが傾いているのでそれを補正する
        float t = (Time.time - startAt) / transitionTime;
        var n2p = new Vector3(player.transform.position.x - navigator.transform.position.x, navigator.transform.forward.y, player.transform.position.z - navigator.transform.position.z).normalized;
        if (t <= 1)
        {
            navigator.transform.forward = Vector3.Lerp(navigator.transform.forward, n2p, t);
//            navigator.transform.Rotate(KITTEN_ROTATION_OFFSET);
        }
        else
        {
            navigator.transform.forward = n2p;
            navigator.transform.Rotate(KITTEN_ROTATION_OFFSET);
            animator.Play("Waiting");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}

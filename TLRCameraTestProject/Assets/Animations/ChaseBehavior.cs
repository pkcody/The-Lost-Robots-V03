using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseBehavior : StateMachineBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    float attackRange = 1.7f;
    float chaseRange = 20f;

    public Transform closestTrans;

    public CharacterMovement[] cms;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.transform.parent.GetComponent<NavMeshAgent>();
        //closestTrans = FindObjectOfType<CharacterMovement>().transform;
        if(cms.Length == 0)
        {
            cms = FindObjectsOfType<CharacterMovement>();
        }
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float closestPlayerDist = float.PositiveInfinity;
        foreach (CharacterMovement cm in cms)
        {
            float _distance = Vector3.Distance(cm.transform.position, agent.transform.position);
            if (_distance < closestPlayerDist)
            {
                closestPlayerDist = _distance;
                closestTrans = cm.transform;
            }
        }
        player = closestTrans;

        agent.SetDestination(player.position);


        //Debug.Log(closestPlayerDist + "c");
        //Debug.Log(closestTrans.name + "c");
        if (closestPlayerDist < attackRange)
        {
            //Debug.Log("lolly");
            animator.SetBool("attack", true);
        }
        else if(closestPlayerDist > chaseRange)
        {
            animator.SetBool("chase", false);

        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseBehavior : StateMachineBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    float attackRange = 2.6f;
    float chaseRange = 20f;

    public Transform closestTrans;
    public float closestPlayerDist;

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
        closestPlayerDist = float.PositiveInfinity;
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

        try
        {
            agent.SetDestination(player.position);

        }
        catch
        {

        }


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
        try
        {
            agent.SetDestination(agent.transform.position);

        }
        catch
        {
            
        }
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

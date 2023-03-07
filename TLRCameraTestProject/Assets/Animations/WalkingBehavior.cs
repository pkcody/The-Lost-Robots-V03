using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkingBehavior : StateMachineBehaviour
{
    public float timer;
    public List<Transform> waypoints = new List<Transform>();
    public NavMeshAgent agent;
    public Transform player;
    public float chaseRange = 20f;

    public Transform closestTrans;

    public CharacterMovement[] cms;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        foreach (Transform t in animator.transform.parent.parent.GetChild(0).GetComponentsInChildren<Transform>())
        {
            waypoints.Add(t);
        }

        agent = animator.transform.parent.GetComponent<NavMeshAgent>();
        agent.SetDestination(waypoints[0].position);
        //closestTrans = FindObjectOfType<CharacterMovement>().transform;
        if (cms.Length == 0)
        {
            cms = FindObjectsOfType<CharacterMovement>();
        }

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float closestPlayerDist = float.PositiveInfinity;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(waypoints[Random.Range(0, waypoints.Count)].position);
        }
        timer += Time.deltaTime;

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

        //Debug.Log(closestPlayerDist + "w");
        //Debug.Log(closestTrans.name + "w");
        if (closestPlayerDist < chaseRange)
        {
            animator.SetBool("chase", true);
        }

        if (timer > 10)
        {
            animator.SetBool("walk", true);

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

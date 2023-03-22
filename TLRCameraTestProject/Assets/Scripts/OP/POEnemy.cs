using UnityEngine.AI;

public class POEnemy : PoolableObject
{
    //public EnemyMovement Movement;
    public NavMeshAgent Agent;
    public int Health = 100;

    public override void OnDisable()
    {
        base.OnDisable();

        Agent.enabled = false;
    }
}
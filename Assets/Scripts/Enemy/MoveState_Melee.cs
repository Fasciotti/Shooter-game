using UnityEngine;
using UnityEngine.AI;

public class MoveState_Melee : EnemyState
{
    private readonly Enemy_Melee enemy;

    private Vector3 destination;

    public MoveState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();

        destination = enemy.GetPatronDestination();
        enemy.agent.SetDestination(destination);
    }

    public override void Exit()
    {
        base.Exit();

        Debug.Log("I left move state");

    }

    public override void Update()
    {
        base.Update();

        enemy.transform.rotation = enemy.FaceTarget(GetNextPathPoint());

        if (enemy.agent.remainingDistance <= enemy.agent.stoppingDistance && enemy.agent.hasPath)
            stateMachine.ChangeState(enemy.idleState);
        
    }

    // This method can be substituted by the NavMesh property steeringTarget
    // that automatically gives the rotation to face the path.
    private Vector3 GetNextPathPoint()
    {
        NavMeshAgent agent = enemy.agent;
        NavMeshPath path = agent.path;

        if (path.corners.Length < 2)
            return agent.destination;

        for (int i = 0; i < path.corners.Length; i++)
        {
            if (Vector3.Distance(agent.transform.position, path.corners[i]) < 1)
                return path.corners[i + 1];
        }

        return agent.destination;
    }

}

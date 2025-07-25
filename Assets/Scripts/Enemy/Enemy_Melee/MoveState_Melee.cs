using UnityEngine;
using UnityEngine.AI;

public class MoveState_Melee : EnemyState
{
    private Enemy_Melee enemy;

    private Vector3 destination;

    public MoveState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName)
        : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();

        destination = enemy.GetPatrolDestination();
        enemy.agent.SetDestination(destination);

        enemy.agent.speed = enemy.moveSpeed;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.FaceTarget(GetNextPathPoint());

        if (enemy.agent.remainingDistance <= enemy.agent.stoppingDistance && enemy.agent.hasPath)
            stateMachine.ChangeState(enemy.IdleState);
        
    }

    // This method can be substituted by the NavMesh property steeringTarget
    // that automatically gives the rotation to face the path.
    

}

using UnityEngine;

public class MoveState_Range : EnemyState
{
    private Enemy_Range enemy;
    private Vector3 destination;

    public MoveState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.agent.isStopped = false;
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
}

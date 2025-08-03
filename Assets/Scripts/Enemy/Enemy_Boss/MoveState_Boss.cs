using UnityEngine;

public class MoveState_Boss : EnemyState
{
    private Enemy_Boss enemy;
    private Vector3 destination;

    public MoveState_Boss(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Boss;
    }

    public override void Enter()
    {
        base.Enter();

        destination = enemy.GetPatrolDestination();
        enemy.agent.SetDestination(destination);
        enemy.agent.isStopped = false;

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

        // Stopping distace must not be 0
        if (enemy.agent.remainingDistance <= enemy.agent.stoppingDistance && enemy.agent.hasPath)
            stateMachine.ChangeState(enemy.IdleState);
    }
}

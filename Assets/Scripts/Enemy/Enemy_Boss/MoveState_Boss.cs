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

        if (enemy.inBattleMode)
        {
            Vector3 playerPos = enemy.player.transform.position;

            enemy.agent.SetDestination(playerPos);

            if (enemy.CanDoJumpAttack())
            {
                stateMachine.ChangeState(enemy.JumpAttackState);
            }
            else if (Vector3.Distance(enemy.transform.position, playerPos) < enemy.attackRange)
            {
                stateMachine.ChangeState(enemy.AttackState);
            }
        }
        else
        {
            // Stopping distace must not be 0
            if ((Vector3.Distance(enemy.transform.position, destination) <= enemy.agent.stoppingDistance) && enemy.agent.hasPath)
                stateMachine.ChangeState(enemy.IdleState);
        }


    }
}

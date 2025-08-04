using UnityEngine;

public class IdleState_Boss : EnemyState
{
    private Enemy_Boss enemy;

    public IdleState_Boss(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Boss;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.agent.isStopped = true;
        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerInAttackRange() && enemy.inBattleMode)
        {
            stateMachine.ChangeState(enemy.AttackState);
        }

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.MoveState);
        }
    }
}

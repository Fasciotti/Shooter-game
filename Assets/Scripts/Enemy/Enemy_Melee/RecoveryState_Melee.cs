using UnityEngine;

public class RecoveryState_Melee : EnemyState
{
    Enemy_Melee enemy;

    public RecoveryState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) :
        base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.agent.isStopped = true;

        enemy.visuals.WeaponModelActive(true);

    }

    public override void Exit()
    {
        base.Exit();

        enemy.agent.isStopped = false;
    }

    public override void Update()
    {
        base.Update();

        enemy.FaceTarget(enemy.player.transform.position);

        if (triggerCalled)
        {
            if (enemy.CanThrowAxe())
            {
                stateMachine.ChangeState(enemy.AbilityState);
            }
            else if (enemy.IsPlayerInAttackRange())
            {
                stateMachine.ChangeState(enemy.AttackState);
            }
            else
            {
                stateMachine.ChangeState(enemy.ChaseState);
            }
        }

    }
}

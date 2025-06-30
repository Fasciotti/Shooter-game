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

    }

    public override void Exit()
    {
        base.Exit();

        enemy.agent.isStopped = false;
    }

    public override void Update()
    {
        base.Update();

        enemy.transform.rotation = enemy.FaceTarget(enemy.player.transform.position);

        if (triggerCalled)
        {
            if (enemy.IsPlayerInAttackRange())
                stateMachine.ChangeState(enemy.attackState);
            else
                stateMachine.ChangeState(enemy.chaseState);
        }

    }
}

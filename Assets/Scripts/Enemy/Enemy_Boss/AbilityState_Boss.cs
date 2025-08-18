using UnityEngine;

public class AbilityState_Boss : EnemyState
{
    private Enemy_Boss enemy;

    public AbilityState_Boss(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Boss;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;
        stateTimer = enemy.abilityDuration;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.FaceTarget(enemy.player.transform.position);

        if (ShouldDisableFlamethrower())
        {
            enemy.FlameThrowerActive(false);
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.MoveState);
        }
    }

    private bool ShouldDisableFlamethrower()
    {
        return stateTimer < 0 && enemy.flameThrowerActive;
    }

    public override void AbilityTrigger()
    {
        base.AbilityTrigger();

        if (enemy.bossAttackType == BossAttackType.Flamethrower)
        {
            enemy.FlameThrowerActive(true);
        }
        else if (enemy.bossAttackType == BossAttackType.Hammer)
        {
            enemy.HammerActive(true);
        }
    }
}

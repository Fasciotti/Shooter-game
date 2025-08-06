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
        stateTimer = enemy.flameThrowerDuration;

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.FaceTarget(enemy.player.transform.position);

        if (stateTimer < 0)
        {
            enemy.ActivateFlameThrower(false);
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.MoveState);
        }
    }
    public override void AbilityTrigger()
    {
        base.AbilityTrigger();


        enemy.ActivateFlameThrower(true);
    }
}

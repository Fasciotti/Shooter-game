using UnityEngine;

public class IdleState_Range : EnemyState
{
    private Enemy_Range enemy;

    public IdleState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;


        if (enemy.HandHoldIndex() == 1)
            enemy.visuals.IKActive(false, false);
        else
            enemy.visuals.IKActive(true, false);

        // If bigger than 6, it will include an idle animation that is lower
        float randomIndex = enemy.idleTime >= 6 ? Random.Range(0, 3) : Random.Range(0, 2);

        enemy.anim.SetFloat("IdleIndex", randomIndex); // Random idle animation
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.MoveState);
        }
    }
}

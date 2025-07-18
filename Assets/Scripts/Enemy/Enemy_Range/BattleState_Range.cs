using UnityEngine;

public class BattleState_Range : EnemyState
{
    private Enemy_Range enemy;
    private float lastTimeShot = -10;

    public BattleState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        enemy.agent.isStopped = true;
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        enemy.FaceTarget(enemy.player.transform.position);

        if (Time.time > lastTimeShot + (1f / enemy.fireRate))
        {
            lastTimeShot = Time.time;
            enemy.FireSingleBullet();
        }

    }
}

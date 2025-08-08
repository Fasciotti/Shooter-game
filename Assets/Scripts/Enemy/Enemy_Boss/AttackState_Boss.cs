using UnityEngine;

public class AttackState_Boss : EnemyState
{
    private Enemy_Boss enemy;
    public float lastTimeAttacked { get; private set; }
    public AttackState_Boss(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Boss;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.anim.SetFloat("AttackIndex", Random.Range(0, 2));
        enemy.agent.isStopped = true;

        stateTimer = 1;
    }

    public override void Exit()
    {
        base.Exit();

        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
            enemy.FaceTarget(enemy.player.transform.position, 20);

        if (triggerCalled)
        {
            if (enemy.IsPlayerInAttackRange())
            {
                stateMachine.ChangeState(enemy.IdleState); // Functioning as a recovery state
            }
            else
            {
                stateMachine.ChangeState(enemy.MoveState);
            }

        }
    }
}

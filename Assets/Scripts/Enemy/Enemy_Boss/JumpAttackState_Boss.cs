using UnityEngine;

public class JumpAttackState_Boss : EnemyState
{
    private Enemy_Boss enemy;
    private Vector3 lastPlayerPos;

    private float jumpAttackFlySpeed;

    public JumpAttackState_Boss(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Boss;
    }

    public override void Enter()
    {
        base.Enter();
        lastPlayerPos = enemy.player.transform.position;

        enemy.agent.isStopped = true;

        jumpAttackFlySpeed = Vector3.Distance(lastPlayerPos, enemy.transform.position) / enemy.jumpTimeToTarget;

        enemy.FaceTarget(lastPlayerPos, 1000);

        enemy.bossVisuals.PlaceLandingZoneEffect(lastPlayerPos);
        enemy.bossVisuals.WeaponTrailActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.bossVisuals.WeaponTrailActive(false);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.ManualMovementActive())
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, lastPlayerPos, Time.deltaTime * jumpAttackFlySpeed);
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.MoveState);
        }

    }
}

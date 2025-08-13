using UnityEngine;

public class MoveState_Boss : EnemyState
{
    private Enemy_Boss enemy;
    private Vector3 destination;

    private float actionTimer;
    private bool speedUpActivated;

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
        ResetSpeed();

        actionTimer = enemy.actionCooldown;
        stateTimer = 0f;
    }


    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        actionTimer -= Time.deltaTime;

        enemy.FaceTarget(GetNextPathPoint());

        if (enemy.inBattleMode)
        {
            Vector3 playerPos = enemy.player.transform.position;

            enemy.agent.SetDestination(playerPos);

            if (CanSpeedUp())
                SpeedUp();

            if (actionTimer < 0)
            {
                PerformAction();
            }
            else if (Vector3.Distance(enemy.transform.position, playerPos) < enemy.attackRange)
            {
                stateMachine.ChangeState(enemy.AttackState);
            }
        }
        else
        {
            Debug.Log(Vector3.Distance(enemy.transform.position, destination) <= enemy.agent.stoppingDistance);
            // Stopping distace must not be 0
            if ((Vector3.Distance(enemy.transform.position, destination) <= enemy.agent.stoppingDistance))
            {
                Debug.Log("hi");
                stateMachine.ChangeState(enemy.IdleState);

            }
        }


    }

    private void SpeedUp()
    {
        speedUpActivated = true;
        enemy.agent.speed = enemy.runSpeed;
        enemy.anim.SetFloat("MoveIndex", 1);
    }
    private void ResetSpeed()
    {
        speedUpActivated = false;
        enemy.agent.speed = enemy.moveSpeed;
        enemy.anim.SetFloat("MoveIndex", 0);
    }
    private bool CanSpeedUp()
    {
        if (speedUpActivated)
            return false;

        if (Time.time > enemy.AttackState.lastTimeAttacked + enemy.speedUpCooldown)
            return true;

        return false;
    }

    // This makes specific cooldown pretty much useless
    private void PerformAction()
    {
        actionTimer = enemy.actionCooldown;

        if (Random.Range(0, 2) == 0) // 0 or 1
        {
            if (enemy.CanUseAbility())
            {
                stateMachine.ChangeState(enemy.AbilityState);
            }
        }
        else
        {
            if (enemy.CanDoJumpAttack())
            {
                stateMachine.ChangeState(enemy.JumpAttackState);
            }
        }
    }
}

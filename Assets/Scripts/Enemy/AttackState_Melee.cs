using Unity.VisualScripting;
using UnityEngine;

public class AttackState_Melee : EnemyState
{

    private Enemy_Melee enemy;
    private Vector3 attackDirection;
    private float attackMoveSpeed;

    private const float MAX_ATTACK_DISTANCE = 50f;

    public AttackState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;

        attackDirection = enemy.transform.position + (enemy.transform.forward * MAX_ATTACK_DISTANCE);

        attackMoveSpeed = enemy.attackData.moveSpeed;
        enemy.anim.SetFloat("AttackAnimationSpeed", enemy.attackData.animationSpeed);
        enemy.anim.SetFloat("AttackIndex", enemy.attackData.attackIndex);

    }

    public override void Exit()
    {
        base.Exit();

        enemy.anim.SetFloat("RecoveryIndex", 0);

        if (enemy.IsPlayerInAttackRange())
            enemy.anim.SetFloat("RecoveryIndex", 1);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.ManualRotationActive())
        {
            enemy.transform.rotation = enemy.FaceTarget(enemy.player.transform.position);
            attackDirection = enemy.transform.position + (enemy.transform.forward * MAX_ATTACK_DISTANCE);
        }


        if (enemy.ManualMovementActive())
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, attackDirection, attackMoveSpeed * Time.deltaTime);
        }


        if (triggerCalled)
        {
            if (enemy.IsPlayerInAttackRange())
                stateMachine.ChangeState(enemy.recoveryState);
            else
                stateMachine.ChangeState(enemy.chaseState);
            
        }
    }
}

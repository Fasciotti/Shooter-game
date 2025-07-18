using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

        enemy.UpdateAttackData();
        enemy.visuals.TrailEffectActive(true);

        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;

        attackDirection = enemy.transform.position + (enemy.transform.forward * MAX_ATTACK_DISTANCE);

        attackMoveSpeed = enemy.attackData.moveSpeed;
        enemy.anim.SetFloat("AttackAnimationSpeed", enemy.attackData.animationSpeed);
        enemy.anim.SetFloat("AttackIndex", enemy.attackData.attackIndex);
        enemy.anim.SetFloat("AttackSlashIndex", Random.Range(0, 6));
        enemy.anim.SetFloat("AttackSpinIndex", Random.Range(0, 2));

    }

    public override void Exit()
    {
        base.Exit();
        enemy.visuals.TrailEffectActive(false);
        SetupNextAttack();
    }

    private void SetupNextAttack()
    {
        int recoveryIndex = PlayerClose() ? 1 : 0;

        if (enemy.meleeType == EnemyMelee_Type.Dodge)
            recoveryIndex = 1;

        if (!LookingAtPlayer())
            recoveryIndex = 0;

        enemy.anim.SetFloat("RecoveryIndex", recoveryIndex);

        enemy.attackData = UpdateAttackData();
    }

    public override void Update()
    {

        base.Update();

        if (enemy.ManualRotationActive())
        {
            enemy.FaceTarget(enemy.player.transform.position); 
            attackDirection = enemy.transform.position + (enemy.transform.forward * MAX_ATTACK_DISTANCE);
        }


        if (enemy.ManualMovementActive())
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, attackDirection, attackMoveSpeed * Time.deltaTime);
        }


        if (triggerCalled)
        {
            if (enemy.IsPlayerInAttackRange())
            {
                stateMachine.ChangeState(enemy.recoveryState);
            }
            else if (Random.Range(0, 2) == 1)
            {
                stateMachine.ChangeState(enemy.chaseState);
            }
            else
            {
                stateMachine.ChangeState(enemy.recoveryState);

            }

        }
    }

    private bool LookingAtPlayer() =>
    Vector3.Dot(
        enemy.transform.forward,
        (enemy.player.transform.position - enemy.transform.position).normalized
    ) > 0.4f;
    

    private bool PlayerClose() => Vector3.Distance(enemy.transform.position, enemy.player.transform.position) < 1;

    public AttackData_Enemy_Melee UpdateAttackData()
    {
        List<AttackData_Enemy_Melee> validAttacks = new List<AttackData_Enemy_Melee>(enemy.attackList);

        if (PlayerClose())
        {
            validAttacks.RemoveAll(parameter => parameter.attackType == AttackType_Melee.ChargeAttack);
        }

        int random = Random.Range(0, validAttacks.Count);
        return validAttacks[random];

    }
}

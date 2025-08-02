using UnityEngine;

public class AbilityState_Melee : EnemyState
{
    private Enemy_Melee enemy;

    private const float MAX_MOVEMENT_DISTANCE = 20f;

    private Vector3 moveDirection;

    private float moveSpeed;

    public AbilityState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();

        moveDirection = enemy.transform.position + (enemy.transform.forward * MAX_MOVEMENT_DISTANCE);

        moveSpeed = enemy.moveSpeed;

        enemy.agent.isStopped = true;

        enemy.visuals. WeaponModelActive(true);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.moveSpeed = moveSpeed;

        enemy.agent.isStopped = false;

        enemy.anim.SetFloat("RecoveryIndex", 1);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.ManualRotationActive())
        {
            enemy.FaceTarget(enemy.player.transform.position);
            moveDirection = enemy.transform.position + (enemy.transform.forward * MAX_MOVEMENT_DISTANCE);
        }


        if (enemy.ManualMovementActive())
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, moveDirection, enemy.moveSpeed * Time.deltaTime);
        }


        if (triggerCalled)
            stateMachine.ChangeState(enemy.RecoveryState);
    }

    public override void AbilityTrigger()
    {
        base.AbilityTrigger();

        enemy.ThrowAxe();

    }
}

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

        if (enemy.meleeType == EnemyMelee_Type.Axe)
        {
            base.animBoolName = "AxeThrow";
            return;
        }

        base.animBoolName = "Chase";
    }

    public override void Enter()
    {
        base.Enter();

        moveDirection = enemy.transform.position + (enemy.transform.forward * MAX_MOVEMENT_DISTANCE);

        moveSpeed = enemy.moveSpeed;
    }

    public override void Exit()
    {
        base.Exit();

        enemy.moveSpeed = moveSpeed;

        enemy.anim.SetFloat("RecoveryIndex", 0);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.ManualRotationActive())
        {
            enemy.transform.rotation = enemy.FaceTarget(enemy.player.transform.position);
            moveDirection = enemy.transform.position + (enemy.transform.forward * MAX_MOVEMENT_DISTANCE);
        }


        if (enemy.ManualMovementActive())
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, moveDirection, enemy.moveSpeed * Time.deltaTime);
        }


        if (triggerCalled)
            stateMachine.ChangeState(enemy.recoveryState);
    }
}

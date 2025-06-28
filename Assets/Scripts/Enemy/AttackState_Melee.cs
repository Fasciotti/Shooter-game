using Unity.VisualScripting;
using UnityEngine;

public class AttackState_Melee : EnemyState
{

    private Enemy_Melee enemy;
    private Vector3 attackDirection;
    private float attackMoveSpeed;

    private const float MAX_ATTACK_DISTANCE = 50f;



    // THIS IS A TEMPORARY SOLUTION!
    private float angle;
    private const float ANGLE_MISS_TOLERANCE = 10f;
    private bool randomBool;

    public AttackState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;

        angle = Vector3.Angle(enemy.transform.forward, enemy.player.transform.position - enemy.transform.position);

        randomBool = Random.Range(0, 2) == 1;

        attackMoveSpeed = enemy.attackData.moveSpeed;
        enemy.anim.SetFloat("AttackAnimationSpeed", enemy.attackData.animationSpeed);

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // THIS IS A TEMPORARY SOLUTION!
        // Verifies if the enemy is actually looking at the player before attacking.
        // This prevents the enemy from attacking to the opposite side if the player is behind them.
        // This is useful when transitioning from attackState to chaseState directly,
        // as recoveryState handles rotation and needs triggerCalled variable to change state.
        if (angle > ANGLE_MISS_TOLERANCE)
        {
            enemy.transform.rotation = enemy.FaceTarget(enemy.player.transform.position);
            angle = Vector3.Angle(enemy.transform.forward, enemy.player.transform.position - enemy.transform.position);
            return;
        }
            
        attackDirection = enemy.transform.position + (enemy.transform.forward * MAX_ATTACK_DISTANCE);

        if (enemy.ManualMovementActive())
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, attackDirection, attackMoveSpeed * Time.deltaTime);
        }


        if (triggerCalled && randomBool)        
            stateMachine.ChangeState(enemy.recoveryState);
        else if (triggerCalled && !randomBool)
            stateMachine.ChangeState(enemy.chaseState);

    }
}

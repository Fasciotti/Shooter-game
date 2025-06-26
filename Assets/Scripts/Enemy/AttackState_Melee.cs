using UnityEngine;

public class AttackState_Melee : EnemyState
{
    private Enemy_Melee enemy;

    private Vector3 attackDirection;

    private const float MAX_ATTACK_DISTANCE = 50f;

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

        attackDirection = enemy.transform.position + (enemy.transform.forward * MAX_ATTACK_DISTANCE);

        randomBool = Random.Range(0, 2) == 1;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.ManualMovementActive())
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, attackDirection, enemy.attackMoveSpeed * Time.deltaTime);
        }


        if (triggerCalled && randomBool)        
            stateMachine.ChangeState(enemy.recoveryState);
        else if (triggerCalled && !randomBool)
            stateMachine.ChangeState(enemy.chaseState);

    }
}

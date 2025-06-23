using UnityEngine;

public class MoveState_Melee : EnemyState
{
    private readonly Enemy_Melee enemy;

    private Vector3 destination;

    public MoveState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();

        destination =  enemy.GetPatronDestination();
    }

    public override void Exit()
    {
        base.Exit();

        Debug.Log("I left move state");
    }

    public override void Update()
    {
        base.Update();

        enemy.agent.SetDestination(destination);


        if (enemy.agent.remainingDistance <= 1 && enemy.agent.hasPath)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}

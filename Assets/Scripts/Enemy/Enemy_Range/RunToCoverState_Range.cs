using UnityEngine;
using UnityEngine.AI;

public class RunToCoverState_Range : EnemyState
{
    private Enemy_Range enemy;
    private Vector3 destination;
    public RunToCoverState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.agent.isStopped = false;
        enemy.agent.speed = enemy.runSpeed;

        enemy.visuals.IKActive(true, false);

        destination = enemy.coverDestination.position;
        enemy.agent.destination = destination;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Vector3.Distance(enemy.transform.position, destination) < 0.5f)
        {
            stateMachine.ChangeState(enemy.BattleState);
        }
    }
}

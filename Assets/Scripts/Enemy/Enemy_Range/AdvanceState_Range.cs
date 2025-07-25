using UnityEngine;

public class AdvanceState_Range : EnemyState
{
    private Enemy_Range enemy;
    public AdvanceState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.visuals.IKActive(true, true);

        enemy.agent.isStopped = false;
        enemy.agent.speed = enemy.advanceStateSpeed;
    }

    public override void Update()
    {
        base.Update();

        Vector3 playerPos = enemy.player.transform.position;

        enemy.agent.SetDestination(playerPos);
        enemy.FaceTarget(playerPos);

        if (Vector3.Distance(enemy.transform.position, enemy.player.transform.position) < enemy.advanceStoppingDistance)
        {
            enemy.currentCover.isOccupied = false;
            stateMachine.ChangeState(enemy.BattleState);
        }

    }
}

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

        enemy.agent.SetDestination(enemy.aim.position);
        enemy.FaceTarget(GetNextPathPoint());
        enemy.UpdateAimPosition();

        if (CanChangeToBattleState())
        {
            stateMachine.ChangeState(enemy.BattleState);
        }
    }
    private bool CanChangeToBattleState()
    {
        return Vector3.Distance(enemy.transform.position, enemy.player.transform.position) < enemy.advanceStoppingDistance && enemy.IsSeeingPlayer();
    }
}

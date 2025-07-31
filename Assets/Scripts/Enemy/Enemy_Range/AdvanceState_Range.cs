using UnityEngine;

public class AdvanceState_Range : EnemyState
{
    private Enemy_Range enemy;

    public float lastTimeAdvanced { get; private set; }
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

        if (enemy.IsUnstoppable())
        {
            enemy.visuals.IKActive(true, false);
            stateTimer = enemy.minAdvanceDuration;
        }

        if (enemy.HandHoldIndex() == 1)
            enemy.visuals.IKActive(false, false);
    }

    public override void Exit()
    {
        base.Exit();

        lastTimeAdvanced = Time.time;
    }

    public override void Update()
    {
        base.Update();

        enemy.agent.SetDestination(enemy.aim.position);
        enemy.FaceTarget(GetNextPathPoint());
        enemy.UpdateAimPosition();

        if (CanChangeToBattleState() && enemy.IsSeeingPlayer())
        {
            stateMachine.ChangeState(enemy.BattleState);
        }
    }
    private bool CanChangeToBattleState()
    {
        bool closeEnough = Vector3.Distance(enemy.transform.position, enemy.player.transform.position) < enemy.advanceStoppingDistance;

        if (enemy.IsUnstoppable())
            return closeEnough || stateTimer < 0;
        else
            return closeEnough;
    }
}

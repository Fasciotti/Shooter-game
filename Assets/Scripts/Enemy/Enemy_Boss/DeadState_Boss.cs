public class DeadState_Boss : EnemyState
{
    private Enemy_Boss enemy;
    private bool interactionDisabled;

    public DeadState_Boss(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Boss;
    }

    public override void Enter()
    {
        base.Enter();

        interactionDisabled = false;

        enemy.agent.isStopped = true;
        enemy.anim.enabled = false;

        enemy.FlameThrowerActive(false);

        enemy.ragdoll.RagdollActive(true);

        stateTimer = 2f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // Uncomment to disable interaction with the dead enemy. (Recommended, not disabling affects performance)
        //DisableInteraction();

    }

    private void DisableInteraction()
    {
        if (stateTimer <= 0 && !interactionDisabled)
        {
            interactionDisabled = true;
            enemy.ragdoll.RagdollActive(false);
            enemy.ragdoll.ColliderActive(false);
            enemy.agent.enabled = false;
        }
    }
}

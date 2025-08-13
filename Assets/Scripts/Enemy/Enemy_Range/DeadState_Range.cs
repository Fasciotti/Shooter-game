using UnityEngine;

public class DeadState_Range : EnemyState
{
    private Enemy_Range enemy;
    private bool interactionDisabled;

    public DeadState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();

        if (enemy.ThrowGranadeState.IsThrowing)
        {
            enemy.ThrowGranade();
        }

        interactionDisabled = false;

        enemy.agent.isStopped = true;
        enemy.anim.enabled = false;

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

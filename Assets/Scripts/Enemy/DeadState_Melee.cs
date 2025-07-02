using UnityEngine;

public class DeadState_Melee : EnemyState
{
    private Enemy_Melee enemy;
    private EnemyRagdoll ragdoll;

    #pragma warning disable S4487 // Unread "private" fields should be removed
    private bool interactionDisabled;


    public DeadState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
        ragdoll = enemy.gameObject.GetComponentInChildren<EnemyRagdoll>();
    }
    public override void Enter()
    {
        base.Enter();

        interactionDisabled = false;

        enemy.agent.isStopped = true;
        enemy.anim.enabled = false;

        ragdoll.RagdollActive(true);

        stateTimer = 2f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // Uncomment to disable interaction with the dead enemy. (Not recommended, affects performance)
        /*
        if (stateTimer <= 0 && !interactionDisabled)
        {
            interactionDisabled = true;
            ragdoll.RagdollActive(false);
            ragdoll.ColliderActive(false);
            enemy.agent.enabled = false;
        }
        */
    }
}

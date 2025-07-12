using UnityEngine;

public class DeadState_Melee : EnemyState
{
    private Enemy_Melee enemy;
    private Enemy_Ragdoll ragdoll;

    private bool interactionDisabled;


    public DeadState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
        ragdoll = enemy.gameObject.GetComponentInChildren<Enemy_Ragdoll>();
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
        //DisableInteraction();

    }

    private void DisableInteraction()
    {
        if (stateTimer <= 0 && !interactionDisabled)
        {
            interactionDisabled = true;
            ragdoll.RagdollActive(false);
            ragdoll.ColliderActive(false);
            enemy.agent.enabled = false;
        }
    }
}

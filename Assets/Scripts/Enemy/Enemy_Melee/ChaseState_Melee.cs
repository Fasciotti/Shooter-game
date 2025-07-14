using UnityEngine;

public class ChaseState_Melee : EnemyState
{
    private Enemy_Melee enemy;

    private float lastUpdateDestinationTime;
    private float updateDestinationCooldown = 0.25f;

    public ChaseState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) 
        : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.WeaponModelActive(true);

        enemy.agent.isStopped = false;

        enemy.agent.speed = enemy.chaseSpeed;

        //enemy.agent.acceleration = enemy.chaseAcceleration;
    }

    public override void Exit()
    {
        base.Exit();

        //enemy.agent.acceleration = enemy.standardAcceleration;
    }

    public override void Update()
    {
        base.Update();


        if (enemy.IsPlayerInAttackRange())
        {
            stateMachine.ChangeState(enemy.attackState);
        }

        // Temporary
        if (enemy.agent.path.corners.Length > 2 )
        {
            enemy.FaceTarget(enemy.agent.path.corners[1]);
        }
        else
        {
            enemy.FaceTarget(enemy.player.transform.position);
        }

        if (CanUpdateDestination())
        {
            enemy.agent.destination = enemy.player.transform.position;
        }
    }

    private bool CanUpdateDestination()
    {
        if (Time.time >= lastUpdateDestinationTime + updateDestinationCooldown)
        {
            lastUpdateDestinationTime = Time.time;
            return true;
        }


        return false;
    }
}

using UnityEngine;

public class AbilityState_Melee : EnemyState
{
    private Enemy_Melee enemy;

    private const float MAX_MOVEMENT_DISTANCE = 20f;

    private Vector3 moveDirection;

    private float moveSpeed;

    private EnemyAxe enemyAxe;

    public AbilityState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();

        moveDirection = enemy.transform.position + (enemy.transform.forward * MAX_MOVEMENT_DISTANCE);

        moveSpeed = enemy.moveSpeed;

        if (!enemy.pulledWeapon.gameObject.activeSelf)
        {
            if (!enemy.hiddenWeapon.gameObject.activeSelf)
            {
                enemy.anim.SetBool("WeaponEquip", true);
            }
            else
            {
                enemy.PullWeapon(); //This probably will never going to be executed
            }
        }

        enemy.agent.isStopped = true;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.agent.isStopped = false;

        enemy.moveSpeed = moveSpeed;


        enemy.anim.SetFloat("RecoveryIndex", 0);
        enemy.SetEquipWeapon(false);
        enemy.abilityCalled = false;
        //enemy.attackCount = 0;
    }

    public override void Update()
    {
        base.Update();


        if (enemy.ManualRotationActive())
        {
            enemy.transform.rotation = enemy.FaceTarget(enemy.player.transform.position);
            moveDirection = enemy.transform.position + (enemy.transform.forward * MAX_MOVEMENT_DISTANCE);
        }


        if (enemy.ManualMovementActive())
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, moveDirection, enemy.moveSpeed * Time.deltaTime);
        }

        if (!enemy.WeaponEquipped())
        {
            //return;
        }

        if (!enemy.ManualRotationActive() && !enemy.abilityCalled && enemy.ManualMovementActive())
        {
            Debug.Log("Activated");
            enemyAxe = enemy.GetComponentInChildren<EnemyAxe>(true);
            enemyAxe.gameObject.SetActive(true);
        }

        if (enemy.abilityCalled)
        {
            enemy.pulledWeapon.gameObject.SetActive(false);
            enemyAxe.TryGetComponent<Renderer>(out var renderer);
            renderer.enabled = true;

        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.recoveryState);

        }
    }
}

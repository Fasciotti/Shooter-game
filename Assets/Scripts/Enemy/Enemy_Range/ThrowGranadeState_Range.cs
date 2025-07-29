using UnityEngine;

public class ThrowGranadeState_Range : EnemyState
{
    private Enemy_Range enemy;

    public ThrowGranadeState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;

    }
    public override void Enter()
    {
        base.Enter();

        enemy.agent.isStopped = true;
        enemy.visuals.IKActive(false, false);

        if (enemy.weaponType == Enemy_RangeWeaponType.Pistol || enemy.weaponType == Enemy_RangeWeaponType.Revolver)
        {
            enemy.anim.SetBool("MirrorThrowGranade", true);
            return;
        }

        enemy.anim.SetBool("MirrorThrowGranade", false);
        enemy.visuals.WeaponModelActive(false);
        enemy.visuals.SecondaryWeaponModelActive(true);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.ManualRotationActive())
        {
            Vector3 playerPos = enemy.player.transform.position + Vector3.up;
            enemy.FaceTarget(playerPos);
            enemy.aim.position = playerPos;
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.BattleState);
        }
    }

    public override void AbilityTrigger()
    {
        enemy.ThrowGranade();
    }
}

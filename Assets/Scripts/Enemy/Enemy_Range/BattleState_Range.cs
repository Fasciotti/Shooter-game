using UnityEngine;

public class BattleState_Range : EnemyState
{
    private Enemy_Range enemy;

    private float lastTimeShot;
    private float bulletsShot;

    public BattleState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        enemy.agent.isStopped = true;
        enemy.visuals.IKActive(true);
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.visuals.IKActive(false);
    }

    public override void Update()
    {
        base.Update();
        enemy.FaceTarget(enemy.player.transform.position);

        if (WeaponOutOfBullets())
        {
            if (WeaponCooldown())
                ResetWeapon();

            return;
        }

        if (CanShoot())
        {
            Shoot();
        }

    }

    private void ResetWeapon()
    {
        bulletsShot = 0;
    }

    private bool WeaponCooldown() => Time.time > lastTimeShot + enemy.weaponCooldown; // This will return true when the weapon leaves the cooldown

    private bool WeaponOutOfBullets()
    {
        return bulletsShot >= enemy.bulletsToShoot;
    }

    private bool CanShoot()
    {
        return Time.time > lastTimeShot + (1f / enemy.fireRate);
    }

    private void Shoot()
    {
        lastTimeShot = Time.time;
        enemy.FireSingleBullet();

        bulletsShot++;
    }
}

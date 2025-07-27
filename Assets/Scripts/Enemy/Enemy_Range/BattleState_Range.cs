using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class BattleState_Range : EnemyState
{
    private Enemy_Range enemy;

    private float lastTimeShot;
    private float bulletsShot;
    private float weaponCooldown;
    private float bulletsPerAttack;

    private float timeInCover;

    public BattleState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.agent.isStopped = true;
        enemy.visuals.IKActive(true, true);

        ResetWeapon();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.visuals.IKActive(false, false);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsSeeingPlayer()) // This method also updates the enemy aim position
        {
            enemy.FaceTarget(enemy.aim.position);
        }

        if (!enemy.IsPlayerInAggressionRange())
        {
            stateMachine.ChangeState(enemy.AdvanceState);
        }

        
        ChangeCoverIfShould();



        if (WeaponOutOfBullets())
        {
            if (WeaponCooldown())
                ResetWeapon();

            return;
        }

        if (CanShoot() && enemy.AimOnPlayer())
        {
            Shoot();
        }

    }

    private void ChangeCoverIfShould()
    {
        if (enemy.coverPerk != CoverPerk.CanCoverAndChange)
            return;

        timeInCover -= Time.deltaTime;

        if (timeInCover < 0)
        {
            timeInCover = 1; // Minimum time in a cover point to it change

            if (InPlayerSight() || IsPlayerClose())
            {
                if (enemy.CanGetCover())
                    stateMachine.ChangeState(enemy.CoverState);
            }
        }
    }

    private void ResetWeapon()
    {
        bulletsShot = 0;
        weaponCooldown = enemy.weaponData.GetRandomWeaponCooldown();
        bulletsPerAttack = enemy.weaponData.GetRandomBulletsPerAttack();
    }

    private bool WeaponCooldown() => Time.time > lastTimeShot + weaponCooldown; // This will return true when the weapon leaves the cooldown

    private bool WeaponOutOfBullets()
    {
        return bulletsShot >= bulletsPerAttack;
    }

    private bool CanShoot()
    {
        return Time.time > lastTimeShot + (1f / enemy.weaponData.fireRate);
    }

    private void Shoot()
    {
        lastTimeShot = Time.time;
        enemy.FireSingleBullet();

        bulletsShot++;
    }

    private bool IsPlayerClose()
    {
        return Vector3.Distance(enemy.transform.position, enemy.player.transform.position) < enemy.safeDistance;
    }

    private bool InPlayerSight()
    {
        Vector3 yOffset = new Vector3(0, 0.1f, 0);
        Vector3 playerDirection = (enemy.player.transform.position - enemy.transform.position);

        if (Physics.Raycast(enemy.transform.position + yOffset, playerDirection + yOffset, out var hit))
        {
            Player player = hit.collider.GetComponentInParent<Player>();
            if (player != null)
            {
                return true;
            }
        }

        return false;
    }

}

using UnityEngine;

public class BattleState_Range : EnemyState
{
    private Enemy_Range enemy;

    private float lastTimeShot;
    private float bulletsShot;
    private float weaponCooldown;
    private float bulletsPerAttack;

    private float timeInCover;
    private bool firstTimeAttacking = true;

    public BattleState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.agent.isStopped = true;

        enemy.visuals.IKActive(true, true, 2f); // 2f is the perfect value to sync with the anim.

        stateTimer = enemy.attackDelay;

        enemy.visuals.WeaponModelActive(true);
        enemy.visuals.SecondaryWeaponModelActive(false);

        ResetWeapon();
    }

    public override void Exit()
    {
        base.Exit();

        enemy.anim.ResetTrigger("Shoot");
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsSeeingPlayer()) // This method also updates the enemy aim position
        {
            enemy.FaceTarget(enemy.aim.position);
        }

        if (enemy.CanThrowGranade())
        {
            stateMachine.ChangeState(enemy.ThrowGranadeState);
        }

        if (MustAdvancePlayer())
        {
            stateMachine.ChangeState(enemy.AdvanceState);
        }


        ChangeCoverIfShould();


        if (stateTimer > 0)
            return;


        if (WeaponOutOfBullets())
        {
            if (enemy.IsUnstoppable() && UnstoppableWalkReady())
            {
                stateMachine.ChangeState(enemy.AdvanceState);
            }

            if (WeaponCooldown())
                ResetWeapon();

            return;
        }

        if (CanShoot() && enemy.AimOnPlayer())
        {
            Shoot();
        }

    }

    private bool MustAdvancePlayer()
    {
        if (enemy.IsUnstoppable())
            return false; // We already handle the change when the weapon in out of bullets

        return !enemy.IsPlayerInAggressionRange() && ReadyToLeaveCover();
    }

    private bool UnstoppableWalkReady()
    {
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);
        bool close = distanceToPlayer < enemy.advanceStoppingDistance;

        bool cooldown = Time.time > enemy.AdvanceState.lastTimeAdvanced + enemy.weaponData.minWeaponCooldown;

        return !close && cooldown;
    }

    #region Weapon System
    private void ResetWeapon()
    {
        bulletsShot = 0;

        if (firstTimeAttacking)
        {
            firstTimeAttacking = false;

            weaponCooldown = enemy.weaponData.GetRandomWeaponCooldown();
            bulletsPerAttack = enemy.weaponData.GetRandomBulletsPerAttack();
            enemy.aim.position = enemy.playerBody.position;
        }
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

    #endregion

    #region Cover System
    private void ChangeCoverIfShould()
    {
        if (enemy.coverPerk != CoverPerk.CanCoverAndChange)
            return;

        timeInCover -= Time.deltaTime;

        if (timeInCover < 0)
        {
            timeInCover = 1; // Check cover status every X second

            if (ReadyToChangeCover() && ReadyToLeaveCover())
            {
                if (enemy.CanGetCover())
                    stateMachine.ChangeState(enemy.RunToCoverState);
            }
        }
    }

    private bool ReadyToLeaveCover()
    {
        return Time.time > enemy.RunToCoverState.lastCoverTime + enemy.minCoverDuration;
    }

    private bool ReadyToChangeCover()
    {
        bool inDanger = IsPlayerCloseToAim() || IsPlayerInClearSight();
        bool advancedOver = Time.time > enemy.AdvanceState.lastTimeAdvanced + enemy.minAdvanceDuration;

        return inDanger && advancedOver;
    }

    private bool IsPlayerCloseToAim()
    {
        return Vector3.Distance(enemy.transform.position, enemy.player.transform.position) < enemy.aimSafeDistance;
    }

    private bool IsPlayerInClearSight()
    {
        Vector3 yOffset = new Vector3(0, 0.1f, 0);
        Vector3 playerDirection = (enemy.player.transform.position - enemy.transform.position);

        if (Physics.Raycast(enemy.transform.position + yOffset, playerDirection + yOffset, out var hit))
        {
            return hit.transform.parent == enemy.player.transform;
        }

        return false;
    }

    #endregion
}

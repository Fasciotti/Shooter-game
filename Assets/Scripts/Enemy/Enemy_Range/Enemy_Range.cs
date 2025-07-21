using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Range : Enemy
{
    [Header("Weapon Settings")]
    public Enemy_RangeWeaponType weaponType;
    public Enemy_RangeWeaponData weaponData;

    [Space]
    
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gunPoint;

    [SerializeField] private List<Enemy_RangeWeaponData> availableWeaponPresets;

    public IdleState_Range IdleState { get; private set; }
    public MoveState_Range MoveState { get; private set; }
    public BattleState_Range BattleState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        IdleState = new IdleState_Range(this, stateMachine, "Idle");
        MoveState = new MoveState_Range(this, stateMachine, "Move");
        BattleState = new BattleState_Range(this, stateMachine, "Battle");
    }

    protected override void Start()
    {
        base.Start();
        
        stateMachine.Initialize(IdleState);
        
        visuals.SetupLook();
        visuals.IKActive(false);

        SetupWeaponData();
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }
    public override void EnterBattleMode()
    {
        if (inBattleMode)
            return;

        base.EnterBattleMode();

        stateMachine.ChangeState(BattleState);
    }

    public void FireSingleBullet()
    {
        anim.SetTrigger("Shoot");

        Vector3 bulletDirection = ((player.transform.position + Vector3.up) - gunPoint.position).normalized;

        GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);

        bullet.transform.position = gunPoint.position;
        bullet.transform.rotation = Quaternion.LookRotation(gunPoint.forward);

        Enemy_Bullet newBullet = bullet.GetComponent<Enemy_Bullet>();

        newBullet.BulletSetup();

        Rigidbody newBulletRb = newBullet.GetComponent<Rigidbody>();

        newBulletRb.mass = 20 / weaponData.bulletSpeed;
        newBulletRb.linearVelocity = bulletDirection * weaponData.bulletSpeed;

    }

    private void SetupWeaponData()
    {
        foreach (var data in availableWeaponPresets)
        {
            if (weaponType == data.weaponType)
            {
                weaponData = data;
            }
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
}

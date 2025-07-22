using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Range : Enemy
{
    [Header("Cover Settings")]
    public bool canUseCover = true;
    public float searchRadius;
    public Transform lastCover;
    public List<Cover> allCovers;

    [Header("Weapon Settings")]
    public Enemy_RangeWeaponType weaponType;
    public Enemy_RangeWeaponData weaponData;

    [Space]
    
    public List<Enemy_RangeWeaponData> availableWeaponPresets;
    public GameObject bulletPrefab;
    private Transform gunPoint;

    public IdleState_Range IdleState { get; private set; }
    public MoveState_Range MoveState { get; private set; }
    public BattleState_Range BattleState { get; private set; }
    public RunToCoverState_Range CoverState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        IdleState = new IdleState_Range(this, stateMachine, "Idle");
        MoveState = new MoveState_Range(this, stateMachine, "Move");
        BattleState = new BattleState_Range(this, stateMachine, "Battle");
        CoverState = new RunToCoverState_Range(this, stateMachine, "Run");
    }

    protected override void Start()
    {
        base.Start();
        
        stateMachine.Initialize(IdleState);
        
        visuals.SetupLook();
        visuals.IKActive(false, false);
        allCovers.AddRange(CollectNerbyCovers());

        SetupWeapon();
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

        if (canUseCover)
        {
            stateMachine.ChangeState(CoverState);
        }else
        {
            stateMachine.ChangeState(BattleState);
        }
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

        bulletDirection = weaponData.ApplyWeaponSpread(bulletDirection);

        newBulletRb.mass = 20 / weaponData.bulletSpeed;
        newBulletRb.linearVelocity = bulletDirection * weaponData.bulletSpeed;

    }

    public Transform AttemptToFindCover()
    {
        List<CoverPoint> coverPoints = new List<CoverPoint>();

        foreach (Cover cover in allCovers)
        {
            coverPoints.AddRange(cover.GetCoverPoints());
        }

        float shortestDistance = float.MaxValue;
        float currentDistance = 0;
        Transform closestCoverPoint = null;

        foreach (CoverPoint coverPoint in coverPoints)
        {
            currentDistance = Vector3.Distance(transform.position, coverPoint.transform.position);

            if (shortestDistance > currentDistance)
            {
                shortestDistance = currentDistance;
                closestCoverPoint = coverPoint.transform;
            }
        }

        if (closestCoverPoint != null)
        {
            lastCover = closestCoverPoint;
        }

        return lastCover.transform;

    }

    private List<Cover> CollectNerbyCovers()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius);
        List<Cover> covers = new List<Cover>();

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<Cover>(out var cover) && !allCovers.Contains(cover))
            {
                covers.Add(collider.GetComponent<Cover>());
            }
        }

        return covers;
    }

    private void SetupWeapon()
    {
        List<Enemy_RangeWeaponData> filteredData = new List<Enemy_RangeWeaponData>();

        foreach (var data in availableWeaponPresets)
        {
            if (weaponType == data.weaponType)
            {
                filteredData.Add(data);
            }
        }

        if (filteredData.Count > 0)
        {
            int randomIndex = Random.Range(0, filteredData.Count);
            weaponData = filteredData[randomIndex];
        }
        else
        {
            Debug.LogWarning("No Enemy Range Weapon Data was found.");
        }


        gunPoint = visuals.currentWeaponModel.GetComponent<Enemy_RangeWeaponModel>().gunPoint;

    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
}

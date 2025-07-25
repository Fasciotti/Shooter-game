using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum CoverPerk { NoCover, CanCoverOnce, CanCoverAndChange}

public class Enemy_Range : Enemy
{
    [Header("Advance settings")]
    public float advanceStateSpeed = 3;
    public float advanceStoppingDistance = 7;

    [Header("Enemy Perk")]
    public CoverPerk coverPerk;

    [Header("Cover Settings")]
    private readonly float searchRadius = 30;
    
    public float safeDistance = 2;
    public bool canUseCover = true;
    public CoverPoint lastCover { get; private set; }
    public CoverPoint currentCover { get; private set; }


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
    public AdvanceState_Range AdvanceState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        IdleState = new IdleState_Range(this, stateMachine, "Idle");
        MoveState = new MoveState_Range(this, stateMachine, "Move");
        BattleState = new BattleState_Range(this, stateMachine, "Battle");
        CoverState = new RunToCoverState_Range(this, stateMachine, "Run");
        AdvanceState = new AdvanceState_Range(this, stateMachine, "Advance");
    }

    protected override void Start()
    {
        base.Start();
        
        stateMachine.Initialize(IdleState);
        
        visuals.SetupLook();
        visuals.IKActive(false, false);

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

        if (CanGetCover())
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

    public bool CanGetCover()
    {
        if (coverPerk == CoverPerk.NoCover)
            return false;

        if (AttemptToFindCover() != null && lastCover != currentCover)
        {
            return true;
        }
        
        Debug.LogWarning("Cover Not Found: " + gameObject.name);
        return false;
    }

    private Transform AttemptToFindCover()
    {
        List<CoverPoint> coverPoints = new List<CoverPoint>();

        foreach (Cover cover in CollectNerbyCovers())
        {
            coverPoints.AddRange(cover.GetValidCoverPoints(transform));
        }

        float shortestDistance = float.MaxValue;
        float currentDistance = 0;
        CoverPoint closestCoverPoint = null;

        foreach (CoverPoint coverPoint in coverPoints)
        {
            currentDistance = Vector3.Distance(transform.position, coverPoint.transform.position);

            if (shortestDistance > currentDistance)
            {
                shortestDistance = currentDistance;
                closestCoverPoint = coverPoint;
            }
        }

        if (closestCoverPoint != null)
        {
            lastCover?.SetOccupied(false);
            lastCover = currentCover;

            currentCover = closestCoverPoint;
            currentCover.SetOccupied(true);
            
            return currentCover.transform;
        }

        return null;
    }

    private List<Cover> CollectNerbyCovers()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius);
        List<Cover> covers = new List<Cover>();

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<Cover>(out var cover) && !covers.Contains(cover))
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

}

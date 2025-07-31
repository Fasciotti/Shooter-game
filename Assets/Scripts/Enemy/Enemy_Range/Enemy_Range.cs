using NUnit.Framework;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum CoverPerk { Unavailable, CanCoverOnce, CanCoverAndChange}
public enum UnstoppablePerk { Unavailable, Unstoppable}
public enum ThrowGranadePerk { Unavailable, ThrowGranade};

public class Enemy_Range : Enemy
{
    [Header("Enemy Perk")]
    public CoverPerk coverPerk;
    public UnstoppablePerk unstoppablePerk;
    public ThrowGranadePerk throwGranadePerk;

    [Header("Throw Granade Settings")]
    [SerializeField] private GameObject granadePrefab;
    public Transform[] granadeStartPoint;
    public float granadeCooldown;
    public float granadeSafeDistance;
    public float explosionTimer;
    public float timeToReach = 2.5f;
    private float lastTimeGranadeThrown;
    [SerializeField] private float explosionForce;

    [Header("Advance Settings")]
    public float advanceStateSpeed = 3;
    public float advanceStoppingDistance = 7;
    public float minAdvanceDuration = 2.5f;

    [Header("Cover Settings")]
    public float minCoverDuration = 2.5f;
    public float aimSafeDistance = 2;
    public bool canUseCover = true;
    private readonly float searchRadius = 15;
    public CoverPoint lastCover { get; private set; }
    public CoverPoint currentCover { get; private set; }

    [Header("Weapon Settings")]
    public float attackDelay = 0.5f;
    public Enemy_RangeWeaponType weaponType;
    public Transform weaponHolder;
    public Enemy_RangeWeaponData weaponData;

    [Header("Aim Settings")]
    public Transform aim;
    public Transform playerBody;
    public LayerMask whatToIgnore;
    public float slowAim = 4;
    public float fastAim = 20;
    public float aimMaxDistance = 2;

    [Space]
    
    public List<Enemy_RangeWeaponData> availableWeaponPresets;
    public GameObject bulletPrefab;
    private Transform gunPoint;

    public IdleState_Range IdleState { get; private set; }
    public MoveState_Range MoveState { get; private set; }
    public BattleState_Range BattleState { get; private set; }
    public RunToCoverState_Range RunToCoverState { get; private set; }
    public AdvanceState_Range AdvanceState { get; private set; }
    public ThrowGranadeState_Range ThrowGranadeState { get; private set; }
    public DeadState_Range DeadState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        IdleState = new IdleState_Range(this, stateMachine, "Idle");
        MoveState = new MoveState_Range(this, stateMachine, "Move");
        BattleState = new BattleState_Range(this, stateMachine, "Battle");
        RunToCoverState = new RunToCoverState_Range(this, stateMachine, "Run");
        AdvanceState = new AdvanceState_Range(this, stateMachine, "Advance");
        ThrowGranadeState = new ThrowGranadeState_Range(this, stateMachine, "ThrowGranade");
        DeadState = new DeadState_Range(this, stateMachine, "Idle");


    }

    protected override void Start()
    {
        base.Start();

        playerBody = player.playerBody;
        aim.parent = null;
        
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
            stateMachine.ChangeState(RunToCoverState);
        }else
        {
            stateMachine.ChangeState(BattleState);
        }

    }

    public void FireSingleBullet()
    {
        anim.SetTrigger("Shoot");

        Vector3 bulletDirection = (aim.position - gunPoint.position).normalized;

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

    public override void GetHit()
    {
        base.GetHit();

        if (healthPoints <= 0 && stateMachine.currentState != DeadState)
        {
            stateMachine.ChangeState(DeadState);
        }
    }

    #region Cover System

    public bool CanGetCover()
    {
        if (coverPerk == CoverPerk.Unavailable)
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

    #endregion

    protected override void InitializePerk()
    {
        base.InitializePerk();

        if (unstoppablePerk == UnstoppablePerk.Unstoppable)
        {
            anim.SetFloat("AdvanceIndex", 1);
            visuals.corruptionAmount = 10; // High amount
            advanceStateSpeed = 1;

            if (coverPerk != CoverPerk.Unavailable)
                Debug.LogWarning("An unstoppable enemy should not be able to take cover.");
        }
    }

    public bool IsUnstoppable()
    {
        return unstoppablePerk == UnstoppablePerk.Unstoppable;
    }

    public bool CanThrowGranade()
    {
        if (throwGranadePerk != ThrowGranadePerk.ThrowGranade)
            return false;

        if (Vector3.Distance(transform.position, player.transform.position) < granadeSafeDistance)
            return false;

        if (Time.time < lastTimeGranadeThrown + granadeCooldown)
            return false;

        return true;
    }

    public Transform GranadeStartPoint()
    {
        if (weaponType == Enemy_RangeWeaponType.Pistol || weaponType == Enemy_RangeWeaponType.Revolver)
        {
            return granadeStartPoint[1];
        }

        return granadeStartPoint[0];
    }

    public void ThrowGranade()
    {
        lastTimeGranadeThrown = Time.time;

        visuals.GranadeModelActive(false);

        GameObject newGranade = ObjectPool.Instance.GetObject(granadePrefab, GranadeStartPoint().position);

        Enemy_Granade granadeScript = newGranade.GetComponent<Enemy_Granade>();

        if (stateMachine.currentState == DeadState)
        {
            granadeScript.SetupGranade(transform.position, 1, explosionTimer, explosionForce);
            return;
        }

        granadeScript.SetupGranade(player.transform.position, timeToReach, explosionTimer, explosionForce);

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

    #region Aim System

    public void UpdateAimPosition()
    {
        float speed = AimOnPlayer() ? fastAim : slowAim;
        aim.position = Vector3.MoveTowards(aim.position, playerBody.position, Time.deltaTime * speed);
    }

    public bool AimOnPlayer()
    {
        float distance = Vector3.Distance(player.transform.position, aim.position);

        return distance < aimMaxDistance;
    }
    
    public bool IsSeeingPlayer()
    {
        Vector3 myPosition = transform.position + Vector3.up;
        Vector3 directionToPlayer = playerBody.position - myPosition;

        if (Physics.Raycast(myPosition, directionToPlayer, out var hit, Mathf.Infinity, ~whatToIgnore))
        {
            if (hit.transform == player.transform)
            {
                UpdateAimPosition();
                return true;
            }
        }
        return false;
    }

    #endregion
}

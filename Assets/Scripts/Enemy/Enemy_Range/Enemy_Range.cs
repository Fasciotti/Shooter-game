using UnityEngine;

public class Enemy_Range : Enemy
{
    [SerializeField] private GameObject weaponHolder;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gunPoint;


    [Header("Shooting Settings")]
    public Enemy_RangeWeaponType weaponType;
    public float fireRate = 1; // Bullets per second
    public float bulletSpeed = 20;
    public float bulletsToShoot = 5; // How many bullets is going to be shoot before entering in cooldown
    public float weaponCooldown = 1.5f;


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

        newBulletRb.mass = 20 / bulletSpeed;
        newBulletRb.linearVelocity = bulletDirection * bulletSpeed;

    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
}

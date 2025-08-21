using UnityEngine;

public enum BossAttackType { Flamethrower, Hammer}
public class Enemy_Boss : Enemy
{

    [Header("Boss Settings")]
    public BossAttackType bossAttackType;
    public float attackRange;
    public float actionCooldown;
    public float speedUpCooldown = 15;
    public Transform impactPoint;


    [Header("Jump Attack Settings")]
    public LayerMask whatToIgnore;
    public float jumpTimeToTarget;
    public float jumpAttackCooldown;
    public float minJumpAttackDistance;
    private float lastJumpAttack;
    [Space]
    public float impactForce;
    public float impactRadius; // Used also in visuals
    private float upwardModifier = 7.5f;

    [Header("Ability settings")]
    public float abilityCooldown;
    public float abilityMaxDistance = 7;
    public float abilityDuration;
    private float lastTimeAbility;

    [Header("Flamethrower")]
    public ParticleSystem flameSteam;
    public bool flameThrowerActive { get; private set; }

    [Header("Hammer")]
    public GameObject hammerFxPrefab;

    [Header("Attack")]
    [SerializeField] private Transform[] damagePoints;
    [SerializeField] private float damageRadius;
    [SerializeField] private GameObject meleeImpactFx;


    public Enemy_BossVisuals bossVisuals { get; private set;}

    public MoveState_Boss MoveState { get; private set; }
    public IdleState_Boss IdleState { get; private set; }
    public AttackState_Boss AttackState { get; private set; }
    public JumpAttackState_Boss JumpAttackState { get; private set; }
    public AbilityState_Boss AbilityState { get; private set; }
    public DeadState_Boss DeadState { get; private set; }
    protected override void Awake()
    {
        base.Awake();

        bossVisuals = GetComponent<Enemy_BossVisuals>();

        IdleState = new IdleState_Boss(this, stateMachine, "Idle");
        MoveState = new MoveState_Boss(this, stateMachine, "Move");
        AttackState = new AttackState_Boss(this, stateMachine, "Attack");
        JumpAttackState = new JumpAttackState_Boss(this, stateMachine, "JumpAttack");
        AbilityState = new AbilityState_Boss(this, stateMachine, "Ability");
        DeadState = new DeadState_Boss(this, stateMachine, "Idle"); // Idle as placeholder
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.L))
        {
            stateMachine.ChangeState(DeadState);
        }

        stateMachine.currentState.Update();

        if (ShouldEnterBattleMode())
        {
            EnterBattleMode();
        }

        MeleeAttackCheck(damagePoints, damageRadius, meleeImpactFx);
    }

    public override void Die()
    {
        base.Die();

        if (stateMachine.currentState != DeadState)
            stateMachine.ChangeState(DeadState);
    }

    public override void EnterBattleMode()
    {
        if (inBattleMode)
            return;
        
        base.EnterBattleMode();

        // Go after the player. In the MoveState, when close enough, change to AttackState
        stateMachine.ChangeState(MoveState);
    }

    public bool IsPlayerInAttackRange() => Vector3.Distance(transform.position, player.transform.position) < attackRange;

    public bool CanDoJumpAttack()
    {
        if(Vector3.Distance(transform.position, player.transform.position) < minJumpAttackDistance)
        {
            return false;
        }

        if (IsPlayerInClearSight() && Time.time > lastJumpAttack + jumpAttackCooldown)
        {
            lastJumpAttack = Time.time + jumpTimeToTarget;
            return true;
        }

        return false;
    }
    private bool IsPlayerInClearSight()
    {
        Vector3 currentPos = transform.position;
        currentPos.y += 1.7f;

        Vector3 direction = (player.transform.position + Vector3.up) - currentPos;

        if(Physics.Raycast(currentPos, direction, out var hit, Mathf.Infinity, ~whatToIgnore))
        {
            if (hit.transform == player.transform || hit.transform.parent == player.transform)
            {
                return true;
            }
        }

        return false;
    }

    public void JumpImpact()
    {
        Transform impactPoint = this.impactPoint;

        if (impactPoint == null)
        {
            impactPoint = transform;
        }

        Collider[] colliders = Physics.OverlapSphere(impactPoint.position, impactRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<Rigidbody>(out var hit))
            {
                hit.AddExplosionForce(impactForce, impactPoint.position, impactRadius, upwardModifier, ForceMode.Impulse);
            }
        }
    }

    public bool CanUseAbility()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > abilityMaxDistance)
        {
            return false;
        }

        if (IsPlayerInClearSight() && Time.time > lastTimeAbility + abilityCooldown)
        {
            lastTimeAbility = Time.time + abilityDuration;
            return true;
        }

        return false;
    }

    public void HammerActive(bool activate)
    {
        GameObject hammerFx = ObjectPool.Instance.GetObject(hammerFxPrefab, impactPoint.position);
        ObjectPool.Instance.ReturnObject(hammerFx, 0.5f);
    }

    // Supposed to be in BossVisuals
    public void FlameThrowerActive(bool activate)
    {
        flameThrowerActive = activate;

        if (!activate)
        {
            anim.SetTrigger("StopFlamethrower");
            flameSteam.Stop();
            bossVisuals.ResetBatteries();
            return;
        }

        var mainModule = flameSteam.main;
        var childModule = flameSteam.transform.GetChild(0).GetComponent<ParticleSystem>().main;

        mainModule.duration = abilityDuration;
        childModule.duration = abilityDuration;

        bossVisuals.DischargeBatteries();
        flameSteam.Clear();
        flameSteam.Play();
    }

    protected override void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (player != null)
        {
            Vector3 currentPos = transform.position;
            currentPos.y += 1.7f;

            Vector3 playerPos = (player.transform.position + Vector3.up);

            Gizmos.DrawWireSphere(transform.position, minJumpAttackDistance);
            Gizmos.DrawLine(transform.position, playerPos);
        }

        foreach(Transform point in damagePoints)
        {
            Gizmos.DrawWireSphere(point.position, damageRadius);
        }
    }
}

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using Random = UnityEngine.Random;
using RangeAttribute = UnityEngine.RangeAttribute;

[System.Serializable]
public struct AttackData_Enemy_Melee
{
    public string attackName;
    public float attackRange;
    public float moveSpeed;
    public int attackIndex;
    public AttackType_Melee attackType;
    [Range(1,2)]
    public float animationSpeed;
}

public enum AttackType_Melee { CloseAttack, ChargeAttack}

public enum EnemyMelee_Type { Regular, Shield, Dodge, AxeThrow};

public class Enemy_Melee : Enemy
{
    public MoveState_Melee MoveState {  get; private set; }
    public IdleState_Melee IdleState {  get; private set; }
    public RecoveryState_Melee RecoveryState { get; private set; }
    public ChaseState_Melee ChaseState { get; private set; }
    public AttackState_Melee AttackState { get; private set; }
    public AbilityState_Melee AbilityState { get; private set; }
    public DeadState_Melee DeadState { get; private set; }

    [Header("Attack Data")]
    public AttackData_Enemy_Melee attackData;
    public List<AttackData_Enemy_Melee> attackList;

    [Header("Axe Throw Ability")]
    public GameObject axePrefab;
    public Transform axeStartPoint;
    public float axeFlySpeed;
    public float axeAimTimer;
    public float axeThrowCooldown;
    private float axeLastThrownTime;

    [Header("Dodge")]
    [SerializeField] private float dodgeCooldown = 5;
    private readonly float dodgeMinimumDistance = 3f;
    private float lastDodge = -10;

    [Header("Shield")]
    [SerializeField] private Transform shieldTransform;
    public int shieldDurability;

    [Header("Enemy Settings")]
    public EnemyMelee_Type meleeType;
    public Enemy_MeleeWeaponType weaponType;
    private Enemy_WeaponModel weaponModel;
    [SerializeField] private GameObject MeleeImpactFx;

    private readonly float moveSpeedMultiplierInAbility = 0.5f; // AxeThrow

    protected override void Awake()
    {
        base.Awake();

        IdleState = new IdleState_Melee(this, stateMachine, "Idle");
        MoveState = new MoveState_Melee(this, stateMachine, "Move");
        RecoveryState = new RecoveryState_Melee(this, stateMachine, "Recovery");
        ChaseState = new ChaseState_Melee(this, stateMachine, "Chase");
        AttackState = new AttackState_Melee(this, stateMachine, "Attack");
        AbilityState = new AbilityState_Melee(this, stateMachine, "AxeThrow"); //Null is used because the variable is defined inside the class.
        DeadState = new DeadState_Melee(this, stateMachine, "Idle"); //Idle anim is just a place holder. Ragdoll

        AttackState.UpdateAttackData();

    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(IdleState);
        visuals.SetupLook();
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        MeleeAttackCheck(weaponModel.damagePoints, weaponModel.damageRadius, MeleeImpactFx);
    }

    public override void EnterBattleMode()
    {
        if (inBattleMode)
            return;

        base.EnterBattleMode();

        stateMachine.ChangeState(RecoveryState);
    }
    public override void Die()
    {
        base.Die();

        if(stateMachine.currentState != DeadState)
            stateMachine.ChangeState(DeadState);
    }
    protected override void InitializePerk()
    {
        if (EnemyMelee_Type.AxeThrow == meleeType)
            weaponType = Enemy_MeleeWeaponType.Throw;

        if (EnemyMelee_Type.Dodge == meleeType)
            weaponType = Enemy_MeleeWeaponType.Unarmed;

        if (EnemyMelee_Type.Shield == meleeType)
        {
            anim.SetFloat("ChaseIndex", 1);
            shieldTransform.gameObject.SetActive(true);

            weaponType = Enemy_MeleeWeaponType.OneHand;
        }
    }

    // Called by EnemyAnimationEvents
    public override void AbilityTrigger()
    {
        base.AbilityTrigger();

        moveSpeed *= moveSpeedMultiplierInAbility;
        visuals.WeaponModelActive(false);
    }
    public void ActivateDodgeAnimation()
    {
        if (meleeType != EnemyMelee_Type.Dodge)
            return;

        if (stateMachine.currentState != ChaseState)
            return;

        if (Vector3.Distance(transform.position, player.transform.position) < dodgeMinimumDistance)
            return;

        // Maybe call it just once?
        if (Time.time > lastDodge + dodgeCooldown + GetAnimationDuration("Dodge"))
        {
            lastDodge = Time.time;
            anim.SetTrigger("Dodge");
        }
    }
    private float GetAnimationDuration(string clipName)
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in clips)
        {
            if (clip.name == clipName)
                return clip.length;
        }

        Debug.LogError("Animation not found. You might have changed the name of it." +
                       " Check the code that calls this method");
        return 0;

    }

    public void ThrowAxe()
    {
        GameObject axePrefabLocal = ObjectPool.Instance.GetObject(axePrefab, axeStartPoint.position);

        axePrefabLocal.GetComponent<Enemy_Axe>().SetupAxe(player.transform, axeFlySpeed, axeAimTimer);
    }
    public bool CanThrowAxe()
    {
        if (meleeType != EnemyMelee_Type.AxeThrow)
            return false;

        if (Vector3.Distance(transform.position, player.transform.position) < 1.5f)
            return false;

        if (Time.time > axeLastThrownTime + axeThrowCooldown)
        {
            axeLastThrownTime = Time.time;
            return true;

        }

        return false;
    }
    public void UpdateAttackData()
    {
        weaponModel = GetComponentInChildren<Enemy_WeaponModel>();

        if (weaponModel.weaponType == Enemy_MeleeWeaponType.Unarmed)
        {
            attackList = new List<AttackData_Enemy_Melee>(weaponModel.weaponData.attackData);
            rotationSpeed = weaponModel.weaponData.turnSpeed;
        }
    }
    public bool IsPlayerInAttackRange() => Vector3.Distance(transform.position, player.transform.position) < attackData.attackRange;
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // Attack Range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackData.attackRange);
    }
}

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using RangeAttribute = UnityEngine.RangeAttribute;

[System.Serializable]
public struct AttackData
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
    private Enemy_Visuals visuals;
    public MoveState_Melee moveState {  get; private set; }
    public IdleState_Melee idleState {  get; private set; }
    public RecoveryState_Melee recoveryState { get; private set; }
    public ChaseState_Melee chaseState { get; private set; }
    public AttackState_Melee attackState { get; private set; }
    public AbilityState_Melee abilityState { get; private set; }
    public DeadState_Melee deadState { get; private set; }

    [Header("AttackData")]
    public AttackData attackData;
    public List<AttackData> attackList;

    [Header("AxeThrow Throw Ability")]
    public GameObject axePrefab;
    public Transform axeStartPoint;
    public float axeFlySpeed;
    public float axeAimTimer;
    public float axeThrowCooldown;
    private float axeLastThrownTime;


    [Header("Enemy Settings")]
    public EnemyMelee_Type meleeType;
    [SerializeField] private Transform shieldTransform;
    [SerializeField] private float dodgeCooldown = 5;

    private readonly float dodgeMinimumDistance = 2.5f;
    private readonly float moveSpeedMultiplierInAbility = 0.5f;
    private float lastDodge = -10;

    protected override void Awake()
    {
        base.Awake();

        idleState = new IdleState_Melee(this, stateMachine, "Idle");
        moveState = new MoveState_Melee(this, stateMachine, "Move");
        recoveryState = new RecoveryState_Melee(this, stateMachine, "Recovery");
        chaseState = new ChaseState_Melee(this, stateMachine, "Chase");
        attackState = new AttackState_Melee(this, stateMachine, "Attack");
        abilityState = new AbilityState_Melee(this, stateMachine, null); //Null is used because the variable is defined inside the class.
        deadState = new DeadState_Melee(this, stateMachine, "Idle"); //Idle anim is just a place holder. Ragdoll

        attackState.UpdateAttackData();

    }

    protected override void Start()
    {
        base.Start();

        visuals = GetComponent<Enemy_Visuals>();
        stateMachine.Initialize(idleState);
        InitializeSpeciality();
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        if (ShouldEnterBattleMode())
        {
            EnterBattleMode();
        }
    }

    public override void EnterBattleMode()
    {
        if (inBattleMode)
            return;

        base.EnterBattleMode();

        stateMachine.ChangeState(recoveryState);
    }

    public override void GetHit()
    {
        base.GetHit();

        if (healthPoints <= 0)
            stateMachine.ChangeState(deadState);
    }

    protected void InitializeSpeciality()
    {
        if (EnemyMelee_Type.AxeThrow == meleeType)
            visuals.SetEnemyWeaponType(Enemy_MeleeWeaponType.Throw);

        if (EnemyMelee_Type.Shield == meleeType)
        {
            anim.SetFloat("ChaseIndex", 1);
            shieldTransform.gameObject.SetActive(true);

            visuals.SetEnemyWeaponType(Enemy_MeleeWeaponType.OneHand);

        }
    }

    public void WeaponModelActive(bool active)
    {
        visuals.CurrentWeaponModel().SetActive(active);
    }


    public bool IsPlayerInAttackRange() => Vector3.Distance(transform.position, player.transform.position) < attackData.attackRange;

    // Called by EnemyAnimationEvents
    public override void AbilityTrigger()
    {
        base.AbilityTrigger();

        moveSpeed *= moveSpeedMultiplierInAbility;
        WeaponModelActive(false);
    }

    public void ActivateDodgeAnimation()
    {
        if (meleeType != EnemyMelee_Type.Dodge)
            return;

        if (stateMachine.currentState != chaseState)
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

        Debug.Log(axeLastThrownTime + "///" + Time.time);

        return false;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // Attack Range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackData.attackRange);
    }
}

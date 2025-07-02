using NUnit.Framework;
using System.Collections.Generic;
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

public class Enemy_Melee : Enemy
{
    public MoveState_Melee moveState {  get; private set; }
    public IdleState_Melee idleState {  get; private set; }
    public RecoveryState_Melee recoveryState { get; private set; }
    public ChaseState_Melee chaseState { get; private set; }
    public AttackState_Melee attackState { get; private set; }
    public DeadState_Melee deadState { get; private set; }

    [Header("AttackData")]
    public AttackData attackData;
    public List<AttackData> attackList;

    [SerializeField] private Transform hiddenWeapon;
    [SerializeField] private Transform pulledWeapon;

    protected override void Awake()
    {
        base.Awake();

        idleState = new IdleState_Melee(this, stateMachine, "Idle");
        moveState = new MoveState_Melee(this, stateMachine, "Move");
        recoveryState = new RecoveryState_Melee(this, stateMachine, "Recovery");
        chaseState = new ChaseState_Melee(this, stateMachine, "Chase");
        attackState = new AttackState_Melee(this, stateMachine, "Attack");
        deadState = new DeadState_Melee(this, stateMachine, "Idle"); //Idle anim is just a place holder. Ragdoll

        attackState.UpdateAttackData();

    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }

    public override void GetHit()
    {
        base.GetHit();

        if (healthPoints <= 0)
            stateMachine.ChangeState(deadState);
    }

    public void PullWeapon()
    {
        hiddenWeapon.gameObject.SetActive(false);
        pulledWeapon.gameObject.SetActive(true);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // Attack Range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackData.attackRange);
    }

    public bool IsPlayerInAttackRange() => Vector3.Distance(transform.position, player.transform.position) < attackData.attackRange;

}

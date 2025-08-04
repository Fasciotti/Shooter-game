using UnityEngine;

public class Enemy_Boss : Enemy
{

    [Header("Attack Settings")]
    public float attackRange;
    [Header("Jump Attack Settings")]
    public LayerMask whatToIgnore;
    public float travelTimeToTarget;
    public float jumpAttackCooldown;
    public float minJumpAttackDistance;
    private float lastJumpAttack;

    public MoveState_Boss MoveState { get; private set; }
    public IdleState_Boss IdleState { get; private set; }
    public AttackState_Boss AttackState { get; private set; }
    public JumpAttackState_Boss JumpAttackState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        IdleState = new IdleState_Boss(this, stateMachine, "Idle");
        MoveState = new MoveState_Boss(this, stateMachine, "Move");
        AttackState = new AttackState_Boss(this, stateMachine, "Attack");
        JumpAttackState = new JumpAttackState_Boss(this, stateMachine, "JumpAttack");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        if (Input.GetKeyDown(KeyCode.V))
        {
            stateMachine.ChangeState(JumpAttackState);
        }

        if (ShouldEnterBattleMode())
        {
            EnterBattleMode();
        }
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
            lastJumpAttack = Time.time;
            return true;
        }

        return false;
    }

    public override void EnterBattleMode()
    {
        base.EnterBattleMode();

        // Go after the player. In the MoveState, when close enough, change to AttackState
        stateMachine.ChangeState(MoveState);
    }

    private bool IsPlayerInClearSight()
    {
        Vector3 currentPos = transform.position;
        currentPos.y += 1.7f;

        Vector3 direction = (player.transform.position + Vector3.up) - currentPos;

        if(Physics.Raycast(currentPos, direction, out var hit, Mathf.Infinity, ~whatToIgnore))
        {
            if(hit.transform == player.transform || player.transform.parent)
            {
                Debug.Log(hit.transform);
                return true;
            }
        }

        return false;
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
    }
}

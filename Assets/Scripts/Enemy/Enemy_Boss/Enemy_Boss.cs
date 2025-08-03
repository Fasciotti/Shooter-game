using UnityEngine;

public class Enemy_Boss : Enemy
{
    public MoveState_Boss MoveState { get; private set; }
    public IdleState_Boss IdleState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        IdleState = new IdleState_Boss(this, stateMachine, "Idle");
        MoveState = new MoveState_Boss(this, stateMachine, "Move");
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
    }
}

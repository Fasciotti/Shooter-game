using UnityEngine;

public class Enemy_Melee : Enemy
{
    public MoveState_Melee moveState {  get; private set; }
    public IdleState_Melee idleState {  get; private set; }

    protected override void Awake()
    {
        base.Awake();


        idleState = new IdleState_Melee(this, stateMachine, "Idle");
        moveState = new MoveState_Melee(this, stateMachine, "Move");

        
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
}

using UnityEngine;

public class ThrowGranadeState_Range : EnemyState
{
    private Enemy_Range enemy;

    public ThrowGranadeState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;

    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
    }
}

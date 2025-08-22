using UnityEngine;

public class Player_Hitbox : Hitbox
{
    private Player player;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponentInParent<Player>();
    }

    public override void TakeDamage(int damage)
    {
        player.health.ReduceHealth(damage);
    }
}

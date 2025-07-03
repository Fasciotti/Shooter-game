using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    [SerializeField] private int durability;

    private Enemy_Melee enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy_Melee>();
    }
    public void ReduceDurability()
    {
        durability--;

        if (durability < 1)
        {
            Destroy(gameObject);
            enemy.anim.SetFloat("ChaseIndex", 0);
        }
    }
}

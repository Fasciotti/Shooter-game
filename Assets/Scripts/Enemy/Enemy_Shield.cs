using UnityEngine;

public class Enemy_Shield : MonoBehaviour
{
    [SerializeField] private int durability;

    private Rigidbody rb;

    private Enemy_Melee enemy;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        enemy = GetComponentInParent<Enemy_Melee>();
    }
    public void ReduceDurability()
    {
        durability--;

        if (durability < 1)
        {
            // Uncomment and commment the rest if the shield must be destroyed, not dropped.
            //Destroy(gameObject);
            rb.isKinematic = false;
            gameObject.transform.SetParent(null, true);

            //Don't comment
            enemy.anim.SetFloat("ChaseIndex", 0);
        }
    }
}

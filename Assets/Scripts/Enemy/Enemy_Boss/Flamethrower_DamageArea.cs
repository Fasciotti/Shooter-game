using UnityEngine;

public class Flamethrower_DamageArea : MonoBehaviour
{
    private Enemy_Boss enemy;
    private float damageCooldown;
    private float lastTimeDamaged;
    [SerializeField] private Transform startPoint;
    [SerializeField] private LayerMask whatToIgnore;
    private void Awake()
    {
        enemy = GetComponentInParent<Enemy_Boss>();
        damageCooldown = enemy.flameDamageCooldown;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!enemy.flameThrowerActive)
            return;

        if (Time.time < lastTimeDamaged + damageCooldown)
            return;

        TryToDamage(other);
    }

    private void TryToDamage(Collider other)
    {
        Ray ray = new Ray(startPoint.position, other.transform.position - startPoint.position);

        if (Physics.Raycast(ray, out var hit, enemy.abilityMaxDistance, ~whatToIgnore))
        {
            if (hit.transform.root != other.transform.root)
                return;

            if(other.TryGetComponent<IDamageble>(out IDamageble hitbox))
            {
                hitbox.TakeDamage();
                lastTimeDamaged = Time.time;
            }
        }
    }
}

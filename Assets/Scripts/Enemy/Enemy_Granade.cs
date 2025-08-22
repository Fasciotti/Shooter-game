using System.Collections.Generic;
using UnityEngine;

public class Enemy_Granade : MonoBehaviour
{
    private Rigidbody rb;
    private bool canExplode = true;
    private float timer;
    private float explosionForce;

    [SerializeField] private float impactRadius;
    [SerializeField] private float upwardModifier;
    [SerializeField] private GameObject explosionFXPrefab;

    private LayerMask allyLayerMask;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetupGranade(LayerMask allyLayerMask, Vector3 target, float timeToReach, float timer, float explosionForce)
    {
        canExplode = true;
        rb.linearVelocity = CalculateLaunchDirection(target, timeToReach);
        this.timer = timer + timeToReach;
        this.explosionForce = explosionForce;
        this.allyLayerMask = allyLayerMask;
    }
    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0 && canExplode)
        {
            Explode();
        }
    }

    private void Explode()
    {
        canExplode = false;

        PlayExplosionFx();

        Collider[] colliders = Physics.OverlapSphere(transform.position, impactRadius);

        HashSet<Transform> uniqueEntities = new HashSet<Transform>();
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<IDamageble>(out IDamageble hitbox))
            {
                if (!IsTargetValid(collider))
                    continue;

                if (!uniqueEntities.Add(collider.transform.root))
                    continue;

                hitbox?.TakeDamage();
            }

            ApplyPhysicalForceTo(collider);
        }

        ObjectPool.Instance.ReturnObject(gameObject);
    }

    private void ApplyPhysicalForceTo(Collider collider)
    {
        if (collider.TryGetComponent<Rigidbody>(out var hit))
        {
            hit.AddExplosionForce(explosionForce, transform.position, impactRadius, upwardModifier, ForceMode.Impulse);
        }
    }

    private void PlayExplosionFx()
    {
        GameObject explosionFX = ObjectPool.Instance.GetObject(explosionFXPrefab, transform.position);

        // Return explosionFX after 0.75 seconds of the explosion
        ObjectPool.Instance.ReturnObject(explosionFX, 0.95f);
    }

    private bool IsTargetValid(Collider collider)
    {
        if (GameManager.Instance.FriendlyFire)
            return true;

        if ((allyLayerMask.value & (1 << collider.gameObject.layer)) > 0)
            return false;

        return true;
    }

    private Vector3 CalculateLaunchDirection(Vector3 target, float timeToReach)
    {
        Vector3 direction = target - transform.position;
        Vector3 directionY = new Vector3(0, direction.y, 0);
        Vector3 directionXZ = new Vector3(direction.x, 0, direction.z);

        Vector3 velocityXZ = directionXZ / timeToReach;
        Vector3 velocityY = (directionY - (0.5f * Mathf.Pow(timeToReach, 2) * Physics.gravity) / timeToReach);

        Vector3 velocityOfLaunch = velocityXZ + velocityY;

        return velocityOfLaunch;
    }
    private void OnDrawGizmos()
    {

        Gizmos.DrawWireSphere(transform.position, impactRadius);
    }
}

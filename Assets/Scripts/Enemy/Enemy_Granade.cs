using System;
using UnityEngine;

public class Enemy_Granade : MonoBehaviour
{
    private Rigidbody rb;
    private float timer;
    private float explosionForce;
    [SerializeField] private float impactRadius;
    [SerializeField] private float upwardModifier;
    [SerializeField] private GameObject explosionFXPrefab;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetupGranade(Vector3 target, float timeToReach, float timer, float explosionForce)
    {
        rb.linearVelocity = CalculateLaunchDirection(target, timeToReach);
        this.timer = timer + timeToReach;
        this.explosionForce = explosionForce;
    }
    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        GameObject explosionFX = ObjectPool.Instance.GetObject(explosionFXPrefab, transform.position);
        
        // Return explosionFX after 0.75 seconds of the explosion
        ObjectPool.Instance.ReturnObject(explosionFX, 0.95f);


        Collider[] colliders = Physics.OverlapSphere(transform.position, impactRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<Rigidbody>(out var hit))
            {
                hit.AddExplosionForce(explosionForce, transform.position, impactRadius, upwardModifier, ForceMode.Impulse);
            }
        }

        // Return granade instantly after explosion
        ObjectPool.Instance.ReturnObject(gameObject);
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

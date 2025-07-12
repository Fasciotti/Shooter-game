using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{

    [SerializeField] private GameObject bulletImpactFX;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    private BoxCollider boxCollider;
    private TrailRenderer trail;


    private float flyDistance;
    private float impactForce;
    private Vector3 startPosition;

    private bool returnCalled = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        trail = GetComponent<TrailRenderer>();
    }

    // It's called when the bullet is shot. (After the positioning)
    public void BulletSetup(float flyDistance, float impactForce)
    {
        startPosition = transform.position;

        // Both determined when shooting. Different weapons, different ranges.
        this.impactForce = impactForce;
        this.flyDistance = flyDistance;
        
        ResetBullet();
    }

    private void Update()
    {
        ReturnToPoolIfNotCollided();
    }

    private void ReturnToPoolIfNotCollided()
    {
        // If the bullet doesn't collide with anything, it's "destroyed" after some running distance.
        // startPosition is set as gunPoint, and flyDistance is set as weaponMaximumDistance.
        // Both determined when shooting (see FireSingleBullet() in PlayerWeaponController).
        if (Vector3.Distance(startPosition, transform.position) > flyDistance && gameObject.activeSelf)
        {
            ReturnBullet();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        CreateImpactFX(collision);
        ReturnBullet();

        Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();
        Enemy_Shield shield =  collision.gameObject.GetComponent<Enemy_Shield>();

        if (shield != null)
        {
            shield.ReduceDurability();
            return;
        }

        if (enemy != null)
        {
            Vector3 force = rb.linearVelocity.normalized * impactForce;
            Rigidbody targetRigidbody = collision.collider.attachedRigidbody;
            
            enemy.DeathImpact(force, collision.contacts[0].point, targetRigidbody);
            enemy.GetHit();
        }

            
    }

    private void ResetBullet()
    {
        meshRenderer.enabled = true;
        boxCollider.enabled = true;
        trail.emitting = true;
        returnCalled = false;
        trail.Clear();
    }

    private void ReturnBullet()
    {
        if (returnCalled)
            return;

        DisableBulletVisually();
        returnCalled = true;

        ObjectPool.Instance.ReturnObject(gameObject, trail.time);
    }

    private void DisableBulletVisually()
    {
        trail.emitting = false;
        meshRenderer.enabled = false;
        boxCollider.enabled = false;
    }

    private void CreateImpactFX(Collision collision)
    {
        if (collision.contactCount > 0)
        {
            ContactPoint contact = collision.GetContact(0);

            GameObject newImpactFX = ObjectPool.Instance.GetObject(bulletImpactFX);

            newImpactFX.transform.SetPositionAndRotation(contact.point, Quaternion.LookRotation(contact.normal));

            ObjectPool.Instance.ReturnObject(newImpactFX, 0.8f);
        }
    }
}

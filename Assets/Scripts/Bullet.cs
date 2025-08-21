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
    private LayerMask allyLayerMask;
    private bool hasAlreadyCollided;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        trail = GetComponent<TrailRenderer>();
    }

    // It's called when the bullet is shot. (After the positioning)
    public void BulletSetup(LayerMask allyLayerMask ,float flyDistance = 50, float impactForce = 100)
    {
        startPosition = transform.position;

        // Both determined when shooting. Different weapons, different ranges.
        this.impactForce = impactForce;
        this.flyDistance = flyDistance;

        this.allyLayerMask = allyLayerMask;
        hasAlreadyCollided = false;
        
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
            ReturnBulletToPool();
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {

        if (hasAlreadyCollided)
            return;

        hasAlreadyCollided = true;



        CreateImpactFX(collision);
        ReturnBulletToPool();

        if (!FriendlyFireEnabled())
        {
            // Bitwise operation
            if((allyLayerMask.value & (1 << collision.gameObject.layer)) > 0)
            {
                return;
            }
        }

        IDamageble hitbox = collision.gameObject.GetComponent<IDamageble>();
        hitbox?.TakeDamage();

        ApplyBulletImpactToEnemy(collision);

    }

    // At death
    private void ApplyBulletImpactToEnemy(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();
        if (enemy != null)
        {

            Vector3 force = rb.linearVelocity.normalized * impactForce;
            Rigidbody targetRigidbody = collision.collider.attachedRigidbody;

            enemy.BulletImpact(force, collision.contacts[0].point, targetRigidbody);
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

    protected void ReturnBulletToPool()
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

    protected void CreateImpactFX(Collision collision)
    {
        if (collision.contactCount > 0)
        {
            ContactPoint contact = collision.GetContact(0);

            GameObject newImpactFX = ObjectPool.Instance.GetObject(bulletImpactFX);

            newImpactFX.transform.SetPositionAndRotation(contact.point, Quaternion.LookRotation(contact.normal));

            ObjectPool.Instance.ReturnObject(newImpactFX, 0.8f);
        }
    }

    private bool FriendlyFireEnabled() => GameManager.Instance.FriendlyFire;
}

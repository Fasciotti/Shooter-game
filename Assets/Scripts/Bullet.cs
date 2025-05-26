using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb => GetComponent<Rigidbody>();

    [SerializeField] private TrailRenderer trail;
    [SerializeField] private GameObject bulletImpactFX;


    private float flyDistance;
    private Vector3 startPosition;


    // physicalBullet is the children of this.gameObject (bulletRoot), it's the object where the collider and the meshrender are stored.
    [SerializeField] private GameObject physicalBullet;

    // It's called when the bullet is shot. (After the positioning)
    public void BulletSetup(float flyDistance)
    {
        startPosition = transform.position;

        // Determined when shooting. Different weapons, different ranges.
        this.flyDistance = flyDistance;

        // It's used to prevent visual artefacts that occurs when moving the bullet to gunPoint.
        trail.Clear();
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
        if (Vector3.Distance(startPosition, transform.position) > flyDistance && physicalBullet.activeSelf)
        {
            ObjectPool.instance.ReturnBullet(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        CreateImpactFX(collision);

        ObjectPool.instance.ReturnBullet(gameObject);
    }

    public void EnableBullet()
    {
        physicalBullet.SetActive(true);

        // Activates trail.
        trail.emitting = true;
    }

    public void DisableBullet()
    {
        // Root stays active.
        physicalBullet.SetActive(false);

        // Disables trail smoothly.
        trail.emitting = false;

        // Disables rigidbody.
        rb.linearVelocity = Vector3.zero;
    }

    public GameObject PhysicalBullet() => physicalBullet;

    private void CreateImpactFX(Collision collision)
    {
        if (collision.contactCount > 0)
        {
            ContactPoint contact = collision.GetContact(0);

            GameObject newImpactFX = Instantiate(bulletImpactFX, contact.point, Quaternion.LookRotation(contact.normal));

            Destroy(newImpactFX, 0.9f);
        }
    }
}

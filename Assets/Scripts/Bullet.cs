using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private GameObject bulletImpactFX;
    private Rigidbody rb => GetComponent<Rigidbody>();
    private MeshRenderer meshRenderer => GetComponent<MeshRenderer>();
    private BoxCollider boxCollider => GetComponent<BoxCollider>();
    private TrailRenderer trail => GetComponent<TrailRenderer>();


    private float flyDistance;
    private Vector3 startPosition;

    private bool returnCalled = false;

    // It's called when the bullet is shot. (After the positioning)
    public void BulletSetup(float flyDistance)
    {
        startPosition = transform.position;

        // Determined when shooting. Different weapons, different ranges.
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

        ObjectPool.instance.ReturnObject(trail.time, gameObject);
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

            GameObject newImpactFX = ObjectPool.instance.GetObject(bulletImpactFX);
                // Instantiate(bulletImpactFX, contact.point, Quaternion.LookRotation(contact.normal));

            newImpactFX.transform.SetPositionAndRotation(contact.point, Quaternion.LookRotation(contact.normal));

            ObjectPool.instance.ReturnObject(0.8f, newImpactFX);
        }
    }
}

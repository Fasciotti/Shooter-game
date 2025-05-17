using Unity.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb => GetComponent<Rigidbody>();
    private TrailRenderer trail => GetComponentInChildren<TrailRenderer>();
    private bool doubleCollision;

    [SerializeField] private GameObject bulletImpactFX;

    private void OnCollisionEnter(Collision collision)
    {
        CreateImpactFX(collision);
        TrailHandler(collision);
        ObjectPool.instance.ReturnBullet(gameObject);

    }

    // FIXME: The trail is ejected out of the bullet gameObject, it doesn't return to the ObjectPool
    private void TrailHandler(Collision collision)
    {
        if (!doubleCollision)
            trail.transform.parent = null;
        
        doubleCollision = true;
    }

    private void CreateImpactFX(Collision collision)
    {
        if (collision.contactCount > 0)
        {
            ContactPoint contact = collision.GetContact(0);

            GameObject newImpactFX = Instantiate(bulletImpactFX, contact.point, Quaternion.LookRotation(contact.normal));

            Destroy(newImpactFX, 1f);
        }
    }
}

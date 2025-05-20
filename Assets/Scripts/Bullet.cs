using System;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb => GetComponent<Rigidbody>();
    private TrailRenderer trail => GetComponentInChildren<TrailRenderer>();
    private bool doubleCollision;

    private float flyDistance;
    private Vector3 startPosition;

    [SerializeField] private GameObject bulletImpactFX;

    public void BulletSetup(float flyDistance)
    {
        startPosition = transform.position;
        this.flyDistance = flyDistance + 1;
    }

    private void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance)
        {
            ObjectPool.instance.ReturnBullet(gameObject);

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        CreateImpactFX(collision);
        TrailHandler(collision);
        try
        {
            ObjectPool.instance.ReturnBullet(gameObject);

        }
        catch (NullReferenceException error)
        {
            Debug.LogError("Could not return object to the pool: " + error.Message);
        }
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

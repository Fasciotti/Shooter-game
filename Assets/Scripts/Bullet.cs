using System;
using System.Collections;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

// FIXME: Some bullets are ricocheting, i don't know why.
public class Bullet : MonoBehaviour
{
    private Rigidbody rb => GetComponent<Rigidbody>();
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private GameObject bulletImpactFX;


    private float flyDistance;
    private Vector3 startPosition;


    // physicalBullet is the children of this.gameObject (bulletRoot), it's the object where the collider and the meshrender are stored.
    [SerializeField] private GameObject physicalBullet;

    // It's called when the bullet is shot.
    public void BulletSetup(float flyDistance)
    {
        startPosition = transform.position;

        // Determined when shooting. Different weapons, different ranges.
        this.flyDistance = flyDistance;
    }

    private void Update()
    {
        ReturnToPoolIfNotCollided();
    }

    private void ReturnToPoolIfNotCollided()
    {
        // If the bullet doesn't collide with anything, it's "destroyed" after some running distance.
        // startPosition is set as gunPoint, and flyDistance is set as gunDistance.
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

    public void EnableTrail()
    {
        // TODO: Find a better way to do this. (The frame skipping seems obligatory, don't work without it)
        //IEnumerator WaitToActivateTrail()
        //{
            // Wait one frame after initiating the trail.
            // This prevents it of showing up when moving bullet in and out of the pool, and when repositioning it.
            //yield return new WaitForNextFrameUnit();
            trail.emitting = true;
            trail.Clear();
        //}

        //StartCoroutine(WaitToActivateTrail());
    }
    public void ClearTrail()
    {
        trail.Clear();
    }

    private void DisableTrail() => trail.emitting = false;

    public void EnableBullet()
    {
        physicalBullet.SetActive(true);
        EnableTrail();
    }

    public void DisableBullet()
    {
        // Root stays active.
        physicalBullet.SetActive(false);
        DisableTrail();
    }

    public GameObject PhysicalBullet() => physicalBullet;

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

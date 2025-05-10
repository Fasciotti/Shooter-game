using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb => GetComponent<Rigidbody>();
    private TrailRenderer trail => GetComponentInChildren<TrailRenderer>();

    [SerializeField] private GameObject bulletImpactFX;

    private void OnCollisionEnter(Collision collision)
    {
        CreateImpactFX(collision);
        trail.transform.parent = null;
        Destroy(gameObject);

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

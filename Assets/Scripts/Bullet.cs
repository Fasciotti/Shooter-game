using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb => GetComponent<Rigidbody>();

    private void OnCollisionEnter()
    {    
        // rb.constraints = RigidbodyConstraints.FreezeAll;
        Destroy(gameObject);
    }



}

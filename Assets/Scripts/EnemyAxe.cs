using UnityEngine;

[RequireComponent (typeof(Rigidbody), typeof(Collider))]
public class EnemyAxe : MonoBehaviour
{
    public Transform player;
    public Rigidbody rb;
    public Transform visualAxe;
    public float flySpeed;
    public float rotationSpeed;

    private Vector3 direction;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        visualAxe.Rotate(rotationSpeed * Time.deltaTime * Vector3.right);

        direction = (player.position + (Vector3.up)) - transform.position;
        rb.linearVelocity = direction.normalized * flySpeed;

        transform.forward = rb.linearVelocity;
        
    }
}

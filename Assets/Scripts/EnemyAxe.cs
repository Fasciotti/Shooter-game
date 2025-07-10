using UnityEngine;

[RequireComponent (typeof(Rigidbody), typeof(Collider))]
public class EnemyAxe : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform visualAxe;



    private Vector3 direction;
    private Transform player;

    private float flySpeed = 2;
    private float rotationSpeed = 1600;
    private float timer = 1;

    public void SetupAxe(Transform player, float flySpeed = 2, float timer = 1)
    {
        this.player = player;
        this.flySpeed = flySpeed;
        this.timer = timer;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        visualAxe.Rotate(rotationSpeed * Time.deltaTime * Vector3.right);

        timer -= Time.deltaTime;

        if (timer > 0)
        {
            direction = (player.position + (Vector3.up)) - transform.position;
        }

        rb.linearVelocity = direction.normalized * flySpeed;
        transform.forward = rb.linearVelocity;    
    }
}

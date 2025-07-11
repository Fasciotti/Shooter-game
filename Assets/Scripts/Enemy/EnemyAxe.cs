using UnityEngine;

[RequireComponent (typeof(Rigidbody), typeof(Collider))]
public class EnemyAxe : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform visualAxe;
    [SerializeField] private GameObject axeImpactFX;

    private Vector3 direction;
    private Transform player;

    private float flySpeed = 2;
    private float rotationSpeed = 1500;
    private float timer = 1;

    public void SetupAxe(Transform player, float flySpeed = 2, float timer = 1)
    {
        this.player = player;
        this.flySpeed = flySpeed;
        this.timer = timer;
    }

    private void Update()
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out _) || other.TryGetComponent<Bullet>(out _))
        {
            CreateImpactFX();

            rb.linearVelocity = Vector3.zero;
            ObjectPool.Instance.ReturnObject(gameObject);
        }
    }

    private void CreateImpactFX()
    {
        GameObject newImpactFX = ObjectPool.Instance.GetObject(axeImpactFX, transform.position);

        ObjectPool.Instance.ReturnObject(newImpactFX, 0.8f);
        
    }
}

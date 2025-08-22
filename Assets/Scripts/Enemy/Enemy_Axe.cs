using UnityEngine;

[RequireComponent (typeof(Rigidbody), typeof(Collider))]
public class Enemy_Axe : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform visualAxe;
    [SerializeField] private GameObject axeImpactFX;

    private Vector3 direction;
    private Transform player;

    private float flySpeed = 2;
    private float rotationSpeed = 1500;
    private float timer = 1;
    private bool hasAlreadyCollided;

    private int damage;


    public void SetupAxe(Transform player, int damage, float flySpeed = 2, float axeAimTimer = 1)
    {
        this.player = player;
        this.flySpeed = flySpeed;
        this.timer = axeAimTimer;
        this.damage = damage;
        hasAlreadyCollided = false;
    }

    private void Update()
    {
        visualAxe.Rotate(rotationSpeed * Time.deltaTime * Vector3.right);

        timer -= Time.deltaTime;

        if (timer > 0)
        {
            direction = (player.position + (Vector3.up)) - transform.position;
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = direction.normalized * flySpeed;
        transform.forward = rb.linearVelocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasAlreadyCollided)
            return;

        if (collision.gameObject.TryGetComponent<IDamageble>(out IDamageble hitbox))
            hitbox?.TakeDamage(damage);

        CreateImpactFX();
        rb.linearVelocity = Vector3.zero;
        hasAlreadyCollided = true;
        ObjectPool.Instance.ReturnObject(gameObject);
    }

    private void CreateImpactFX()
    {
        GameObject newImpactFX = ObjectPool.Instance.GetObject(axeImpactFX, transform.position);

        ObjectPool.Instance.ReturnObject(newImpactFX, 0.8f);
        
    }
}

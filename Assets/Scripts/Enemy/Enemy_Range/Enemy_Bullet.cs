using UnityEngine;

public class Enemy_Bullet : Bullet
{
    protected override void OnCollisionEnter(Collision collision)
    {
        Player player = collision.gameObject.GetComponentInParent<Player>();

        CreateImpactFX(collision);
        ReturnBulletToPool();

        if (player != null)
        {
            Debug.Log("Shot the player");
        }
    }
}

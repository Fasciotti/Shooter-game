using TMPro;
using UnityEngine;
using static UnityEditor.IMGUI.Controls.PrimitiveBoundsHandle;

public class EnemyAxe : MonoBehaviour
{

    private Enemy_Melee enemy;

    private Vector3 moveDirection;
    private Quaternion weaponRotation;
    private Quaternion targetRotation;

    private Transform parent;

    private float rotationTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy = GetComponentInParent<Enemy_Melee>();

    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotationTimer += Time.deltaTime;

        if (!enemy.pulledWeapon.gameObject.activeSelf)
        {
            transform.parent.parent = null;
            transform.Rotate(new Vector3(1, 0, 0), 600 * Time.deltaTime);
            transform.parent.LookAt(enemy.player.transform.position);
            transform.parent.position = Vector3.MoveTowards(transform.position, enemy.player.transform.position, 1f * Time.deltaTime);
            return;
        }

        weaponRotation = Quaternion.LookRotation(enemy.pulledWeapon.transform.forward, enemy.pulledWeapon.transform.up);
        targetRotation = Quaternion.LookRotation(transform.forward, transform.up);

        enemy.pulledWeapon.rotation = Quaternion.Slerp(weaponRotation, targetRotation, Time.deltaTime * 15);
        enemy.pulledWeapon.position = Vector3.Slerp(enemy.pulledWeapon.position, transform.position + new Vector3(0, 0.275f, -0.2f), Time.deltaTime * 13);
    }
}

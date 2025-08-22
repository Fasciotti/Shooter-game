using System;
using UnityEngine;

public class Dummy : MonoBehaviour, IDamageble
{
    public int currentHealth;
    public int maxHealth;

    public MeshRenderer mesh;
    public Material defaultMat;
    public Material deadMat;
    public Material damageMat;

    public float refreshCooldown;
    public float damageCooldown;
    private float lastTimeShot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Refresh();
    }

    private void Refresh()
    {
        currentHealth = maxHealth;
        mesh.sharedMaterial = defaultMat;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > lastTimeShot + refreshCooldown)
        {
            Refresh();
        }

        if (Time.time > lastTimeShot + damageCooldown && currentHealth > 0)
        {
            mesh.sharedMaterial = defaultMat;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        lastTimeShot = Time.time;

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        mesh.sharedMaterial = damageMat;

    }

    private void Die()
    {
        mesh.sharedMaterial = deadMat;
    }
}

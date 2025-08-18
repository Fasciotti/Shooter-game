using System;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_BossVisuals : MonoBehaviour
{
    private Enemy_Boss enemy;

    public float landingOffset;
    [SerializeField] private ParticleSystem landingZone;
    [SerializeField] private TrailRenderer[] weaponTrails;

    // 0 = Right, 1 = Left;
    [Header("Batteries")]
    [SerializeField] private GameObject[] batteries;

    private const float initialBatteryScaleY = 0.15f; // Max battery scale
    private const float minBatteryScaleY = 0f;
    [SerializeField] private float rechargeSpeed;

    [SerializeField] private float dischargeSpeed;
    private bool isRecharging;


    private void Awake()
    {
        enemy = GetComponent<Enemy_Boss>();
    }

    private void Start()
    {
        ResetBatteries();
        landingZone.transform.parent = null;
    }
    private void Update()
    {
        UpdateBatteryVisuals();

    }
    public void ResetBatteries()
    {
        isRecharging = true;
        rechargeSpeed = initialBatteryScaleY / enemy.abilityCooldown;
        dischargeSpeed = initialBatteryScaleY / (enemy.abilityDuration * 0.75f);

        foreach (var battery in batteries)
        {
            battery.SetActive(true);
        }

    }
    public void PlaceLandingZoneEffect(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        Vector3 offset = dir.normalized * landingOffset;

        landingZone.transform.position = target + offset;
        landingZone.Clear();

        var mainModule = landingZone.main;

        mainModule.startLifetime = enemy.jumpTimeToTarget * 2f; // Magic number to give some margin of time.
        mainModule.startSize = enemy.impactRadius * 2;

        landingZone.Play();

    }

    public void WeaponTrailActive(bool active)
    {
        foreach(var trail in weaponTrails)
        {
            trail.gameObject.SetActive(active);
        }
    }

    public void DischargeBatteries() => isRecharging = false;


    private void UpdateBatteryVisuals()
    {
        float speed = isRecharging ? rechargeSpeed : -dischargeSpeed;

        foreach (var battery in batteries)
        {

            float adder = speed * Time.deltaTime;

            float Yscale = Mathf.Clamp(battery.transform.localScale.y + adder, minBatteryScaleY, initialBatteryScaleY);

            Vector3 newScale = new Vector3(
                battery.transform.localScale.x, Yscale, battery.transform.localScale.z);

            battery.transform.localScale = newScale;
            if (battery.transform.localScale.y <= 0)
            {
                battery.SetActive(false);
            }
        }
    }
}

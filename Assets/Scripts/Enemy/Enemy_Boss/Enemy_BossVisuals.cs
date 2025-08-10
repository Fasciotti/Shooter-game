using System;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_BossVisuals : MonoBehaviour
{
    private Enemy_Boss enemy;

    // 0 = Right, 1 = Left;
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
    }
    public void ResetBatteries()
    {
        isRecharging = true;
        rechargeSpeed = initialBatteryScaleY / enemy.abilityCooldown;
        dischargeSpeed = initialBatteryScaleY / (enemy.flameThrowerDuration * 0.75f);

        foreach (var battery in batteries)
        {
            Debug.Log("tring");

            battery.SetActive(true);
        }

    }

    public void DischargeBatteries() => isRecharging = false;

    private void Update()
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

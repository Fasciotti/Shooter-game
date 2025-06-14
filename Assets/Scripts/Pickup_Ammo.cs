using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using RangeAttribute = UnityEngine.RangeAttribute;
public enum AmmoBoxType { smallBox, bigBox}

public class Pickup_Ammo : Interactable
{
    private Player player;

    [SerializeField] private AmmoBoxType ammoBoxType;

    [System.Serializable]
    public struct AmmoData
    {
        public WeaponType weaponType;
        [Range(10,100)] public int maxAmount;
        [Range(10,100)]public int minAmount;
    }

    [SerializeField] private List<AmmoData> smallBoxAmmo;
    [SerializeField] private List<AmmoData> bigBoxAmmo;


    [SerializeField] private GameObject[] boxModels;

    private void Start()
    {
        SetupBoxModel();
    }

    private void SetupBoxModel()
    {
        for (int i = 0; i < boxModels.Length; ++i)
        {
            boxModels[i].SetActive(false);

            if (i == (int)(ammoBoxType))
            {
                boxModels[i].SetActive(true);

                UpdateMeshAndMaterial(boxModels[i].GetComponent<MeshRenderer>());
            }
        }
    }

    public override void Interaction()
    {
        List<AmmoData> currentAmmoList = smallBoxAmmo;

        if (ammoBoxType == AmmoBoxType.bigBox)
            currentAmmoList = bigBoxAmmo;



        foreach (AmmoData ammo in currentAmmoList)
        {
            Weapon weapon = player.weapon.WeaponInSlots(ammo.weaponType);
            AddBulletsToWeapon(weapon, GetBulletAmount(ammo));
        }

        ObjectPool.instance.ReturnObject(gameObject);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (player == null)
            player = other.GetComponent<Player>();
        
    }

    private int GetBulletAmount(AmmoData ammo)
    {
        float max = Mathf.Max(ammo.maxAmount, ammo.minAmount);
        float min = Mathf.Min(ammo.minAmount, ammo.maxAmount);


        float bulletAmount = Random.Range(min, max);

        return Mathf.RoundToInt(bulletAmount);
    }

    private void AddBulletsToWeapon(Weapon weapon, int amount)
    {
        if (weapon == null)
            return;

        weapon.totalReserveAmmo += amount;
    }

}

using UnityEngine;

public class Pickup_Weapon : Interactable
{
    [SerializeField] private Weapon_Data weaponData;
    [SerializeField] private BackupWeaponModel[] backupWeaponModels;

    [SerializeField] private Weapon weapon;

    private bool oldWeapon;

    private void Start()
    {
        if (!oldWeapon)
        {
            weapon = new Weapon(weaponData);
        }

        SetupGameObject();
    }

    public override void Interaction()
    {
        player.weapon.PickUpWeapon(weapon);

        ObjectPool.instance.ReturnObject(gameObject);
    }

    [ContextMenu("Update Item Model")]
    public void SetupGameObject()
    {
        gameObject.name = "Pickup Weapon: " + weaponData.weaponType;
        SetupWeaponModel();
    }

    public void SetupPickupWeapon(Weapon weapon, Transform transform)
    {
        oldWeapon = true;

        weaponData = weapon.weaponData;
        this.weapon = weapon;

        this.transform.position = transform.position + new Vector3(0, 0.75f, 0); // .75 is just an offset value to maintain the weapon floating.

        SetupGameObject();
    }

    private void SetupWeaponModel()
    {
        foreach (BackupWeaponModel model in backupWeaponModels)
        {
            model.gameObject.SetActive(false);

            if (model.weaponType == weaponData.weaponType)
            {
                model.gameObject.SetActive(true);
                UpdateMeshAndMaterial(model.gameObject.GetComponent<MeshRenderer>());
            }
        }
    }
}

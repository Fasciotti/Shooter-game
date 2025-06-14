using UnityEngine;

public class Pickup_Weapon : Interactable
{
    private Player player;

    [SerializeField] private Weapon_Data weaponData;
    [SerializeField] private BackupWeaponModel[] backupWeaponModels;

    [SerializeField] private Weapon weapon;

    private bool oldWeapon;

    private void Start()
    {
        UpdateGameObject();

        if (!oldWeapon)
        {
            weapon = new Weapon(weaponData);
        }
    }

    public override void Interaction()
    {
        player.weapon.PickUpWeapon(weapon);

        ObjectPool.instance.ReturnObject(gameObject);
    }

    [ContextMenu("Update Item Model")]
    public void UpdateGameObject()
    {
        gameObject.name = "Pickup Weapon: " + weaponData.weaponType;
        UpdateItemModel();
    }

    public void SetupPickupWeapon(Weapon weapon, Transform transform)
    {
        oldWeapon = true;

        weaponData = weapon.weaponData;
        this.weapon = weapon;

        this.transform.position = transform.position + new Vector3(0, 0.75f, 0); // .75 is just an offset value to maintain the weapon floating.

        UpdateGameObject();
    }

    public void UpdateItemModel()
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

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (player == null)
        {
            player = other.GetComponent<Player>();
        }
    }
}

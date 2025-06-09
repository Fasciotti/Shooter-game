using UnityEngine;

public class Pickup_Weapon : Interactable
{
    private Player player;

    [SerializeField] private Weapon_Data weaponData;
    [SerializeField] private BackupWeaponModel[] backupWeaponModels;


    private void Start()
    {
        UpdateGameObject();
    }

    public override void Interaction()
    {
        base.Interaction();

        player.weapon.PickUpWeapon(weaponData);
    }

    [ContextMenu("Update Item Model")]
    public void UpdateGameObject()
    {
        gameObject.name = "Pickup Weapon: " + weaponData.weaponType;
        UpdateItemModel();
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

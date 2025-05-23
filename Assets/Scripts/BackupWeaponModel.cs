using UnityEngine;

public enum HangType { LowBackHang, BackHang, SideHang }

public class BackupWeaponModel : MonoBehaviour
{
    public WeaponType weaponType;

    [SerializeField] private HangType hangType;


    public void ActivateModel(bool active) => gameObject.SetActive(active);

    public bool HangTypeEquals(HangType hangType) => this.hangType == hangType;
}

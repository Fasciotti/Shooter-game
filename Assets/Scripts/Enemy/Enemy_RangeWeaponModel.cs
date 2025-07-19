using UnityEngine;

public enum Enemy_RangeWeaponHoldType {Common, LowHold, HighHold }
public class Enemy_RangeWeaponModel : MonoBehaviour
{
    public Enemy_RangeWeaponType weaponType;
    public Enemy_RangeWeaponHoldType weaponHoldType;
}

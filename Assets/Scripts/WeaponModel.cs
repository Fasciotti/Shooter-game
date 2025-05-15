using UnityEngine;

public enum EquipType
{ sideEquip, backEquip };
public enum HoldType 
{ CommonHold = 1, LowHold, HighHold}

public class WeaponModel : MonoBehaviour
{
    public WeaponType weaponType;
    public HoldType holdType;
    public EquipType equipType;

    public Transform gunPoint;
    public Transform holdPoint;


}

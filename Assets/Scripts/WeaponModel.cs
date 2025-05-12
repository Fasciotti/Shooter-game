using UnityEngine;

public enum GrabType
{ sideGrab, backGrab };
public enum HoldType 
{ CommonHold = 1, LowHold, HighHold}

public class WeaponModel : MonoBehaviour
{
    public WeaponType weaponType;
    public HoldType holdType;
    public GrabType grabType;

    public Transform gunPoint;
    public Transform holdPoint;


}

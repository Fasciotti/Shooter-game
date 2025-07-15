using UnityEngine;


public enum Enemy_MeleeWeaponType {Throw, OneHand, Unarmed}
public class Enemy_WeaponModel : MonoBehaviour
{
    public Enemy_MeleeWeaponType weaponType;
    public AnimatorOverrideController animatorOverride;
}

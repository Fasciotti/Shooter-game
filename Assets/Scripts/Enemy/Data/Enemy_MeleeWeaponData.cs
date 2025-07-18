using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Weapon Data", menuName ="Enemy Melee/Weapon Data")]
public class Enemy_MeleeWeaponData : ScriptableObject
{
    public List<AttackData_Enemy_Melee> attackData;
    public float turnSpeed = 10;
}

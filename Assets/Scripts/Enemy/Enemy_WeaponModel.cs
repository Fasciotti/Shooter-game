using UnityEngine;
using UnityEngine.InputSystem.LowLevel;


public class Enemy_WeaponModel : MonoBehaviour
{
    public Enemy_MeleeWeaponType weaponType;
    public AnimatorOverrideController animatorOverride;
    public Enemy_MeleeWeaponData weaponData;


    [SerializeField] GameObject[] effects;

    private void Awake()
    {
        TrailEffectActive(false);
    }

    public void TrailEffectActive(bool active)
    {
        foreach(var effect in effects)
        {
            effect.SetActive(active);
        }

    }
}

using UnityEngine;
using UnityEngine.InputSystem.LowLevel;


public class Enemy_WeaponModel : MonoBehaviour
{
    public Enemy_MeleeWeaponType weaponType;
    public AnimatorOverrideController animatorOverride;
    public Enemy_MeleeWeaponData weaponData;


    [SerializeField] private GameObject[] trailPoints;

    [Header("Damage Attributes")]
    public Transform[] damagePoints; // Hitbox

    public float damageRadius;

    private void Awake()
    {
        TrailEffectActive(false);
    }

    [ContextMenu("Assing Damage Points")]
    private void AssignDamagePoint()
    {
        damagePoints = new Transform[trailPoints.Length];

        // TrailPoints already are in the place where the hitboxes would be
        for (int i = 0; i < trailPoints.Length; i++)
        {
            damagePoints[i] = trailPoints[i].transform;
        }
    }

    public void TrailEffectActive(bool active)
    {
        foreach(var effect in trailPoints)
        {
            effect.SetActive(active);
        }

    }

    private void OnDrawGizmos()
    {
        foreach (Transform t in damagePoints)
        {
            Gizmos.DrawWireSphere(t.position, damageRadius);
        }
    }
}

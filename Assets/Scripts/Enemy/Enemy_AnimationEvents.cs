using UnityEngine;

public class Enemy_AnimationEvents : MonoBehaviour
{
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();        
    }

    public void AnimationTrigger() => enemy.AnimationTrigger();

    // Movement
    public void StartManualMovement() => enemy.SetActiveManualMovement(true);
    public void StopManualMovement() => enemy.SetActiveManualMovement(false);

    // Rotation
    public void StartManualRotation() => enemy.SetActiveManualRotation(true);
    public void StopManualRotation() => enemy.SetActiveManualRotation(false);

    public void AbilityEvent() => enemy.AbilityTrigger();

    public void EnableIK() => enemy.visuals.IKActive(true, true, 1.75f); // perfect value to sync with anim

    public void EnableWeaponModel()
    {
        enemy.visuals.SecondaryWeaponModelActive(false);
        enemy.visuals.WeaponModelActive(true);
    }
}

using UnityEngine;

public class Enemy_AnimationEvents : MonoBehaviour
{
    private Enemy enemy;
    private Enemy_Boss enemyBoss;
    private Enemy_Melee enemyMelee;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        enemyBoss = GetComponentInParent<Enemy_Boss>();
        enemyMelee = GetComponentInParent<Enemy_Melee>();
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

    public void JumpImpactAnim()
    {
        enemyBoss?.JumpImpact();
    }
    public void EnableAttackCheck() => enemy?.AttackCheckActive(true);

    public void DisableAttackCheck() => enemy?.AttackCheckActive(false);
}

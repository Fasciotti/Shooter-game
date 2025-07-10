using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
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
}

using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();        
    }

    public void AnimationTrigger() => enemy.AnimationTrigger();
}

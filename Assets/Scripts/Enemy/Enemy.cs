using UnityEngine;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour
{
    [Header("Idle Info")]
    public float idleTime;
    public EnemyStateMachine stateMachine { get; private set; }


    protected virtual void Awake()
    {
        stateMachine = new EnemyStateMachine();

    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        
    }
}

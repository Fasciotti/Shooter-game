using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour
{
    [Header("Idle Configuration")]
    public float idleTime;

    [Header("Move Configuration")]
    [SerializeField] private float moveSpeed;
    public Transform[] patrolPoints;
    public int currentPatrolIndex;


    public NavMeshAgent agent { get; private set; }
    public EnemyStateMachine stateMachine { get; private set; }
    public Animator anim {  get; private set; }


    protected virtual void Awake()
    {
        stateMachine = new EnemyStateMachine();

        agent = GetComponent<NavMeshAgent>();

        anim = GetComponentInChildren<Animator>();
    }

    protected virtual void Start()
    {
        InitializePatrolPoints();
    }

    private void InitializePatrolPoints()
    {
        foreach (Transform t in patrolPoints)
            t.parent = null;
    }

    protected virtual void Update()
    {
        
    }

    public Vector3 GetPatronDestination()
    {
        Vector3 destination = patrolPoints[currentPatrolIndex].transform.position;

        currentPatrolIndex++;

        if (currentPatrolIndex >=  patrolPoints.Length)
            currentPatrolIndex = 0;

        return destination;
    }
}

using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour
{


    [Header("Attack Configuration")]
    public float aggressionRange;
    public float attackRange;
    public float attackMoveSpeed;

    [Header("Idle Configuration")]
    public float idleTime;

    [Header("Move Configuration")]
    public float moveSpeed;
    public float chaseSpeed;
    public float rotationSpeed;
    public bool manualMovement;


    //[System.NonSerialized] public float chaseAcceleration = 15;
    //[System.NonSerialized] public float standardAcceleration = 8;


    [Space]

    [SerializeField]private int currentPatrolIndex;
    public Transform[] patrolPoints;


    public Player player { get; private set; }
    public NavMeshAgent agent { get; private set; }
    public EnemyStateMachine stateMachine { get; private set; }
    public Animator anim {  get; private set; }


    protected virtual void Awake()
    {
        stateMachine = new EnemyStateMachine();

        agent = GetComponent<NavMeshAgent>();

        anim = GetComponentInChildren<Animator>();

        player = GameObject.Find("Player").GetComponent<Player>();
    }

    protected virtual void Start()
    {
        InitializePatrolPoints();
    }
    protected virtual void Update()
    {

    }

    private void InitializePatrolPoints()
    {
        foreach (Transform t in patrolPoints)
            t.parent = null;
    }

    private void OnDrawGizmos()
    {
        // Aggression Range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggressionRange);

        // Attack Range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void SetActiveManualMovement(bool manualMovement) => this.manualMovement = manualMovement;
    public bool ManualMovementActive() => manualMovement;
    public void AnimationTrigger() => stateMachine.currentState.AnimationTrigger();
    public bool IsPlayerInAggressionRange() => Vector3.Distance(transform.position, player.transform.position) < aggressionRange;
    public bool IsPlayerInAttackRange() => Vector3.Distance(transform.position, player.transform.position) < attackRange;

    public Vector3 GetPatronDestination()
    {
        Vector3 destination = patrolPoints[currentPatrolIndex].transform.position;

        currentPatrolIndex++;

        if (currentPatrolIndex >=  patrolPoints.Length)
            currentPatrolIndex = 0;

        return destination;
    }

    public Quaternion FaceTarget(Vector3 target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);

        Vector3 currentRotation = transform.rotation.eulerAngles;

        float yRotation = Mathf.LerpAngle(currentRotation.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);

        return Quaternion.Euler(currentRotation.x, yRotation, currentRotation.z);
    }
}

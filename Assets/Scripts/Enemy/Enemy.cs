using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using System.Collections;

public class Enemy : MonoBehaviour
{

    [Header("Health")]
    [SerializeField] protected int healthPoints = 20;

    [Header("Idle Configuration")]
    public float idleTime;
    public float aggressionRange;

    [Header("Move Configuration")]
    public float moveSpeed;
    public float chaseSpeed;
    public float rotationSpeed;
    private bool manualMovement;
    private bool manualRotation;

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

    protected virtual void OnDrawGizmos()
    {
        // Aggression Range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggressionRange);

    }

    public virtual void GetHit()
    {
        healthPoints--;
    }

    public virtual void HitImpact(Vector3 force, Vector3 hitpoint, Rigidbody rb)
    {
        StartCoroutine(HitImpactCoroutine(force, hitpoint, rb));
    }

    private IEnumerator HitImpactCoroutine(Vector3 force, Vector3 hitPoint, Rigidbody rb)
    {
        yield return new WaitForSeconds(0.05f);
        
        rb.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationTrigger();
    public void SetActiveManualMovement(bool manualMovement) => this.manualMovement = manualMovement;
    public bool ManualMovementActive() => manualMovement;
    public void SetActiveManualRotation(bool manualRotation) => this.manualRotation = manualRotation;
    public bool ManualRotationActive() => manualRotation;
    public bool IsPlayerInAggressionRange() => Vector3.Distance(transform.position, player.transform.position) < aggressionRange;


    public Vector3 GetPatrolDestination()
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

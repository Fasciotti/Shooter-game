using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{

    public LayerMask whatIsAlly;

    [Header("Idle Configuration")]
    public float idleTime;
    public float aggressionRange;

    [Header("Move Configuration")]
    public float moveSpeed;
    public float runSpeed;
    public float rotationSpeed;
    private bool manualMovement;
    private bool manualRotation;

    //[Header("Attack Configuration")]
    private bool isMeleeAttackReady;


    //[System.NonSerialized] public float chaseAcceleration = 15;
    //[System.NonSerialized] public float standardAcceleration = 8;


    [Space]

    public Transform[] patrolPoints;
    private Vector3[] patrolPointsPosition; // Used to store the locations initialize, no need to null parent.
    private int currentPatrolIndex;


    public Enemy_Visuals visuals { get; private set; }
    public Player player { get; private set; }
    public NavMeshAgent agent { get; private set; }
    public EnemyStateMachine stateMachine { get; private set; }
    public Animator anim { get; private set; }
    public Ragdoll ragdoll { get; private set; }
    public Enemy_Health health { get; private set; }

    public bool inBattleMode { get; private set; }

    protected virtual void Awake()
    {
        visuals = GetComponent<Enemy_Visuals>();

        stateMachine = new EnemyStateMachine();

        agent = GetComponent<NavMeshAgent>();

        anim = GetComponentInChildren<Animator>();

        ragdoll = GetComponent<Ragdoll>();

        health = GetComponent<Enemy_Health>();

        player = GameObject.Find("Player").GetComponent<Player>();
    }

    protected virtual void Start()
    {
        InitializePatrolPoints();
        InitializePerk();
    }
    protected virtual void Update()
    {
        if (ShouldEnterBattleMode())
        {
            EnterBattleMode();
        }
    }

    private void InitializePatrolPoints()
    {
        patrolPointsPosition = new Vector3[patrolPoints.Length];

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            patrolPointsPosition[i] = patrolPoints[i].position;
            patrolPoints[i].gameObject.SetActive(false);
        }
    }

    public virtual void EnterBattleMode()
    {
        inBattleMode = true;
    }

    protected bool ShouldEnterBattleMode()
    {
        if (IsPlayerInAggressionRange() && !inBattleMode)
        {
            EnterBattleMode();
            return true;
        }

        return false;
    }
    protected void MeleeAttackCheck(Transform[] damagePoints, float damageRadius, int damage, GameObject fx = null)
    {
        if (!isMeleeAttackReady)
            return;

        foreach (Transform damagePoint in damagePoints)
        {
            Collider[] colliders = Physics.OverlapSphere(damagePoint.position, damageRadius, 1 << LayerMask.NameToLayer("Player"));

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<IDamageble>(out var hitbox))
                {
                    isMeleeAttackReady = false;
                    hitbox.TakeDamage(damage);

                    if (fx != null)
                    {
                        GameObject impactFx = ObjectPool.Instance.GetObject(fx, collider.transform.position);
                        ObjectPool.Instance.ReturnObject(impactFx, 1f);

                    }

                    return;
                }
            }
        }
    }


    public virtual void GetHit(int damage)
    {
        health.ReduceHealth(damage);

        if (health.ShouldDie())
            Die();

        EnterBattleMode();
    }

    public virtual void Die()
    {

    }

    public virtual void BulletImpact(Vector3 force, Vector3 hitpoint, Rigidbody rb)
    {
        if (health.ShouldDie())
            StartCoroutine(DeathImpactCoroutine(force, hitpoint, rb));
    }

    protected virtual void InitializePerk()
    {

    }

    private IEnumerator DeathImpactCoroutine(Vector3 force, Vector3 hitPoint, Rigidbody rb)
    {
        yield return new WaitForSeconds(0.05f);

        rb.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);
    }

    #region AnimationTrigger
    public bool IsPlayerInAggressionRange() => Vector3.Distance(transform.position, player.transform.position) < aggressionRange;
    public void AnimationTrigger() => stateMachine.currentState.AnimationTrigger();
    public void SetActiveManualMovement(bool manualMovement) => this.manualMovement = manualMovement;
    public bool ManualMovementActive() => manualMovement;
    public void SetActiveManualRotation(bool manualRotation) => this.manualRotation = manualRotation;
    public bool ManualRotationActive() => manualRotation;
    public virtual void AbilityTrigger() => stateMachine.currentState.AbilityTrigger();
    public void AttackCheckActive(bool active) => isMeleeAttackReady = active;
    #endregion

    public Vector3 GetPatrolDestination()
    {
        Vector3 destination = patrolPointsPosition[currentPatrolIndex];

        currentPatrolIndex++;

        if (currentPatrolIndex >= patrolPoints.Length)
            currentPatrolIndex = 0;

        return destination;
    }

    public void FaceTarget(Vector3 target, float rotationSpeed = 0)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);

        Vector3 currentRotation = transform.rotation.eulerAngles;

        if (rotationSpeed == 0)
        {
            rotationSpeed = this.rotationSpeed;
        }

        float yRotation = Mathf.LerpAngle(currentRotation.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(currentRotation.x, yRotation, currentRotation.z);
    }
    protected virtual void OnDrawGizmos()
    {
        // Aggression Range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggressionRange);

    }
}

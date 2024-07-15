using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine.AI;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] protected int healthPoints = 20;

    [Header("Idle Info")]
    public float idleTime;
    public float agressionRange;

    [Header("Move Info")]
    public float moveSpeed;
    public float chaseSpeed;
    public float turnSpeed;
    private bool manualMovement;
    private bool manualRotation;
    
    [SerializeField] private  Transform[] patrolPoints;
    private Vector3[] patrolPointsPosition;
    private int currentPatrolIndex;

    public bool inBattleMode {  get; private set; }


    public Transform player;
    public Animator anim { get; private set; }

    public NavMeshAgent agent {  get; private set; }
    public EnemyStateMachine stateMachine {  get; private set; }


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


    protected virtual void Update()
    {

    }

    protected bool ShouldEnterBattleMode()
    {
        bool inAggresionRange = Vector3.Distance(transform.position, player.position) < agressionRange;

        if (inAggresionRange && !inBattleMode)
        {
            EnterBattleMode();
            return true;
        }

        return false;
    }

    public virtual void EnterBattleMode()
    {
        inBattleMode = true;
    }


    public virtual void GetHit()
    {
        healthPoints--;
    }

    public virtual void DeathImpact(Vector3 _force, Vector3 _hitPoint, Rigidbody _rb)
    {
        StartCoroutine(DeathImpactCouroutine( _force, _hitPoint, _rb));
    }

    private IEnumerator DeathImpactCouroutine(Vector3 _force, Vector3 _hitPoint, Rigidbody _rb)
    {
        yield return new WaitForSeconds(0.1f);

        _rb.AddForceAtPosition(_force, _hitPoint, ForceMode.Impulse);
    }
    public void FaceTarget(Vector3 _target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(_target - transform.position);

        Vector3 currentEulerAngles = transform.rotation.eulerAngles;

        float yRotation = Mathf.LerpAngle(currentEulerAngles.y, targetRotation.eulerAngles.y, turnSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(currentEulerAngles.x, yRotation, currentEulerAngles.z);
    }



    #region [======= Patrol ========]


    private void InitializePatrolPoints()
    {
        patrolPointsPosition = new Vector3[patrolPoints.Length];

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            patrolPointsPosition[i] = patrolPoints[i].position;
            patrolPoints[i].gameObject.SetActive(false);
        }
    }


    public Vector3 GetPatrolDestination()
    {
        Vector3 destination = patrolPointsPosition[currentPatrolIndex];
        currentPatrolIndex++;

        if (currentPatrolIndex >= patrolPoints.Length)
            currentPatrolIndex = 0;

        return destination;
    }

    #endregion


    #region [====== Gizmos ==========]

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, agressionRange);

    }
    #endregion

    #region [====== Animation Events =======]

    public void ActivateManualMovement(bool _manualMovement) => this.manualMovement = _manualMovement;
    public bool ManualMovementActive() => manualMovement;
    public bool ManualRotationActive() => manualRotation;

    public void ActiveManualRotation(bool _manualRotation) => this.manualRotation = _manualRotation;
    public void Animationtrigger() => stateMachine.currentState.AnimationTrigger();
    public virtual void AbilityTrigger()
    {
        stateMachine.currentState.AbilityTrigger();
    }

    #endregion
}

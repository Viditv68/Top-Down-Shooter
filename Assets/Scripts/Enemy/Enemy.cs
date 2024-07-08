using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine.AI;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float turnSpeed;
    public float agressionRange;

    [Header("Attack Data")]
    public float attackRange;
    public float attackMoveSpeed;


    [Header("Idle Info")]
    public float idleTime;

    [Header("Move Info")]
    public float moveSpeed;
    public float chaseSpeed;
    private bool manualMovement;
    
    [SerializeField] private  Transform[] patrolPoints;
    private int currentPatrolIndex;


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

    public Quaternion FaceTarget(Vector3 _target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(_target - transform.position);

        Vector3 currentEulerAngles = transform.rotation.eulerAngles;

        float yRotation = Mathf.LerpAngle(currentEulerAngles.y, targetRotation.eulerAngles.y, turnSpeed * Time.deltaTime);

        return Quaternion.Euler(currentEulerAngles.x, yRotation, currentEulerAngles.z);
    }


    private void InitializePatrolPoints()
    {
        foreach (var t in patrolPoints)
        {
            t.parent = null;
        }
    }

    public void ActivateManualMovement(bool _manualMovement) => this.manualMovement = _manualMovement;
    public bool ManualMovementActive() => manualMovement;

    public bool PlayerInAgressionRange() => Vector3.Distance(transform.position, player.position) < agressionRange;
    public bool PlayerinAttackRange() => Vector3.Distance(transform.position, player.position) < attackRange;

    #region [======= Getters ========]

    public Vector3 GetPatrolDestination()
    {
        Vector3 destination = patrolPoints[currentPatrolIndex].transform.position;
        currentPatrolIndex++;

        if (currentPatrolIndex >= patrolPoints.Length)
            currentPatrolIndex = 0;

        return destination;
    }

    #endregion


    #region [====== Gizmos ==========]

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, agressionRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    #endregion

    #region [====== Animation Events =======]

    public void Animationtrigger() => stateMachine.currentState.AnimationTrigger();
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine.AI;
using UnityEngine;

public class Enemy : MonoBehaviour
{

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
    
    public void ActiveManualRotation(bool _manualRotation) => this.manualRotation = _manualRotation;
    
    public bool ManualRotationActive() => manualRotation;

    public bool PlayerInAgressionRange() => Vector3.Distance(transform.position, player.position) < agressionRange;

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

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, agressionRange);

    }
    #endregion

    #region [====== Animation Events =======]

    public void Animationtrigger() => stateMachine.currentState.AnimationTrigger();
    #endregion
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : Enemy
{
    public MeleeIdleState idleState { get; private set; } 
    public MeleeMoveState moveState { get; private set; }

    public MeleeRecoveryState recoveryState { get; private set; }
    public MeleeChaseState chaseState { get; private set; }
    public MeleeAttackState attackState { get; private set; }


    [SerializeField] private Transform hiddenWeapon;
    [SerializeField] private Transform pulledWeapon;


    protected override void Awake()
    {
        base.Awake();
        idleState = new MeleeIdleState(this, stateMachine, "Idle");
        moveState = new MeleeMoveState(this, stateMachine, "Move");
        recoveryState = new MeleeRecoveryState(this, stateMachine, "Recovery");
        chaseState = new MeleeChaseState(this, stateMachine, "Chase");
        attackState = new MeleeAttackState(this, stateMachine, "Attack");
    }

    protected override void Start()
    {
        base.Start();
        Debug.Log("Enemy initalize");
        stateMachine.Initialize(idleState);

    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }

    public void PullWeapon()
    {
        hiddenWeapon.gameObject.SetActive(false);
        pulledWeapon.gameObject.SetActive(true);
    }
}

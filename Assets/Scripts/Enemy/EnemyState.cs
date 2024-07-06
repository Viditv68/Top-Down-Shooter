using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{

    protected Enemy enemyBase;
    protected EnemyStateMachine stateMachine;
    protected Animator anim;

    protected string animBoolName;

    protected float stateTimer;

    protected bool triggerCalled;

    public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName)
    {
        enemyBase = _enemyBase;
        stateMachine = _stateMachine;
        animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        Debug.Log(animBoolName);

        enemyBase.anim.SetBool(animBoolName, true);
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName, false);
    }


    #region [======= Animation Events =======]

    public void AnimationTrigger() => triggerCalled = true;


    #endregion
}

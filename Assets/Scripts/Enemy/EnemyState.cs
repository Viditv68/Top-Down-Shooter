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


    public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName)
    {
        enemyBase = _enemyBase;
        stateMachine = _stateMachine;
        animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {

    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {

    }
}

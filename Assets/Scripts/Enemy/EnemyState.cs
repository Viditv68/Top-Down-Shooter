using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{

    protected Enemy enemyBase;
    protected EnemyStateMachine stateMachine;
    protected Animator anim;

    protected string animBoolName;


    public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName)
    {
        enemyBase = _enemyBase;
        stateMachine = _stateMachine;
        animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        Debug.Log("I enter " + animBoolName);
    }

    public virtual void Update()
    {
        Debug.Log("I am runniung " + animBoolName);
    }

    public virtual void Exit()
    {
        Debug.Log("I exit " + animBoolName);

    }
}

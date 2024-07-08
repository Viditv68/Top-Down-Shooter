using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    public Enemy enemy;

    private void Awake()
    {

    }

    public void AnimationTrigger() => enemy.Animationtrigger();
}

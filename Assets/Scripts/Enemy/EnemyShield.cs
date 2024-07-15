using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    [SerializeField] private EnemyMelee enemy;
    [SerializeField] private int durability;

    public void ReduceDurability()
    {
        durability--;

        if(durability <=0 )
        {
            enemy.anim.SetFloat("ChaseIndex", 0);
            Debug.Log("Chase Index 0 ");
            Destroy(gameObject);
        }
    }

}

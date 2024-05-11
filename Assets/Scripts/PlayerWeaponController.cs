using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private Player player;

    private void Start()
    {
        player.controls.Character.Fire.performed += context => Shoot();
    }
    private void Shoot()
    {
        animator.SetTrigger("Fire");
        
    }
}

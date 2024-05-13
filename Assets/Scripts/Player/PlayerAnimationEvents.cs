using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField] private WeaponVisualController weaponVisualController;


    public void ReloadIsOver()
    {
        weaponVisualController.ReturnRigWeightToOne();
    }
}

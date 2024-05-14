using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField] private PlayerWeaponVisuals weaponVisualController;


    public void ReloadIsOver()
    {
        weaponVisualController.MaximizeRigWeight();
    }

    public void ReturnRig()
    {
        weaponVisualController.MaximizeRigWeight();
        weaponVisualController.MaximizeLeftHandWeight();

    }

    public void WeaponGrabIsOver()
    {

        weaponVisualController.SetBusyGrabbingWeapon(false);
    }
}

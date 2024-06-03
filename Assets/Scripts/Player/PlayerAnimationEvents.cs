using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField] private PlayerWeaponVisuals weaponVisualController;
    [SerializeField] private PlayerWeaponController weaponController;


    public void ReloadIsOver()
    {
        weaponVisualController.MaximizeRigWeight();
        weaponController.CurrentWeapon().RefillBullets();

        weaponController.SetWeaponReady(true);
    }

    public void ReturnRig()
    {
        weaponVisualController.MaximizeRigWeight();
        weaponVisualController.MaximizeLeftHandWeight();

    }

    public void WeaponEquippingIsOver()
    {
        weaponController.SetWeaponReady(true );
    }

    public void SwitchOnWeaponModels() => weaponVisualController.SwitchOnCurrentWeaponModel();
}

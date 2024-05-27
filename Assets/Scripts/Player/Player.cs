using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerControls controls { get; private set; }
    [SerializeField] private PlayerAim aim;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerWeaponController playerWeaponController;
    [SerializeField] private PlayerWeaponVisuals playerWeaponVisuals;


    #region [ ======= Getters =========]
    public PlayerAim GetPlayerAim() => aim;
    public PlayerMovement GetPlayerMovement() => playerMovement;

    public PlayerWeaponVisuals GetPlayerWeaponVisuals() => playerWeaponVisuals;

    public PlayerWeaponController GetPlayerWeaponController() => playerWeaponController;
    #endregion

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

}

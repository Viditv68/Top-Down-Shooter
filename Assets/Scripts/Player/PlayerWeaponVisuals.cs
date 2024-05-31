using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponVisuals : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Animator anim;

    private bool isGrabbingWeapon;


    [SerializeField] private WeaponModel[] weaponModels;
    [SerializeField] private BackupWeaponModel[] backupWeaponModels;


    [Header("Left hand IK")]
    [SerializeField] private Transform leftHandIKTarget;
    [SerializeField] private TwoBoneIKConstraint leftHandIK;
    [SerializeField] private float leftHandIKWeightIncreaseRate;
    private bool shouldIncreaseLeftHandIKWeight;


    [Header("Rig")]
    [SerializeField] private Rig rig;
    [SerializeField] private float rigWeightIncreaseRate;
    private bool shouldIncreaseRigWeight;


    private void Start()
    {

    }

    private void Update()
    {
        UpdateRigWeight();
        UpdateLeftHandIKWeight();
    }

    public WeaponModel CurrentWeaponModel()
    {
        WeaponModel weaponModel = null;

        WeaponType weaponType = player.GetPlayerWeaponController().CurrentWeapon().weaponType;

        for (int i = 0; i < weaponModels.Length; i++)
        {
            if (weaponModels[i].weaponType == weaponType)
                weaponModel = weaponModels[i];
        }

        return weaponModel;

    }

    public void PlayReloadAnimation()
    {
        if (isGrabbingWeapon)
            return;

        anim.SetTrigger("Reload");
        ReduceRigWeight();
    }

    public void PlayWeaponEquipAnimation()
    {
        GrabType grabType = CurrentWeaponModel().grabType;


        leftHandIK.weight = 0;
        ReduceRigWeight();
        anim.SetFloat("WeaponGrabType", ((float)grabType));
        anim.SetTrigger("WeaponGrab");

        SetBusyGrabbingWeapon(true);
    }

    public void SetBusyGrabbingWeapon(bool _isBusy)
    {
        isGrabbingWeapon = _isBusy;
        anim.SetBool("BusyGrabbingWeapon", isGrabbingWeapon);
    }

    public void SwitchOnCurrentWeaponModel()
    {
        SwitchOffWeaponModels();
        SwitchOffBackupWeaponModels();
        if(!player.GetPlayerWeaponController().HasOnlyOneWeapon())
            SwitchOnBackUpWeaponModel();

        SwitchAnimationLayer((int)CurrentWeaponModel().holdType);
        CurrentWeaponModel().gameObject.SetActive(true);

        AttachLeftHand();
    }

    public void SwitchOffWeaponModels()
    {
        for (int i = 0; i < weaponModels.Length; i++)
        {
            weaponModels[i].gameObject.SetActive(false);
        }
    }

    private void SwitchOffBackupWeaponModels()
    {
        foreach (BackupWeaponModel weapon in backupWeaponModels)
        {
            weapon.gameObject.SetActive(false);
        }
    }

    public void SwitchOnBackUpWeaponModel()
    {
        WeaponType weaponType = player.GetPlayerWeaponController().BackupWeapon().weaponType;

        foreach (BackupWeaponModel weapon in backupWeaponModels)
        {
            if(weapon.weaponType == weaponType)
            {
                weapon.gameObject.SetActive(true);
            }
        }
    }


    private void SwitchAnimationLayer(int _layerIndex)
    {
        for (int i = 1; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }

        anim.SetLayerWeight(_layerIndex,1);
    }

    #region [ ======== ANimation Rigging Methods =========]
    private void UpdateLeftHandIKWeight()
    {
        if (shouldIncreaseLeftHandIKWeight)
        {
            leftHandIK.weight += leftHandIKWeightIncreaseRate * Time.deltaTime;

            if (leftHandIK.weight >= 1)
                shouldIncreaseLeftHandIKWeight = false;
        }
    }

    private void UpdateRigWeight()
    {
        if (shouldIncreaseRigWeight)
        {
            rig.weight += rigWeightIncreaseRate * Time.deltaTime;

            if (rig.weight >= 1)
                shouldIncreaseRigWeight = false;
        }
    }

    private void ReduceRigWeight()
    {
        rig.weight = 0.15f;
    }

    private void AttachLeftHand()
    {
        Transform targetTransform = CurrentWeaponModel().holdPoint;

        leftHandIKTarget.localPosition = targetTransform.localPosition;
        leftHandIKTarget.localRotation = targetTransform.localRotation;
    }


    public void MaximizeRigWeight() => shouldIncreaseRigWeight = true;
    public void MaximizeLeftHandWeight() => shouldIncreaseLeftHandIKWeight = true;

    #endregion 


}



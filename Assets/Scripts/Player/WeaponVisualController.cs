using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponVisualController : MonoBehaviour
{

    [SerializeField] private Animator anim;

    [SerializeField] private Transform[] gunTransform;

    [SerializeField] private Transform pistol;
    [SerializeField] private Transform revolver;
    [SerializeField] private Transform autoRifle;
    [SerializeField] private Transform shotgun;
    [SerializeField] private Transform rifle;

    private Transform currentGun;

    [Header("Left hand IK")]
    [SerializeField] private Transform leftHand;


    private void Start()
    {
        SwitchOn(pistol);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchOn(pistol);
            SwitchAnimationLayer(1);
        }    
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchOn(revolver);
            SwitchAnimationLayer(2);
        }    
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchOn(autoRifle);
            SwitchAnimationLayer(1);
        }  
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchOn(shotgun);
            SwitchAnimationLayer(2);
        }  
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchOn(rifle);
            SwitchAnimationLayer(3);
        }

    }


    private void SwitchOn(Transform _gunTransform)
    {
        SwitchOffGuns();
        _gunTransform.gameObject.SetActive(true);
        currentGun = _gunTransform;

        AttachLeftHand();
    }

    private void SwitchOffGuns()
    {
        for (int i = 0; i < gunTransform.Length; i++)
        {
            gunTransform[i].gameObject.SetActive(false);
        }
    }

    private void AttachLeftHand()
    {
        Transform targetTransform = currentGun.GetComponentInChildren<LeftHandTargetTransform>().transform;

        leftHand.localPosition = targetTransform.localPosition;
        leftHand.localRotation = targetTransform.localRotation;
    }

    private void SwitchAnimationLayer(int _layerIndex)
    {
        for (int i = 1; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }

        anim.SetLayerWeight(_layerIndex,1);
    }
}

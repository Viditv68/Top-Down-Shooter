using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponVisualController : MonoBehaviour
{

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
        if(Input.GetKeyDown(KeyCode.Alpha0))
            SwitchOn(pistol);
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SwitchOn(revolver);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SwitchOn(autoRifle);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            SwitchOn(shotgun);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            SwitchOn(rifle);

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
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private Player player;
    private PlayerControls controls;

    [Header("Aim Visuals")]
    [SerializeField] private LineRenderer aimLaser;

    [Header("Aim Control")]
    [SerializeField] private Transform aim;

    [SerializeField] private bool isAimingPrecisely;
    [SerializeField] private bool isLookingToTarget;

    [Header("Camera control")]
    [SerializeField] private Transform cameraTarget;
    [Range(0.5f,1f)] 
    [SerializeField] private float minCameraDistance = 1.5f;
    [Range(1f, 3f)] 
    [SerializeField] private float maxCameraDistance = 4f;
    [Range(3,5f)] 
    [SerializeField] private float cameraSensitivity = 5f;

    [SerializeField] private LayerMask aimLayerMask;


    private Vector2 mouserInput;
    private RaycastHit lastKnownMouseHit;

    private void Start()
    {
        controls = player.controls;
        AssignInputEvents();
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)) 
            isAimingPrecisely = !isAimingPrecisely;

        if(Input.GetKeyDown(KeyCode.L))
            isLookingToTarget = !isLookingToTarget;

        UpdateAimVisuals();
        UpdateAimPosition();
        UpdateCameraPosition();

    }



    private void UpdateAimVisuals()
    {
        Transform gunPoint = player.GetPlayerWeaponController().GunPoint();
        Vector3 laserDirection = player.GetPlayerWeaponController().BulletDirection();
        
        float gunDistance = 4f;
        float laserTipLength = 0.5f;

        Vector3 endPoint = gunPoint.position + laserDirection * gunDistance;

        if (Physics.Raycast(gunPoint.position, laserDirection, out RaycastHit hitInfo, gunDistance))
        {
            endPoint = hitInfo.point;
            laserTipLength = 0;
        }

        aimLaser.SetPosition(0, gunPoint.position);
        aimLaser.SetPosition(1, endPoint);
        aimLaser.SetPosition(2, endPoint + laserDirection * laserTipLength);
    }
    private void UpdateAimPosition()
    {

        Transform target = Target();
        if(target != null && isLookingToTarget)
        {
            aim.position = target.position;
            return;
        }
        
        aim.position = GetMouseHitInfo().point;

        if (!isAimingPrecisely)
            aim.position = new Vector3(aim.position.x, transform.position.y, aim.position.z);
    }


    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(mouserInput);

        if(Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            lastKnownMouseHit = hitInfo;
            return hitInfo;
        }

        return lastKnownMouseHit;
    }

    private void AssignInputEvents()
    {
        controls.Character.Aim.performed += context => mouserInput = context.ReadValue<Vector2>();
        controls.Character.Aim.canceled += context => mouserInput = Vector2.zero;
    }



    #region [ ========= Camera Region ===========]
    private void UpdateCameraPosition()
    {
        cameraTarget.position = Vector3.Lerp(cameraTarget.position, DesiredCameraPosition(), cameraSensitivity * Time.deltaTime);
    }
    private Vector3 DesiredCameraPosition()
    {


        float actualMaxCameraDistance = player.GetPlayerMovement().moveInput.y < 0.5f ? minCameraDistance : maxCameraDistance;


        Vector3 desiredCameraPosition = GetMouseHitInfo().point;
        Vector3 aimDirection = (desiredCameraPosition - transform.position).normalized;

        float distanceToDesiredPosition = Vector3.Distance(transform.position, desiredCameraPosition);

        float clampedDistance = Mathf.Clamp(distanceToDesiredPosition, minCameraDistance, actualMaxCameraDistance);

        desiredCameraPosition = transform.position + aimDirection * clampedDistance;
        desiredCameraPosition.y = transform.position.y + 1;

        return desiredCameraPosition;

    }
    #endregion

    public bool CanAimPrecisely() => isAimingPrecisely;

    public Transform Target()
    {
        Transform target = null;

        if (GetMouseHitInfo().transform.GetComponent<Target>() != null)
            target = GetMouseHitInfo().transform;

        return target;
    }
    public Transform Aim() => aim;

}

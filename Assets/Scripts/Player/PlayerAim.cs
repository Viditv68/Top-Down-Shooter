using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private Player player;
    private PlayerControls controls;



    [Header("Aim Control")]
    [SerializeField] private Transform aim;


    [Header("Camera control")]
    [SerializeField] private Transform cameraTarget;
    [Range(0.5f,1f)] 
    [SerializeField] private float minCameraDistance = 1.5f;
    [Range(1f, 3f)] 
    [SerializeField] private float maxCameraDistance = 4f;
    [Range(3,5f)] 
    [SerializeField] private float cameraSensitivity = 5f;

    [SerializeField] private LayerMask aimLayerMask;


    private Vector2 aimInput;
    private RaycastHit lastKnownMouseHit;

    private void Start()
    {
        controls = player.controls;
        AssignInputEvents();
    }


    private void Update()
    {
        aim.position = GetMouseHitInfo().point;
        aim.position = new Vector3(aim.position.x, transform.position.y, aim.position.z);
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




    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);

        if(Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            lastKnownMouseHit = hitInfo;
            return hitInfo;
        }

        return lastKnownMouseHit;
    }

    private void AssignInputEvents()
    {
        controls.Character.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
        controls.Character.Aim.canceled += context => aimInput = Vector2.zero;
    }
}

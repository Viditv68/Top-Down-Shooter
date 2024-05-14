using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private Player player;
    private PlayerControls controls;


    [Header("Aim Info")]
    [SerializeField] private Transform aim;
    [SerializeField] private LayerMask aimLayerMask;
    private Vector3 lookingDirection;

    private Vector2 aimInput;

    private void Start()
    {
        controls = player.controls;
        AssignInputEvents();
    }


    private void Update()
    {
        aim.position = new Vector3(GetMousePosition().x, transform.position.y + 1, GetMousePosition().z);
    }

    public Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);

        if(Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            return hitInfo.point;
        }

        return Vector3.zero;
    }

    private void AssignInputEvents()
    {
        controls.Character.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
        controls.Character.Aim.canceled += context => aimInput = Vector2.zero;
    }
}

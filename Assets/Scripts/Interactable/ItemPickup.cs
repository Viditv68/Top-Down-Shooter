using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;
    private void OnTriggerEnter(Collider other)
    {
        //other.GetComponent<PlayerWeaponController>()?.PickupWeapon(weaponData);
    }
}

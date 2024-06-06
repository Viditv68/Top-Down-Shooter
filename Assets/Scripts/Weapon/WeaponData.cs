using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New EWeapon Data", menuName = "Weapon System/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;

    [Header("Magzines Info")]
    public int bulletsInMagazines;
    public int magazineCapacity;
    public int totalReserveAmmo;


    [Header("Regular Shot")]
    public ShootType shootType;
    public int bulletPerShot = 1;
    public float fireRate;

    [Header("Burst Fire")]
    public bool burstAvailable;
    public bool burstActive;
    public int burstBulletsPerShot;
    public float burstFireRate;
    public float burstFireDelay = 0.1f;



    [Header("Spread")]
    public float baseSpread;
    public float maximumSpread = 3f;
    public float spreadIncreaseRate = 0.15f;

    [Header("Weapon Generics")]
    public WeaponType weaponType;
    [Range(1f, 2f)]
    public float reloadSpeed = 1f;
    [Range(1f, 2f)]
    public float equipmentSpeed = 1f;
    [Range(2f, 12f)]
    public float gunDistance = 4f;
    [Range(3f, 8f)]
    public float cameraDistance = 6f;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private Player player;


    [SerializeField] private Weapon currentWeapon;

    [Header("Bullet Info")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;


    [SerializeField] private Transform weaponHolder;

    [Header("Inventory")]
    [SerializeField] private int maxSlots = 2;
    [SerializeField] private List<Weapon> weaponSlots;

    private bool weaponReady;
    private bool isShooting;

    private PlayerAim playerAim => player.GetPlayerAim();

    private void Start()
    {
        AssignInputEvents();

        currentWeapon.bulletsInMagazines = currentWeapon.totalReserveAmmo;

        Invoke("EquipStartingWeapon", 0.1f);
    }

    private void Update()
    {
        if (isShooting)
            Shoot();
        
    }


    #region [ ======= Slots Management ========= ]

    private void EquipStartingWeapon()
    {
        EquipWeapon(0);
    }



    private void EquipWeapon(int _index)
    {
        SetWeaponReady(false);
        currentWeapon = weaponSlots[_index];
        player.GetPlayerWeaponVisuals().PlayWeaponEquipAnimation();
    }

    private void DropWeapon()
    {
        if (HasOnlyOneWeapon()) 
            return;

        weaponSlots.Remove(currentWeapon);

        EquipWeapon(0);

    }

    public void PickupWeapon(Weapon _newWeapon)
    {
        if(weaponSlots.Count >= maxSlots)
        {
            Debug.Log("No slots available");
            return;
        }

        weaponSlots.Add(_newWeapon);
        player.GetPlayerWeaponVisuals().SwitchOnBackUpWeaponModel();
    }

    public void SetWeaponReady(bool _ready)
    {
        weaponReady = _ready;
    }

    private void Reload()
    {
        SetWeaponReady(false);
        player.GetPlayerWeaponVisuals().PlayReloadAnimation();
    }


    public bool WeaponReady()=>weaponReady;
    #endregion

    public bool HasOnlyOneWeapon() => weaponSlots.Count <= 1;

    private void Shoot()
    {
        if (!currentWeapon.CanShoot() || !WeaponReady())
            return;

        if (currentWeapon.shootType == ShootType.Single)
            isShooting = false;
            
        //GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));
        GameObject bullet = ObjectPool.instance.GetBullet();
        bullet.transform.position = GunPoint().position;
        bullet.transform.rotation = Quaternion.LookRotation(GunPoint().forward);

        bullet.GetComponent<Rigidbody>().velocity = BulletDirection() * bulletSpeed;
        animator.SetTrigger("Fire");
        
    }

    public Vector3 BulletDirection()
    {
        Transform aim = playerAim.Aim();
        Vector3 direction = (aim.position - GunPoint().position).normalized;

        if (!playerAim.CanAimPrecisely() && playerAim.Target() == null)
            direction.y = 0;


        return direction;
    }

    public Transform GunPoint() => player.GetPlayerWeaponVisuals().CurrentWeaponModel().gunPoint;

    public Weapon CurrentWeapon() => currentWeapon;

    public Weapon BackupWeapon()
    {
        foreach (Weapon weapon in weaponSlots)
        {
            if (weapon != currentWeapon)
                return weapon;
        }

        return null;
    }


    #region [ ========= Input Events ========]

    private void AssignInputEvents()
    {
        PlayerControls controls = player.controls;
        controls.Character.Fire.performed += context => isShooting = true;
        controls.Character.Fire.canceled += context => isShooting = false;

        controls.Character.EquipSlot1.performed += context => EquipWeapon(0);
        controls.Character.EquipSlot2.performed += context => EquipWeapon(1);
        controls.Character.DropCurrentWeapon.performed += context => DropWeapon();
        controls.Character.Reload.performed += context =>
        {
            if (currentWeapon.CanReload() && WeaponReady())
            {
                Reload();
            }

        };
    }

    #endregion

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private Player player;

    [SerializeField] private WeaponData defaultWeaponData;


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
        weaponSlots[0] = new Weapon(defaultWeaponData);
        //EquipWeapon(0);
    }



    private void EquipWeapon(int _index)
    {
        if(_index >= weaponSlots.Count) 
            return;
        
        SetWeaponReady(false);
        currentWeapon = weaponSlots[_index];
        player.GetPlayerWeaponVisuals().PlayWeaponEquipAnimation();

        CameraManager.instance.ChangeCameraDistrance(currentWeapon.cameraDistance);
    }

    private void DropWeapon()
    {
        if (HasOnlyOneWeapon()) 
            return;

        weaponSlots.Remove(currentWeapon);

        EquipWeapon(0);

    }

    public void PickupWeapon(WeaponData _newWeapon)
    {
        if(weaponSlots.Count >= maxSlots)
        {
            Debug.Log("No slots available");
            return;
        }

        Weapon newWeapon = new Weapon(_newWeapon);

        weaponSlots.Add(newWeapon);
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

    public Weapon WeaponInSlots(WeaponType _weaponType)
    {
        foreach(Weapon weapon in weaponSlots)
        {
            if (weapon.weaponType == _weaponType)
                return weapon;
        }

        return null;
    }

    private IEnumerator BurstFire()
    {
        SetWeaponReady(false);
        for (int i = 0; i < currentWeapon.bulletPerShot; i++)
        {
            FireSingleBullet();

            yield return new WaitForSeconds(currentWeapon.burstFireDelay);

            if (i >= currentWeapon.bulletPerShot - 1)
                SetWeaponReady(true);
        }
    }



    private void Shoot()
    {
        if (!currentWeapon.CanShoot() || !WeaponReady())
            return;

        player.GetPlayerWeaponVisuals().PlayFireAnimation();

        if (currentWeapon.shootType == ShootType.Single)
            isShooting = false;


        if(currentWeapon.BurstActivated())
        {
            StartCoroutine(BurstFire());
            return;
        }
        
        FireSingleBullet();

    }

    private void FireSingleBullet()
    {
        currentWeapon.bulletsInMagazines--;
        GameObject bulletObj = ObjectPool.instance.GetObject(bulletPrefab);
        bulletObj.transform.position = GunPoint ().position;
        bulletObj.transform.rotation = Quaternion.LookRotation(GunPoint().forward);

        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.BulletSetup(currentWeapon.gunDistance);
        Vector3 bulletDirection = currentWeapon.ApplySpread(BulletDirection());

        bulletObj.GetComponent<Rigidbody>().velocity = bulletDirection * bulletSpeed;
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



    #region [ ========= Input Events ========]

    private void AssignInputEvents()
    {
        PlayerControls controls = player.controls;
        controls.Character.Fire.performed += context => isShooting = true;
        controls.Character.Fire.canceled += context => isShooting = false;

        controls.Character.ToggleWeaponMode.performed += Context => currentWeapon.ToggleBurst();

        controls.Character.EquipSlot1.performed += context => EquipWeapon(0);
        controls.Character.EquipSlot2.performed += context => EquipWeapon(1);
        controls.Character.EquipSlot3.performed += context => EquipWeapon(2);
        controls.Character.EquipSlot4.performed += context => EquipWeapon(3);
        controls.Character.EquipSlot5.performed += context => EquipWeapon(4);


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

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
    [SerializeField] private Transform gunPoint;


    [SerializeField] private Transform weaponHolder;

    [Header("Inventory")]
    [SerializeField] private int maxSlots = 2;
    [SerializeField] private List<Weapon> weaponSlots;

    private PlayerAim playerAim => player.GetPlayerAim();

    private void Start()
    {
        AssignInputEvents();

        currentWeapon.bulletsInMagazines = currentWeapon.totalReserveAmmo;

        Invoke("EquipStartingWeapon", 0.1f);
    }


    #region [ ======= Slots Management ========= ]

    private void EquipStartingWeapon()
    {
        EquipWeapon(0);
    }



    private void EquipWeapon(int _index)
    {
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

    #endregion

    public bool HasOnlyOneWeapon() => weaponSlots.Count <= 1;

    private void Shoot()
    {
        if (!currentWeapon.CanShoot())
            return;

        GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));
       
        bullet.GetComponent<Rigidbody>().velocity = BulletDirection() * bulletSpeed;
        Destroy(bullet, 5f);
        animator.SetTrigger("Fire");
        
    }

    public Vector3 BulletDirection()
    {
        Transform aim = playerAim.Aim();
        Vector3 direction = (aim.position - gunPoint.position).normalized;

        if (!playerAim.CanAimPrecisely() && playerAim.Target() == null)
            direction.y = 0;

        //weaponHolder.LookAt(aim);
        //gunPoint.LookAt(aim);

        return direction;
    }

    public Transform GunPoint() => gunPoint;

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
        controls.Character.Fire.performed += context => Shoot();
        controls.Character.EquipSlot1.performed += context => EquipWeapon(0);
        controls.Character.EquipSlot2.performed += context => EquipWeapon(1);
        controls.Character.DropCurrentWeapon.performed += context => DropWeapon();
        controls.Character.Reload.performed += context =>
        {
            if (currentWeapon.CanReload())
                player.GetPlayerWeaponVisuals().PlayReloadAnimation();
        };
    }

    #endregion

}

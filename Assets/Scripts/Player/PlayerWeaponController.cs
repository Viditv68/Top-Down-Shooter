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
    [SerializeField] private List<Weapon> weaponSlots;

    private PlayerAim playerAim => player.GetPlayerAim();

    private void Start()
    {
        AssignInputEvents();

        currentWeapon.ammo = currentWeapon.maxAmmo;
    }

    private void AssignInputEvents()
    {
        PlayerControls controls = player.controls;
        controls.Character.Fire.performed += context => Shoot();
        controls.Character.EquipSlot1.performed += context => EquipWeapon(0);
        controls.Character.EquipSlot2.performed += context => EquipWeapon(1);
        controls.Character.DropCurrentWeapon.performed += context => DropWeapon();
    }

    private void EquipWeapon(int _index)
    {
        currentWeapon = weaponSlots[_index];
    }

    private void DropWeapon()
    {
        if (weaponSlots.Count <= 1) 
            return;

        weaponSlots.Remove(currentWeapon);

        currentWeapon = weaponSlots[0];

    }



    private void Shoot()
    {
        if (currentWeapon.ammo <= 0)
        {
            Debug.Log("No more bullets");
            return;
        }
        currentWeapon.ammo--;

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

}

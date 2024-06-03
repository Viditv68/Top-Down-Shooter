
using UnityEngine;

public enum WeaponType
{
    Pistol,
    Revolver,
    AutoRifle,
    Shotgun,
    Rifle
}

public enum ShootType
{
    Single,
    Auto
}



[System.Serializable]
public class Weapon
{
    public WeaponType weaponType;

    [Header("Magzines Info")]
    public int bulletsInMagazines;
    public int magazineCapacity;
    public int totalReserveAmmo;

    [Range(1f,2f)]
    public float reloadSpeed = 1f;
    [Range(1f, 2f)]
    public float equipmentSpeed = 1f;

    [Header("Shoot info")]
    public ShootType shootType;
    public float fireRate = 1f;
    private float lastShootTime;



    public bool CanShoot()
    {
        if(HaveEnoughBullets() && ReadyToFire())
        {
            bulletsInMagazines--;
            return true;
        }
        return false;
    }

    private bool ReadyToFire()
    {
        if(Time.time > lastShootTime + 1 / fireRate) 
        {
            lastShootTime = Time.time;
            return true;
        }

        return false;
    }



    #region [ ======== Reload ==========]

    public void RefillBullets()
    {
        totalReserveAmmo += bulletsInMagazines;

        int bulletsToReload = magazineCapacity;

        if (bulletsToReload > totalReserveAmmo)
            bulletsToReload = totalReserveAmmo;

        totalReserveAmmo -= bulletsToReload;
        bulletsInMagazines = bulletsToReload;

        if (totalReserveAmmo < 0)
            totalReserveAmmo = 0;
    }


    public bool CanReload()
    {
        return totalReserveAmmo > 0 && bulletsInMagazines != magazineCapacity;
    }

    private bool HaveEnoughBullets()
    {
        return bulletsInMagazines > 0;
    }
    #endregion
}

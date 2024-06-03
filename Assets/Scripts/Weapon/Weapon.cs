
using UnityEngine;
using UnityEngine.Rendering;

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
    [Header("Shoot info")]
    public ShootType shootType;
    public int bulletPerShot = 1;
    public float defaultFireRate = 1f;
    public float fireRate = 1f;
    private float lastShootTime;

    [Header("Burst Fire")]
    public bool burstAvailable = false;
    public bool burstActive = false;
    public int burstBulletsPerShot = 5;
    public float burstFireRate;
    public float burstFireDelay = 0.1f;

    [Header("Magzines Info")]
    public int bulletsInMagazines;
    public int magazineCapacity;
    public int totalReserveAmmo;

    [Range(1f,2f)]
    public float reloadSpeed = 1f;
    [Range(1f, 2f)]
    public float equipmentSpeed = 1f;

    [Header("Spread")]
    public float baseSpread;
    public float currentSpread = 2f;
    public float maximumSpread = 3f;
    public float spreadIncreaseRate = 0.15f;

    private float lastSpreadUpdateTime;
    private float spreadCooldown = 1f;


    #region [ ========== Burst ===========]

    public bool BurstActivated()
    {
        if (weaponType == WeaponType.Shotgun)
        {
            burstFireDelay = 0;
            return true;
        }
        return burstActive;
    }

    public void ToggleBurst()
    {
        if (!burstAvailable)
            return;

        burstActive = !burstActive;

        if (burstActive)
        {
            bulletPerShot = burstBulletsPerShot;
            fireRate = burstFireRate;
        }
        else
        {
            bulletPerShot = 1;
            fireRate = defaultFireRate;
        }
    }

    #endregion

    #region [ ========== Spread Bullet =============]

    public Vector3 ApplySpread(Vector3 _originalDirection)
    {
        UpdateSpread();
        float randomizeValue = Random.Range(-currentSpread, currentSpread);
        Quaternion spreadRotation = Quaternion.Euler(randomizeValue, randomizeValue, randomizeValue);

        return spreadRotation * _originalDirection;
    }

    private void UpdateSpread()
    {
        if(Time.time > lastSpreadUpdateTime + spreadCooldown)
            currentSpread = baseSpread;
        else
            IncreaseSpread();

        lastSpreadUpdateTime = Time.time;

    }

    private void IncreaseSpread()
    {
        currentSpread = Mathf.Clamp(currentSpread + spreadIncreaseRate, baseSpread, maximumSpread);
    }


    #endregion
    public bool CanShoot() => HaveEnoughBullets() && ReadyToFire();
  
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

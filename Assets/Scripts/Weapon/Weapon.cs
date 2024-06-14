
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

    public ShootType shootType;
    public int bulletPerShot { get; private set; }

    #region [======= Regular Mode Variables =========]
    private float defaultFireRate;
    public float fireRate;
    private float lastShootTime;

    #endregion

    #region [========= Burst Variables ===========]
    private bool burstAvailable;
    public bool burstActive;
    private int burstBulletsPerShot;
    private float burstFireRate = 0.1f;
    public float burstFireDelay { get; private set; }

#endregion

    [Header("Magzines Info")]
    public int bulletsInMagazines;
    public int magazineCapacity;
    public int totalReserveAmmo;


    #region [======== Weapon Generic Info ==========]

    public float reloadSpeed { get; private set; }
    public float equipmentSpeed { get; private set; }
    public float gunDistance { get; private set; }
    public float cameraDistance { get; private set; }
    #endregion

    #region [====== Weapon Spread Variables =========]

    [Header("Spread")]
    private float baseSpread;
    private float currentSpread;
    private float maximumSpread;
    private float spreadIncreaseRate;

    private float lastSpreadUpdateTime;
    private float spreadCooldown;

    #endregion


    public WeaponData weaponData { get; private set; }

    public Weapon(WeaponData _weaponData)
    {
        weaponData = _weaponData;

        bulletsInMagazines = _weaponData.bulletsInMagazines;
        magazineCapacity = _weaponData.magazineCapacity;
        totalReserveAmmo = _weaponData.totalReserveAmmo;

        fireRate = _weaponData.fireRate;
        weaponType = _weaponData.weaponType;

        bulletPerShot = _weaponData.bulletPerShot;
        shootType = _weaponData.shootType;

        burstAvailable = _weaponData.burstAvailable;
        burstActive = _weaponData.burstActive;
        bulletPerShot = _weaponData.burstBulletsPerShot;
        burstFireRate = _weaponData.burstFireRate;
        burstFireDelay = _weaponData.burstFireDelay;


        baseSpread = _weaponData.baseSpread;
        maximumSpread = _weaponData.maximumSpread;
        spreadIncreaseRate = _weaponData.spreadIncreaseRate;

        reloadSpeed = _weaponData.reloadSpeed;
        equipmentSpeed = _weaponData.equipmentSpeed;
        gunDistance = _weaponData.gunDistance;
        cameraDistance = _weaponData.cameraDistance;

        defaultFireRate = fireRate;

    }


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

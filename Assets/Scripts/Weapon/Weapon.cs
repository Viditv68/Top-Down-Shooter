
public enum WeaponType
{
    Pistol,
    Revolver,
    AutoRifle,
    Shotgun,
    Rifle
}



[System.Serializable]
public class Weapon
{
    public WeaponType weaponType;
    public int bulletsInMagazines;
    public int magazineCapacity;
    public int totalReserveAmmo;

    public void RefillBullets()
    {
        totalReserveAmmo += bulletsInMagazines;

        int bulletsToReload = magazineCapacity;

        if(bulletsToReload > totalReserveAmmo)
            bulletsToReload = totalReserveAmmo;
        
        totalReserveAmmo -= bulletsToReload;
        bulletsInMagazines = bulletsToReload;

        if (totalReserveAmmo < 0)
            totalReserveAmmo = 0;
    }


    public bool CanShoot()
    {
        return HaveEnoughBullets();
    }

    public bool CanReload()
    {
        return totalReserveAmmo > 0 && bulletsInMagazines != magazineCapacity;
    }

    private bool HaveEnoughBullets()
    {
        if (bulletsInMagazines > 0)
        {
            bulletsInMagazines--;
            return true;
        }

        return false;
    }

}

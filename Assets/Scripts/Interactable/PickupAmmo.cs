using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AmmoBoxType
{
    SmallBox,
    BigBox
}


[Serializable]
public struct AmmoData
{
    public WeaponType weaponType;
    [Range(10, 100)] public int minAmount;
    [Range(10, 100)] public int maxAmount;
}


public class PickupAmmo : Interactable
{

    [SerializeField] private AmmoBoxType boxType;

    [SerializeField] private List<AmmoData> smallBoxAmmo;
    [SerializeField] private List<AmmoData> bigBoxAmmo;

    [SerializeField] private GameObject[] boxModel;

    private void Start()
    {
        SetupBoxModel();
    }

    public override void Interaction()
    {
        base.Interaction();
        List<AmmoData> currentAmmoList = smallBoxAmmo;

        if(boxType == AmmoBoxType.BigBox)
            currentAmmoList = bigBoxAmmo;

        foreach (AmmoData ammo in currentAmmoList)
        {
            Weapon weapon = weaponController.WeaponInSlots(ammo.weaponType);
            AddBulletsToWeapon(weapon, GetBulletAmount(ammo));

        }

        ObjectPool.instance.ReturnObject(gameObject);
    }



    private void AddBulletsToWeapon(Weapon _weapon, int _amount)
    {
        if (_weapon == null)
            return;

        _weapon.totalReserveAmmo += _amount;
    }

    private void SetupBoxModel()
    {
        for (int i = 0; i < boxModel.Length; i++)
        {
            boxModel[i].SetActive(false);
            if (i == ((int)boxType))
            {
                boxModel[i].SetActive(true);
                UpdateMeshAndMaterial(boxModel[i].GetComponent<MeshRenderer>());

            }
        }
    }

    private int GetBulletAmount(AmmoData _ammoData)
    {
        float min = Mathf.Min(_ammoData.minAmount, _ammoData.maxAmount);
        float max = Mathf.Max(_ammoData.minAmount, _ammoData.maxAmount);
        float randomAmount = UnityEngine.Random.Range(min, max);
        return Mathf.RoundToInt(randomAmount);
    }

}

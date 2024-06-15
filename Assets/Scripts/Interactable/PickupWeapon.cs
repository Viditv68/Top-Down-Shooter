using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PickupWeapon : Interactable
{

    [SerializeField] private WeaponData weaponData;
    [SerializeField] private Weapon weapon;

    [SerializeField] private BackupWeaponModel[] models;


    private bool oldWeapon;


    private void Start()
    {
        if(!oldWeapon)
            weapon = new Weapon(weaponData);

        SetupGameObject();
    }


    public void SetupPickupWeapon(Weapon _weapon, Transform _transform)
    {
        oldWeapon = true;
        weapon = _weapon;
        weaponData = weapon.weaponData;

        transform.position = _transform.position + new Vector3(0, 0.75f, 0);
    }


    [ContextMenu("Update Item Model")]
    public void SetupGameObject()
    {
        gameObject.name = "PickupWeapon - " + weaponData.weaponType.ToString();
        SetupWeaponModel();
    }

    private void SetupWeaponModel()
    {
        foreach (BackupWeaponModel model in models)
        {
            model.gameObject.SetActive(false);

            if(model.weaponType == weaponData.weaponType)
            {
                model.gameObject.SetActive(true);
                UpdateMeshAndMaterial(model.GetComponent<MeshRenderer>());
            }
        }
    }
    public override void Interaction()
    {
        weaponController.PickupWeapon(weapon);
        ObjectPool.instance.ReturnObject(gameObject);
    }

  
}

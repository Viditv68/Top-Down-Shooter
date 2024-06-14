using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PickupWeapon : Interactable
{
    private PlayerWeaponController weaponController;
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private Weapon weapon;

    [SerializeField] private BackupWeaponModel[] models;


    private bool oldWeapon;


    private void Start()
    {
        if(!oldWeapon)
            weapon = new Weapon(weaponData);

        UpdateGameObject();
    }


    public void SetupPickupWeapon(Weapon _weapon, Transform _transform)
    {
        oldWeapon = true;
        weapon = _weapon;
        weaponData = weapon.weaponData;

        transform.position = _transform.position + new Vector3(0, 0.75f, 0);
    }


    [ContextMenu("Update Item Model")]
    public void UpdateGameObject()
    {
        gameObject.name = "PickupWeapon - " + weaponData.weaponType.ToString();
        UpdateItemModel();
    }

    public void UpdateItemModel()
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

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if(weaponController == null)
            weaponController = other.GetComponent<PlayerWeaponController>();
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
}

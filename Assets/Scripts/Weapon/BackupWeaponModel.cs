using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum HangType
{
    LowBackHang,
    BackHang,
    SideHang
}


public class BackupWeaponModel : MonoBehaviour
{
    public WeaponType weaponType;
    [SerializeField] private HangType hangType;


    public void Activate(bool _activated) => gameObject.SetActive(_activated);

    public bool HangTypeIs(HangType _hangType)
    {
        return hangType == _hangType;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;
using Harmony;

public class infAmmo : MonoBehaviour
{
    private void Awake()
    {
        wm = base.GetComponentInChildren<WeaponManager>();
    }
    private void Update()
    {
        if (!wm.isMasterArmed)
            return;
        foreach (var equip in wm.GetCombinedEquips())
        {
            if (equip != null)
            {
                if (equip is HPEquipGun)
                {
                    if (((HPEquipGun)wm.currentEquip).gun == null)
                    {
                        Debug.LogError("Gun is null on HPEQUIP " + equip.name);
                        continue;
                    }
                    ((HPEquipGun)equip).gun.currentAmmo = ((HPEquipGun)equip).gun.maxAmmo;
                }
                else if (equip is RocketLauncher)
                {
                    if (((RocketLauncher)equip).GetCount() != ((RocketLauncher)equip).GetMaxCount())
                        ((RocketLauncher)equip).LoadCount(999);
                }
                else
                {
                    ((HPEquipMissileLauncher)equip).ml.LoadAllMissiles();
                }
            }
        }
    }
    public WeaponManager wm;
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;
using Harmony;

public class PairedGuns : MonoBehaviour
{
    private void Awake()
    {
        wm = base.GetComponentInChildren<WeaponManager>();
        OnSingleGunFired = new UnityAction<bool>(setAllFire);
        CustomLoadout.OnLoadoutEquipped.AddListener(new UnityAction<WeaponManager>(CollectGun));
    }

    private void CollectGun(WeaponManager manager)
    {
        if (manager != wm)
            return;
        foreach (Gun gun in guns)
            gun.OnSetFire.RemoveListener(OnSingleGunFired);

        guns = new List<Gun>();
        foreach (var gun in (HPEquippable[])Traverse.Create(wm).Field("equips").GetValue())
        {
            if (gun == null || !(gun is HPEquipGun))
                continue;
            AEATDebugLogger.Log("Found equip " + gun.name + ".");
            AEATDebugLogger.Log("Found gun " + gun.name + ", adding it to the paired gun list.");
            guns.Add(((HPEquipGun)gun).gun);
            if (gun.hardpointIdx != 0)
                gun.jettisonable = true;
            ((HPEquipGun)gun).gun.OnSetFire.AddListener(OnSingleGunFired);

        }
    }

    private void setAllFire(bool firing)
    {
        if (!this.enabled)
            return;
        foreach (var gun in guns)
        {
            if (gun == null)
            {
                guns.Remove(gun);
                continue;
            }
            if (EquipConstants.debug)
                Debug.Log("Gun found " + gun.name + " setting firing to " + firing);
            gun.SetFire(firing);
        }
    }
    private void OnDisable()
    {
        foreach (var gun in guns)
            gun.SetFire(false);
    }
    public WeaponManager wm;
    private UnityAction<bool> OnSingleGunFired = null;
    private List<Gun> guns = new List<Gun>();
}
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
        foreach (var gun in wm.GetCombinedEquips())
        {
            if (gun is HPEquipGun)
                guns.Add(((HPEquipGun)gun).gun);
            ((HPEquipGun)gun).gun.OnSetFire.AddListener(new UnityAction<bool>(setAllFire));
        }
    }
    private void setAllFire(bool firing)
    {
        if (!this.enabled)
            return;
        foreach (var gun in guns)
        {
            gun.SetFire(firing);
        }
    }
    public WeaponManager wm;
    private List<Gun> guns = new List<Gun>();
}
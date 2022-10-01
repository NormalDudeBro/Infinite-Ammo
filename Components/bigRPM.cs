/*﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;
using Harmony;

public class BigRPM : MonoBehaviour
{
    private void Awake()
    {
        wm = base.GetComponentInChildren<WeaponManager>();
        CustomLoadout.OnLoadoutEquipped.AddListener(new UnityAction<WeaponManager>(CollectGun));
    }

    private void CollectGun(WeaponManager manager)
    {
        if (wm != manager)
            return;
        bool prevEnabled = enabled;
        this.enabled = false;
        guns = new Dictionary<Gun, float[]>();
        foreach (HPEquippable gun in (HPEquippable[])Traverse.Create(wm).Field("equips").GetValue())
            if (gun is HPEquipGun)
                guns.Add(((HPEquipGun)gun).gun, new float []{ (float)Traverse.Create(((HPEquipGun)gun).gun).Field("fireInterval").GetValue(), ((HPEquipGun)gun).gun.rpm });
        enabled = prevEnabled;
    }

    private void OnEnable() => toggleRpm();
    private void OnDisable() => toggleRpm();

    public void toggleRpm()
    {
        foreach (var gun in guns.Keys)
        {
            if (gun == null)
                continue;
            if (enabled)
            {
                Traverse.Create(gun).Field("fireInterval").SetValue(0.001f);
                gun.rpm = 4100f;
            }
            else
            {
                Traverse.Create(gun).Field("fireInterval").SetValue(guns[gun][0]);
                gun.rpm = guns[gun][1];
            }
        }
    }
    private WeaponManager wm;
    private Dictionary<Gun, float[]> guns = new Dictionary<Gun, float[]>();
}

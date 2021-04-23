using System;
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
        foreach (var gun in wm.GetCombinedEquips())
        {
            if (gun is HPEquipGun)
                guns.Add(((HPEquipGun)gun).gun, ((HPEquipGun)gun).gun.rpm);
        }
    }
    public void toggleRpm()
    {
        foreach (var gun in guns.Keys)
        {
            if (gun == null)
                continue;
            if (bigRPM)
                gun.rpm = 9999f;
            else
                guns.TryGetValue(gun, out gun.rpm);
        }
        bigRPM = !bigRPM;
    }
    private WeaponManager wm;
    private Dictionary<Gun, float> guns = new Dictionary<Gun, float>();
    private bool bigRPM = false;
}
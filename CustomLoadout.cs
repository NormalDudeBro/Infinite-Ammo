using Harmony;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class CustomLoadout : Loadout
{
    public CustomLoadout(Loadout loadoutToConvert)
    {
        if (loadoutToConvert.hpLoadout == null)
        {
            Debug.LogError("hpLoadout is null.");
            hpLoadout = new string[0];
        }
        else
            this.hpLoadout = (string[])loadoutToConvert.hpLoadout.Clone();
        if (loadoutToConvert.cmLoadout == null)
        {
            Debug.LogError("cmLoadout is null.");
            cmLoadout = new int[] { 0, 0 };
        }
        else
            this.cmLoadout = (int[])loadoutToConvert.cmLoadout.Clone();

        this.loadoutPaths = new Dictionary<int, string>();
        allPaths = new List<string>();
        for (int i = 0; i < hpLoadout.Length; i++)
        {
            if (string.IsNullOrEmpty(hpLoadout[i]))
                continue;
            string path = getPathFromEquip(hpLoadout[i]);
            if (path == null)
            {
                Debug.LogError(hpLoadout[i] + " has a null path.");
                continue;
            }
            if (!allPaths.Contains(path))
                allPaths.Add(path);
        }
    }
    public void equipThisCustomLoadout(WeaponManager wm)
    {
        wm.ClearEquips();
        wm.MarkAllJettison();
        wm.JettisonMarkedItems();
        Traverse wmTraverse = Traverse.Create(wm);
        HPEquippable[] weaponsSaved = (HPEquippable[])wmTraverse.Field("equips").GetValue();
        HPEquippable[] newWeapons = new HPEquippable[weaponsSaved.Length];
        weaponsSaved = new HPEquippable[weaponsSaved.Length];
        wmTraverse.Field("equips").SetValue(newWeapons);
        foreach (string path in allPaths)
        {
            wm.resourcePath = "HPEquips/" + path;
            Debug.Log("path is now " + wm.resourcePath);
            wm.EquipWeapons(this);
            for (int i = 0; i < newWeapons.Length; i++)
            {
                if (newWeapons[i] != null)
                    weaponsSaved[i] = newWeapons[i];
            }
            Debug.Log("Set weapons saved.");
            newWeapons = new HPEquippable[weaponsSaved.Length];
            wmTraverse.Field("equips").SetValue(newWeapons);
        }
        wmTraverse.Field("equips").SetValue(weaponsSaved);
        foreach (var equip in weaponsSaved)
        {
            if (equip != null)
            {
                AEATDebugLogger.Log("Found saved equip: " + equip.name);
                foreach (var equip2 in (HPEquippable[])wmTraverse.Field("equips").GetValue())
                    if (equip == equip2)
                        continue;
                AEATDebugLogger.Log("Deleting equip " + equip.name);
            }
        }
        foreach (var equip in (HPEquippable[])wmTraverse.Field("equips").GetValue())
            if (equip != null)
                AEATDebugLogger.Log("Equip found in root: " + equip.name);
        wmTraverse.Method("SetCombinedEquips").GetValue();
        OnLoadoutEquipped.Invoke(wm);
    }
    private string getPathFromEquip(string equipName)
    {
        if (string.IsNullOrEmpty(equipName) || !EquipConstants.isAllowed(equipName))
            return null;

        for (int i = 0; i < EquipConstants.allPaths.Length; i++)
            foreach (string equip in EquipConstants.allowedEquips[i])
                if (equip == equipName)
                    return EquipConstants.allPaths[i];
        Debug.Log("Couldn't find a path for " + equipName);
        return null;
    }
    public Dictionary<int, string> loadoutPaths;
    public List<string> allPaths;
    public static UnityEvent<WeaponManager> OnLoadoutEquipped = new UnityEvent<WeaponManager>();
}

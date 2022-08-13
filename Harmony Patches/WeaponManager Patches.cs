using Harmony;
using UnityEngine;

[HarmonyPatch(typeof(WeaponManager), "EquipWeapons")]
public static class Ensure_AllWeaponsEquipped
{
    public static bool Prefix(ref Loadout loadout, WeaponManager __instance)
    {
        if (loadout == null)
        {
            Debug.Log("Null loadout?");
            return false;
        }
        if (loadout is CustomLoadout)
            return true;
        new CustomLoadout(loadout).equipThisCustomLoadout(__instance);
        return false;
    }
}
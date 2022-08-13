using Harmony;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[HarmonyPatch(typeof(VehicleSave), nameof(VehicleSave.GetCampaignSave))]
public static class Patch_SpoofSave
{
    public static bool Prefix(ref CampaignSave __result, string campaignID)
    {
        if (!(VTOLAPI.currentScene == VTOLScenes.VehicleConfiguration) || AEAT.trueSave == null)
        {
            //Debug.Log("Not spoofing this save.");
            return true;
        }
        Debug.Log(AEAT.trueSave.campaignID + " will be used as a campaign save.");
        __result = AEAT.trueSave;
        if (!AEAT.buttonMade)
        {
            Debug.Log(AEAT.path + " is AEAT.path");
            VTOLAPI.GetPlayersVehicleGameObject().GetComponentInChildren<WeaponManager>().resourcePath = "hpequips/" + AEAT.path;
            LoadoutConfigurator config = Resources.FindObjectsOfTypeAll<LoadoutConfigurator>()[0];
            config.wm.resourcePath = VTOLAPI.GetPlayersVehicleGameObject().GetComponentInChildren<WeaponManager>().resourcePath;
            PilotSaveManager.currentVehicle.equipsResourcePath = "hpequips/" + AEAT.path;
            Traverse.Create(config).Field("unlockedWeaponPrefabs").SetValue(new Dictionary<string, EqInfo>());
            Traverse.Create(config).Field("allWeaponPrefabs").SetValue(new Dictionary<string, EqInfo>());
            config.lockedHardpoints = new List<int>();
            __result.availableWeapons = EquipConstants.allowedEquips[EquipConstants.GetPathIndex(AEAT.path)].ToList();
            Debug.Log("Set allowedEquips.");
            PilotSaveManager.currentVehicle.allEquipPrefabs = EquipConstants.allEquips[EquipConstants.GetPathIndex(AEAT.path)].ToList(); // Don't need to reinit the config because this is prefixing the init statement
        }
        return false;
    }
}

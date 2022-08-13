using Harmony;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[HarmonyPatch(typeof(ReArmingPoint), "FinalBeginReArm")]
public static class patch3
{
    public static void Postfix(RearmingUnitSpawn __instance)
    {
        if (PilotSaveManager.currentScenario.equipConfigurable == false)
            return;
        VTOLAPI.GetPlayersVehicleGameObject().GetComponentInChildren<WeaponManager>().resourcePath = "hpequips/" + AEAT.path;
        LoadoutConfigurator config = (LoadoutConfigurator)Traverse.Create(__instance).Field("config").GetValue();
        //config.wm.resourcePath = VTOLAPI.GetPlayersVehicleGameObject().GetComponentInChildren<WeaponManager>().resourcePath;
        Debug.Log(AEAT.path);
        AEAT.SetupConfigurator(config);
        /*PilotSaveManager.currentVehicle.equipsResourcePath = "hpequips/" + AEAT.path;
        config.availableEquipStrings = EquipConstants.allowedEquips[EquipConstants.GetPathIndex(AEAT.path)].ToList();
        PilotSaveManager.currentVehicle.allEquipPrefabs = EquipConstants.allEquips[EquipConstants.GetPathIndex(AEAT.path)].ToList();
        Traverse configTraverse = Traverse.Create(config);
        configTraverse.Field("unlockedWeaponPrefabs").SetValue(new Dictionary<string, EqInfo>());
        configTraverse.Field("allWeaponPrefabs").SetValue(new Dictionary<string, EqInfo>());
        config.lockedHardpoints = new List<int>(); // making sure all hardpoints are unlocked
        for (int i = 0; i < config.wm.hardpointTransforms.Length; i++)
        {
            config.Detach(i);
        }
        config.Initialize(PilotSaveManager.current.GetVehicleSave(PilotSaveManager.currentVehicle.vehicleName).GetCampaignSave(PilotSaveManager.currentCampaign.campaignID), false);
        if (config.fullInfo != null)
        {
            config.fullInfo.CloseInfo();
        }
        config.Initialize(PilotSaveManager.current.GetVehicleSave(PilotSaveManager.currentVehicle.vehicleName).GetCampaignSave(PilotSaveManager.currentCampaign.campaignID), false);
        if (config.fullInfo != null)
        {
            config.fullInfo.CloseInfo();
        }*/
    }
}
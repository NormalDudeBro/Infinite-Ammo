using Harmony;
using UnityEngine;

[HarmonyPatch(typeof(EndMission), "OnCompleteMission")]
public static class Patch_EnsureScenarioComplete
{
    public static bool Prefix()
    {
        if (PilotSaveManager.currentScenario.equipConfigurable == false)
            return true;
        if (AEAT.trueSave == null)
            Debug.LogError("True save is null.");
        PilotSaveManager.currentVehicle = VTResources.GetPlayerVehicle(AEAT.trueSave.vehicleName);
        Debug.Log("Current scenarioID is: " + PilotSaveManager.currentScenario.scenarioID + " campaignID is " + PilotSaveManager.currentCampaign.campaignID + " and name is " + PilotSaveManager.currentVehicle.name + " and campaign name is " + PilotSaveManager.currentCampaign.campaignName);
        bool gotAnything = false;
        if (PilotSaveManager.current.GetVehicleSave(PilotSaveManager.currentVehicle.vehicleName) != null)
            Debug.Log("Got the vehicle save, pog?");
        else
            Debug.Log("No save not pog.");
        if (PilotSaveManager.current.GetVehicleSave(PilotSaveManager.currentVehicle.vehicleName).GetCampaignSave(PilotSaveManager.currentCampaign.campaignID) != null)
            Debug.Log("welp that wasn't null.");
        else
            Debug.Log("Campaign save is null.");
        //AEATDebugLogger.Log((bool)Traverse.Create(EndMission.instance).Field("done").GetValue() + " is done.");
        foreach (CampaignSave campaignSave in PilotSaveManager.current.GetVehicleSave(PilotSaveManager.currentVehicle.vehicleName).campaignSaves)
        {
            if (campaignSave.campaignID == PilotSaveManager.currentCampaign.campaignID)
            {
                AEATDebugLogger.Log("Got campaign id at least.");
                gotAnything = true;
                foreach (CampaignSave.CompletedScenarioInfo completedScenarioInfo in campaignSave.completedScenarios)
                {
                    if (completedScenarioInfo.scenarioID == PilotSaveManager.currentScenario.scenarioID)
                    {
                        AEATDebugLogger.Log("Got both?");
                        gotAnything = true;
                    }
                    else
                        AEATDebugLogger.Log("ScenarioID " + completedScenarioInfo.scenarioID);

                }
            }
            else
                AEATDebugLogger.Log("Campaign name " + campaignSave.campaignName + " CampaignID " + campaignSave.campaignID);
        }
        if (!gotAnything)
            Debug.Log("Got nothing rip.");
        return true;
    }
}

using Harmony;

[HarmonyPatch(typeof(PlayerSpawn), "OnPreSpawnUnit")]
class Patch_OnPreSpawnUnit // i yoinked the name and stuff from mp, but the actual code isn't 
{
    public static bool Prefix(PlayerSpawn __instance)
    {
        if (AEAT.selectedVehicle != null)
        {
            PilotSaveManager.currentVehicle = AEAT.selectedVehicle;
            VTScenario.current.vehicle = AEAT.selectedVehicle;
        }
        return true;
    }
}
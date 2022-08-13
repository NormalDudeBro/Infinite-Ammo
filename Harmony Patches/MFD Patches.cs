using Harmony;
using UnityEngine;

[HarmonyPatch(typeof(MFDManager), "Awake")]
public static class Inject_MFDPages
{
    public static void Prefix(MFDManager __instance)
    {
        //if (__instance.name != "MFDManager")
            //return;
        GameObject.Instantiate(EquipConstants.AEATMFDPage, __instance.transform).AddComponent<AEATMFDPage>();
        AEATDebugLogger.Log("Tried adding AEAT page.");
    }
}

[HarmonyPatch(typeof(MFDPage), "Awake")]
public static class Patch_MFDAwake
{
    public static void Prefix(MFDPage __instance)
    {
        if (__instance.pageName != "home")
            return;
        MFDPage.MFDButtonInfo newButton = new MFDPage.MFDButtonInfo
        {
            button = MFD.MFDButtons.T2,
            label = "AEAT",
            toolTip = "AEAT Settings"
        };
        newButton.OnPress.AddListener(new UnityEngine.Events.UnityAction(() =>
        {
            AEATDebugLogger.Log("Try open AEAT Page.");
            __instance.OpenPage("aeatPage");
        }));
        __instance.buttons = __instance.buttons.AddToArray(newButton);
        AEATDebugLogger.Log("Tried adding AEAT page button.");
    }
}
[HarmonyPatch(typeof(MFDPortalManager), "Awake")]
public static class Patch_MFDPortalAwake
{
    public static void Prefix(MFDPortalManager __instance)
    {
        MFDPortalPage page = GameObject.Instantiate(EquipConstants.AEATF45MFDPage, __instance.pages[0].transform.parent).GetComponent<MFDPortalPage>();
        __instance.pages.Add(page);
        page.gameObject.AddComponent<AEATF45MFDPage>();
    }
}
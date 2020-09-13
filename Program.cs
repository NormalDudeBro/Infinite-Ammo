using Harmony;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

class LOL : VTOLMOD
{
    public static bool buttonMade = false;
    public static string path = "afighter";
    public static Text text;
    public static bool configInit = false;
    public static infAmmo inf;
    public static MFDPage mfd;
    public static MFD lastMFD = null;
    public static WeaponManager wm;
    public static PairedGuns paired;
    public static MFDPortalManager MFDP;
    public static Text text1;
    public static Text text2;
    private IEnumerator main()
    {
        while (VTMapManager.fetch == null || !VTMapManager.fetch.scenarioReady || FlightSceneManager.instance.switchingScene)
        {
            yield return null;
        }
        // Main code here
        buttonMade = false;
        text = null;
        Debug.Log("AAAAAAAA");
        wm = VTOLAPI.GetPlayersVehicleGameObject().GetComponent<WeaponManager>();
        inf = VTOLAPI.GetPlayersVehicleGameObject().AddComponent<infAmmo>();
        inf.enabled = false;
        paired = VTOLAPI.GetPlayersVehicleGameObject().AddComponent<PairedGuns>();
        paired.enabled = false;
        if (VTOLAPI.GetPlayersVehicleEnum() != VTOLVehicles.F45A)
        {
            WeaponManagerUI wmUI = wm.ui;
            Debug.Log("wmUI");
            mfd = wmUI.mfdPage;
            if (mfd == null)
            {
                Debug.LogError("OOPIES.");
            }
            mfd.OnActivatePage.AddListener(new UnityAction(() =>
            {
                lastMFD = mfd.mfd;
                Debug.Log("=");
                MFDPage.MFDButtonInfo[] allButtons = mfd.buttons;
                MFDPage.MFDButtonInfo newButton = new MFDPage.MFDButtonInfo();
                newButton.button = MFD.MFDButtons.T4;
                newButton.toolTip = "Toggle infinite ammo";
                newButton.label = "infAmmo";
                newButton.OnPress.AddListener(new UnityAction(() =>
                {
                    LOL.inf.enabled = !LOL.inf.enabled;
                    mfd.mfd.buttons[13].GetComponentInChildren<Text>().color = LOL.inf.enabled ? Color.green : Color.red;
                }));
                if (mfd.mfd == null)
                {
                    Debug.LogError("FOUND YA!");
                }
                MFDPage.MFDButtonInfo newButton2 = new MFDPage.MFDButtonInfo();
                newButton2.button = MFD.MFDButtons.T2;
                newButton2.toolTip = "Pair all guns";
                newButton2.label = "pairGuns";
                newButton2.OnPress.AddListener(new UnityAction(() =>
                {
                    LOL.paired.enabled = !LOL.paired.enabled;
                    Debug.Log("Button pressed.");
                    mfd.mfd.buttons[11].GetComponentInChildren<Text>().color = LOL.paired.enabled && wm.equippedGun ? Color.green : Color.red;
                }));
                mfd.SetPageButton(newButton);
                mfd.SetPageButton(newButton2);
                mfd.mfd.buttons[13].GetComponentInChildren<Text>().color = LOL.inf.enabled ? Color.green : Color.red;
                mfd.mfd.buttons[11].GetComponentInChildren<Text>().color = LOL.paired.enabled ? Color.green : Color.red;
            }));
            mfd.OnDeactivatePage.AddListener(new UnityAction(() =>
            {
                if (lastMFD != null)
                {
                    lastMFD.buttons[13].GetComponentInChildren<Text>().color = Color.white;
                    lastMFD.buttons[11].GetComponentInChildren<Text>().color = Color.white;
                    lastMFD = null;
                }
            }));
        }
        else
        {
            LOL.MFDP = wm.gameObject.GetComponentInChildren<MFDPortalManager>();
            MFDPStoresManagement MFDP = (MFDPStoresManagement)LOL.MFDP.gameObject.GetComponentInChildren<MFDPortalManager>().pages[5];
            Debug.Log("got MFPD");
            GameObject toCopy = null;
            foreach (var resource in Resources.FindObjectsOfTypeAll<VRInteractable>())
            {
                if (resource.interactableName == "Weapon Bay Door Overrides")
                {
                    toCopy = resource.gameObject;
                    break;
                }
            }
            if (toCopy == null)
            {
                Debug.LogError("To copy is null");
            }
            GameObject emptyButton = Instantiate(toCopy, MFDP.displayObj.gameObject.transform);
            RectTransform rt = emptyButton.GetComponent<RectTransform>();
            LOL.text1 = emptyButton.gameObject.GetComponentInChildren<Text>();
            rt.localPosition = new Vector3(rt.localPosition.x - 50, rt.localPosition.y, rt.localPosition.z);
            rt.localScale = new Vector3(rt.localScale.x * 0.85f, rt.localScale.y * 0.85f, rt.localScale.z * 0.85f);
            rt.GetComponentInChildren<Image>().color = Color.black;
            Debug.Log("instantiate");
            VRInteractable interactable = emptyButton.GetComponentInChildren<VRInteractable>();
            Debug.Log("vr interactable");
            text = emptyButton.GetComponentInChildren<Text>();
            Debug.Log("text");
            text.text = "infAmmo";
            Debug.Log("infAmmo");
            interactable.OnInteract = new UnityEvent();
            Debug.Log("new UnityEvent()");
            interactable.interactableName = "Toggle infinite ammo";
            Debug.Log("toggle infinite ammo");
            interactable.OnInteract.AddListener(new UnityAction(() => {
                LOL.inf.enabled = !LOL.inf.enabled;
                LOL.MFDP.PlayInputSound();
                LOL.text1.color = LOL.inf.enabled ? Color.green : Color.red;
            }));
            Debug.Log("listener");
            LOL.text1.color = LOL.inf.enabled ? Color.green : Color.red;

            GameObject emptyButton2 = Instantiate(toCopy, MFDP.displayObj.gameObject.transform);
            RectTransform rt2 = emptyButton2.GetComponent<RectTransform>();
            rt2.localPosition = new Vector3(rt2.localPosition.x - 85, rt.localPosition.y, rt.localPosition.z);
            rt2.localScale = new Vector3(rt2.localScale.x  * 0.85f, rt2.localScale.y * 0.85f, rt2.localScale.z * 0.85f);
            rt2.GetComponentInChildren<Image>().color = Color.black;
            Debug.Log("instantiate");
            VRInteractable interactable2 = emptyButton2.GetComponentInChildren<VRInteractable>();
            Debug.Log("vr interactable");
            text2 = emptyButton2.GetComponentInChildren<Text>();
            Debug.Log("text");
            text2.text = "pairGuns";
            Debug.Log("pairGuns");
            interactable2.OnInteract = new UnityEvent();
            Debug.Log("new UnityEvent()");
            interactable2.interactableName = "Pair All Guns";
            Debug.Log("Pair all guns");
            interactable2.OnInteract.AddListener(new UnityAction(() => {
                LOL.paired.enabled = !LOL.paired.enabled;
                LOL.MFDP.PlayInputSound();
                LOL.text2.color = LOL.paired.enabled? Color.green : Color.red;
            }));
            LOL.text2.color = LOL.paired.enabled ? Color.green : Color.red;
        }

    }
    public void DoMain()
    {
        StartCoroutine(main()); // Literally just starts a coroutine because we can't directly call a IEnumerator, must be void, could've used lambda functions
    }
    private void Start()
    {
        ModLoaded();
    }
    public override void ModLoaded()
    {
        HarmonyInstance.Create("Temperz.EQUIPEVERYTHING").PatchAll();
        VTOLAPI.SceneLoaded += SceneChanged; // So when the scene is changed SceneChanged is called
        VTOLAPI.MissionReloaded += DoMain; // So when the mission is reloaded DoMain is called
        base.ModLoaded();
    }
    private void SceneChanged(VTOLScenes scenes)
    {
        buttonMade = false;
        if (scenes == VTOLScenes.Akutan || scenes == VTOLScenes.CustomMapBase) // If inside of a scene that you can fly in
        {
            StartCoroutine(main());
        }
    }
}
[HarmonyPatch(typeof(LoadoutConfigurator), "EquipCompatibilityMask")]
public static class patch0
{
    public static bool Prefix(HPEquippable equip)
    {
        if (!LOL.buttonMade && VTOLAPI.currentScene == VTOLScenes.VehicleConfiguration)
        {
            GameObject button = GameObject.Instantiate(GameObject.Find("RecenterCanvas"));
            foreach (var controller in GameObject.FindObjectsOfType<VRHandController>())
            {
                if (!controller.isLeft)
                {
                    button.transform.SetParent(controller.transform);
                    button.transform.localPosition = new Vector3(0.101411f, 0.02100047f, -0.128024f);
                    button.transform.localRotation = Quaternion.Euler(-5.834f, 283.583f, 328.957f);
                    button.transform.localScale = new Vector3(button.transform.localScale.x * -1, button.transform.localScale.y * -1, button.transform.localScale.z);
                    VRInteractable bInteractable = button.GetComponentInChildren<VRInteractable>();
                    Text text = button.GetComponentInChildren<Text>();
                    text.transform.localScale = text.transform.localScale * 0.75f;
                    LOL.path = VTOLAPI.GetPlayersVehicleGameObject().GetComponentInChildren<WeaponManager>().resourcePath.ToLower();
                    LOL.path = LOL.path.Remove(0, 9);
                    text.text = "Weapons: " + LOL.path;
                    LOL.text = text;
                    bInteractable.interactableName = "Switch vehicle weapons.";
                    bInteractable.OnInteract = new UnityEngine.Events.UnityEvent();
                    bInteractable.OnInteract.AddListener(new UnityEngine.Events.UnityAction(() =>
                    {
                        if (LOL.path.ToLower() == "abomber")
                            LOL.path = "afighter";
                        else if (LOL.path.ToLower() == "afighter")
                            LOL.path = "asf-30";
                        else if (LOL.path.ToLower() == "asf-30")
                            LOL.path = "asf-33";
                        else if (LOL.path == "asf-33")
                            LOL.path = "ebomber";
                        else if (LOL.path.ToLower() == "ebomber")
                            LOL.path = "eucav";
                        else if (LOL.path.ToLower() == "eucav")
                            LOL.path = "f45a";
                        else if (LOL.path.ToLower() == "f45a")
                            LOL.path = "gav-25";
                        else if (LOL.path.ToLower() == "gav-25")
                            LOL.path = "j4";
                        else if (LOL.path.ToLower() == "j4")
                            LOL.path = "mq-31";
                        else if (LOL.path.ToLower() == "mq-31")
                            LOL.path = "vtol";
                        else if (LOL.path.ToLower() == "vtol")
                            LOL.path = "abomber";
                        LOL.text.text = "Weapons: " + LOL.path;
                        Debug.Log(LOL.path + " is LOL.path");
                        LOL.buttonMade = true;
                        VTOLAPI.GetPlayersVehicleGameObject().GetComponentInChildren<WeaponManager>().resourcePath = "hpequips/" + LOL.path;
                        LoadoutConfigurator config = Resources.FindObjectsOfTypeAll<LoadoutConfigurator>()[0];
                        config.wm.resourcePath = VTOLAPI.GetPlayersVehicleGameObject().GetComponentInChildren<WeaponManager>().resourcePath;
                        Debug.Log(LOL.path);
                        PilotSaveManager.currentVehicle.equipsResourcePath = "hpequips/" + LOL.path;
                        List<string> marsh = new List<string>();
                        List<GameObject> ketkev = new List<GameObject>();
                        Dictionary<string, EqInfo> lol = new Dictionary<string, EqInfo>();
                        foreach (var gameobject in Resources.LoadAll<GameObject>("hpequips/" + LOL.path))
                        {
                            if (!AllowedEquips.allowedEquips.Contains(gameobject.name))
                            {
                                Debug.Log("Unauthorized gameobject " + gameobject.name);
                                continue;
                            }
                            marsh.Add(gameobject.name);
                            ketkev.Add(gameobject);
                            lol.Add(gameobject.name, new EqInfo(gameobject, LOL.path));
                        }
                        config.availableEquipStrings = marsh;
                        PilotSaveManager.currentVehicle.allEquipPrefabs = ketkev;
                        Traverse.Create(config).Field("unlockedWeaponPrefabs").SetValue(new Dictionary<string, EqInfo>());
                        Traverse.Create(config).Field("allWeaponPrefabs").SetValue(new Dictionary<string, EqInfo>());
                        for (int i = 0; i < config.wm.hardpointTransforms.Length; i++)
                        {
                            config.Detach(i);
                        }
                        LOL.configInit = true;
                        config.Initialize(PilotSaveManager.current.GetVehicleSave(PilotSaveManager.currentVehicle.vehicleName).GetCampaignSave(PilotSaveManager.currentCampaign.campaignID), false);
                        if (config.fullInfo != null)
                        {
                            config.fullInfo.CloseInfo();
                        }
                    }));
                    break;
                }
            }
        }
        equip.allowedHardpoints = "0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30";
        equip.unitCost = 0f;
        return true;
    }
}
[HarmonyPatch(typeof(PlayerVehicleSetup), "SetupForFlight")]
public static class patch1
{
    public static bool Prefix(PlayerVehicleSetup __instance)
    {
        __instance.gameObject.GetComponent<WeaponManager>().resourcePath = "hpequips/" + LOL.path;
        return true;
    }
}
[HarmonyPatch(typeof(SMSInternalWeaponAnimator), "UpdateCurrentProfile")]
public static class patch2
{
    public static bool Prefix(SMSInternalWeaponAnimator __instance)
    {
        if (__instance == null)
            Debug.LogWarning("__instance is null");
        return Traverse.Create(__instance).Field("currProfile").GetValue() != null;
    }
}
[HarmonyPatch(typeof(ReArmingPoint), "FinalBeginReArm")]
public static class patch3
{
    public static void Postfix(RearmingUnitSpawn __instance)
    {
        VTOLAPI.GetPlayersVehicleGameObject().GetComponentInChildren<WeaponManager>().resourcePath = "hpequips/" + LOL.path;
        LoadoutConfigurator config = (LoadoutConfigurator)Traverse.Create(__instance).Field("config").GetValue();
        config.wm.resourcePath = VTOLAPI.GetPlayersVehicleGameObject().GetComponentInChildren<WeaponManager>().resourcePath;
        Debug.Log(LOL.path);
        PilotSaveManager.currentVehicle.equipsResourcePath = "hpequips/" + LOL.path;
        List<string> marsh = new List<string>();
        List<GameObject> ketkev = new List<GameObject>();
        Dictionary<string, EqInfo> lol = new Dictionary<string, EqInfo>();
        foreach (var gameobject in Resources.LoadAll<GameObject>("hpequips/" + LOL.path))
        {
            if (!AllowedEquips.allowedEquips.Contains(gameobject.name))
            {
                Debug.Log("Unauthorized gameobject " + gameobject.name);
                continue;
            }
            marsh.Add(gameobject.name);
            ketkev.Add(gameobject);
            lol.Add(gameobject.name, new EqInfo(gameobject, LOL.path));
        }
        config.availableEquipStrings = marsh;
        PilotSaveManager.currentVehicle.allEquipPrefabs = ketkev;
        Traverse.Create(config).Field("unlockedWeaponPrefabs").SetValue(new Dictionary<string, EqInfo>());
        Traverse.Create(config).Field("allWeaponPrefabs").SetValue(new Dictionary<string, EqInfo>());
        LOL.configInit = true;
        config.Initialize(PilotSaveManager.current.GetVehicleSave(PilotSaveManager.currentVehicle.vehicleName).GetCampaignSave(PilotSaveManager.currentCampaign.campaignID), false);
        if (config.fullInfo != null)
        {
            config.fullInfo.CloseInfo();
        }
    }
}
public static class AllowedEquips
{
    public static string[] allowedEquips = {
        "abomber_agm89x2",
        "abomber_mk82AIRRack",
        "abomber_mk82Rack",
        "abomber_mk83Rack",
        "af_aim9",
        "af_amraam",
        "af_amraamRail",
        "af_amraamRailx2",
        "af_dropTank",
        "af_gun",
        "af_maverickx1",
        "af_maverickx3",
        "af_mk82",
        "af_tgp",
        "fa26-cft",
        "fa26_agm89x1",
        "fa26_agm161",
        "fa26_aim9x2",
        "fa26_aim9x3",
        "fa26_cagm-6",
        "fa26_cbu97x1",
        "fa26_droptank",
        "fa26_droptankXL",
        "fa26_gbu12x1",
        "fa26_gbu12x2",
        "fa26_gbu12x3",
        "fa26_gbu38x1",
        "fa26_gbu38x2",
        "fa26_gbu38x3",
        "fa26_gbu39x4uFront",
        "fa26_gbu39x4uRear",
        "fa26_gun",
        "fa26_harmx1",
        "fa26_harmx1dpMount",
        "fa26_iris-t-x1",
        "fa26_iris-t-x2",
        "fa26_iris-t-x3",
        "fa26_maverickx1",
        "fa26_maverickx3",
        "fa26_mk82HDx1",
        "fa26_mk82HDx2",
        "fa26_mk82HDx3",
        "fa26_mk82x2",
        "fa26_mk82x3",
        "fa26_mk83x1",
        "fa26_sidearmx1",
        "fa26_sidearmx2",
        "fa26_sidearmx3",
        "fa26_tgp",
        "h70-x7ld",
        "h70-x7ld-under",
        "h70-x14ld",
        "h70-x14ld-under",
        "asf-srmx1",
        "asf-srmx2",
        "asf-srmx3",
        "asf30_droptank",
        "asf30_gun",
        "asf_mrmDrop",
        "asf_mrmRail",
        "sb1x3",
        "wr-25",
        "af_gun",
        "asf58_droptank",
        "asf58_gun",
        "asf58_mrmx8",
        "asf58_srmx2Left",
        "asf58_srmx2Right",
        "asf_droptank",
        "asf_mrmx6",
        "asf_srmx2Left",
        "ebomber_stdRack",
        "eucav_gun",
        "eucav_hellfire",
        "f45-agm145I",
        "f45-agm145ISide",
        "f45-agm145x3",
        "f45-gbu39",
        "f45-gbu53",
        "f45_agm161",
        "f45_agm161Internal",
        "f45_aim9x1",
        "f45_amraamInternal",
        "f45_amraamRail",
        "f45_droptank",
        "f45_gbu12x1",
        "f45_gbu12x2Internal",
        "f45_gbu38x1",
        "f45_gbu38x2Internal",
        "f45_gbu38x4Internal",
        "f45_gun",
        "f45_mk82Internal",
        "f45_mk82x1",
        "f45_mk82x4Internal",
        "f45_mk83x1",
        "f45_mk83x1Internal",
        "f45_sidewinderx2",
        "asf-srmx1",
        "asf-srmx2",
        "asf-srmx3",
        "gav_gun",
        "gma-6x2",
        "gma-14x3",
        "sb1x3",
        "wr-25",
        "j4_gun",
        "mq31-46lt",
        "agm89x1",
        "av42_gbu12x1",
        "av42_gbu12x2",
        "av42_gbu12x3",
        "cagm-6",
        "cbu97x1",
        "gau-8",
        "gbu38x1",
        "gbu38x2",
        "gbu38x3",
        "gbu39x3",
        "gbu39x4u",
        "h70-4x4",
        "h70-x7",
        "h70-x19",
        "hellfirex4",
        "iris-t-x1",
        "iris-t-x2",
        "iris-t-x3",
        "m230",
        "marmx1",
        "maverickx1",
        "maverickx3",
        "mk82HDx1",
        "mk82HDx2",
        "mk82HDx3",
        "mk82x1",
        "mk82x2",
        "mk82x3",
        "sidearmx1",
        "sidearmx2",
        "sidearmx3",
        "sidewinderx1",
        "sidewinderx2",
        "sidewinderx3"
    };
}
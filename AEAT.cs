using Harmony;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

class AEAT : VTOLMOD
{
    public static bool buttonMade = false;
    public static CampaignSave trueSave = null;
    public static string path = "afighter";
    public static WeaponManager wm;
    public static PlayerVehicle selectedVehicle;
    private static bool patched = false;

    private IEnumerator main()
    {
        yield break;
        while (VTMapManager.fetch == null || !VTMapManager.fetch.scenarioReady || FlightSceneManager.instance.switchingScene)
        {
            yield return null;
        }

        GameObject playerVehicle = VTOLAPI.GetPlayersVehicleGameObject();
        wm = playerVehicle.GetComponent<WeaponManager>();
        //SetupMFDSOld(playerVehicle);
    }

    public static void SetupMFDSOld(GameObject playerVehicle, InfiniteAmmo inf)
    {
        buttonMade = false;
        Debug.Log("AAAAAAAA");
        wm = playerVehicle.GetComponent<WeaponManager>();
        inf.enabled = false;
        MFD lastMFD = null;
        MFDPage mfdPage = null;
        if (VTOLAPI.GetPlayersVehicleEnum() != VTOLVehicles.F45A)
        {
            WeaponManagerUI wmUI = wm.ui;
            AEATDebugLogger.Log("wmUI");
            mfdPage = wmUI.mfdPage;
            if (mfdPage == null)
            {
                Debug.LogError("OOPIES.");
            }
            mfdPage.OnActivatePage.AddListener(delegate
            {
                lastMFD = mfdPage.mfd;
                Debug.Log("=");
                MFDPage.MFDButtonInfo newButton = new MFDPage.MFDButtonInfo();
                newButton.button = MFD.MFDButtons.T4;
                newButton.toolTip = "Toggle infinite ammo";
                newButton.label = "infAmmo";
                newButton.OnPress.AddListener(delegate
                {
                    inf.enabled = !inf.enabled;
                    mfdPage.mfd.buttons[13].GetComponentInChildren<Text>().color = inf.enabled ? Color.green : Color.red;
                });
                if (mfdPage.mfd == null)
                {
                    Debug.LogError("FOUND YA!");
                }
                mfdPage.SetPageButton(newButton);
                mfdPage.mfd.buttons[13].GetComponentInChildren<Text>().color = inf.enabled ? Color.green : Color.red;;
            });
            AEATDebugLogger.Log("Activate page");
            Text mfdButtonText13 = lastMFD.buttons[13].GetComponentInChildren<Text>();
            AEATDebugLogger.Log("Text11");
            mfdPage.OnDeactivatePage.AddListener(delegate
            {
                if (lastMFD != null)
                {
                    mfdButtonText13.color = Color.white;
                    lastMFD = null;
                }
            });
            Debug.Log("AEAT Done.");
        }
        else
        {
            MFDPortalManager MFDP = wm.gameObject.GetComponentInChildren<MFDPortalManager>();
            MFDPStoresManagement MFPDM = (MFDPStoresManagement)MFDP.gameObject.GetComponentInChildren<MFDPortalManager>().pages[5];
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
            GameObject emptyButton = Instantiate(toCopy, MFPDM.displayObj.gameObject.transform);
            RectTransform rt = emptyButton.GetComponent<RectTransform>();
            Text text1 = emptyButton.gameObject.GetComponentInChildren<Text>();
            rt.localPosition = new Vector3(rt.localPosition.x - 50, rt.localPosition.y, rt.localPosition.z);
            rt.localScale = new Vector3(rt.localScale.x * 0.85f, rt.localScale.y * 0.85f, rt.localScale.z * 0.85f);
            rt.GetComponentInChildren<Image>().color = Color.black;
            Debug.Log("instantiate");
            VRInteractable interactable = emptyButton.GetComponentInChildren<VRInteractable>();
            Debug.Log("vr interactable");
            Text text = emptyButton.GetComponentInChildren<Text>();
            Debug.Log("text");
            text.text = "infAmmo";
            Debug.Log("infAmmo");
            interactable.OnInteract = new UnityEvent();
            Debug.Log("new UnityEvent()");
            interactable.interactableName = "Toggle infinite ammo";
            Debug.Log("toggle infinite ammo");
            interactable.OnInteract.AddListener(delegate
            {
                inf.enabled = !inf.enabled;
                MFDP.PlayInputSound();
                text1.color = inf.enabled ? Color.green : Color.red;
            });
            Debug.Log("listener");
            text1.color = inf.enabled ? Color.green : Color.red;

        }
        Debug.Log("AEAT Done setting up MFDS.");
    }

    public static void SetupConfigurator(LoadoutConfigurator config)
    {
        config.availableEquipStrings = EquipConstants.allowedEquips[EquipConstants.GetPathIndex(AEAT.path)].ToList();
        PilotSaveManager.currentVehicle.allEquipPrefabs = EquipConstants.allEquips[EquipConstants.GetPathIndex(AEAT.path)].ToList();
        if (EquipConstants.debug)
            foreach (GameObject prefab in PilotSaveManager.currentVehicle.allEquipPrefabs)
                Debug.Log(prefab.name + " found in allPrefabs.");
        Traverse configTraverse = Traverse.Create(config);
        configTraverse.Field("unlockedWeaponPrefabs").SetValue(new Dictionary<string, EqInfo>());
        configTraverse.Field("allWeaponPrefabs").SetValue(new Dictionary<string, EqInfo>());
        config.lockedHardpoints = new List<int>(); // making sure all hardpoints are unlocked
        for (int i = 0; i < config.wm.hardpointTransforms.Length; i++)
        {
            config.Detach(i);
        }
        PilotSaveManager.currentVehicle.equipsResourcePath = "hpequips/" + AEAT.path;
        config.Initialize(PilotSaveManager.current.GetVehicleSave(PilotSaveManager.currentVehicle.vehicleName).GetCampaignSave(PilotSaveManager.currentCampaign.campaignID), true);
        if (config.fullInfo != null)
        {
            config.fullInfo.CloseInfo();
        }
    }

    private void Update()
    {
        if (!EquipConstants.debug)
            return;
        if (Input.GetKeyDown(KeyCode.K))
        {
            Actor[] allActorsShallow = (Actor[])TargetManager.instance.allActors.ToArray().Clone();
            foreach (Actor actor in allActorsShallow)
                if (actor.team == Teams.Enemy)
                    actor.health?.Kill(); // I did this in korengal valley to make sure that campaign saves worked with different vehicles
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (wm == null)
                wm = VTOLAPI.GetPlayersVehicleGameObject().GetComponent<WeaponManager>();
            if (!wm.isMasterArmed)
                wm.ToggleMasterArmed();
            Loadout loadout = new Loadout();
            loadout.hpLoadout = new string[]
            {
                "gav_gun",
                "mk82x3",
                "asf30_gun",
                "h70-x14ld",
                "fa26_gun",
                "gau-8"
            };
            loadout.cmLoadout = new int[] { 1000, 1000 };
            new CustomLoadout(loadout).equipThisCustomLoadout(wm);
            wm.ToggleMasterArmed();
            Debug.Log("Tried equipping this loadout.");
        }
        if (Input.GetKeyDown(KeyCode.J) && LoadingSceneController.instance != null)
            LoadingSceneController.instance.PlayerReady();
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
        if (patched)
            return;
        patched = true;
        HarmonyInstance.Create("Temperz.EQUIPEVERYTHING").PatchAll();
        VTOLAPI.SceneLoaded += SceneChanged; // So when the scene is changed SceneChanged is called
        VTOLAPI.MissionReloaded += DoMain; // So when the mission is reloaded DoMain is called
        if (EquipConstants.debug)
            Debug.LogWarning("AEAT IN DEBUG MODE, IF THIS IS IN RELEASE PREPARE FOR BADNESS");
        EquipConstants.Load(ModFolder);
        base.ModLoaded();
    }
    private void SceneChanged(VTOLScenes scenes)
    {
        buttonMade = false;
        if (scenes == VTOLScenes.Akutan || scenes == VTOLScenes.CustomMapBase) // If inside of a scene that you can fly in
        {
            StartCoroutine(main());
        }
        else if (scenes == VTOLScenes.ReadyRoom)
        {
            trueSave = null;
            selectedVehicle = null;
        }
    }
}

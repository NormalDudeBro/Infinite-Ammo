using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[HarmonyPatch(typeof(LoadoutConfigurator), "EquipCompatibilityMask")]
public static class Patch_EquipCompatibilityMask
{
    public static bool Prefix(HPEquippable equip)
    {
        if (PilotSaveManager.currentScenario.equipConfigurable == false)
            return true;
        if (!AEAT.buttonMade && VTOLAPI.currentScene == VTOLScenes.VehicleConfiguration)
        {
            foreach (var controller in GameObject.FindObjectsOfType<VRHandController>())
            {
                if (!controller.isLeft)
                {
                    bool alreadyAll = false;

                    GameObject button = GameObject.Instantiate(GameObject.Find("RecenterCanvas"));
                    button.transform.SetParent(controller.transform);
                    button.transform.localPosition = new Vector3(0.101411f, 0.02100047f, -0.128024f);
                    button.transform.localRotation = Quaternion.Euler(-5.834f, 283.583f, 328.957f);
                    button.transform.localScale = new Vector3(button.transform.localScale.x * -1, button.transform.localScale.y * -1, button.transform.localScale.z);
                    VRInteractable bInteractable = button.GetComponentInChildren<VRInteractable>();
                    Text text = button.GetComponentInChildren<Text>();
                    text.transform.localScale = text.transform.localScale * 0.75f;
                    AEAT.path = VTOLAPI.GetPlayersVehicleGameObject().GetComponentInChildren<WeaponManager>().resourcePath.ToLower();
                    AEAT.path = AEAT.path.Remove(0, 9);
                    text.text = "A.E.A.T. Super Menu";
                    bInteractable.interactableName = "Switch vehicle weapons\n (Current Weapons: " + AEAT.path + ")";
                    bInteractable.OnInteract = new UnityEngine.Events.UnityEvent();
                    bInteractable.transform.localPosition = new Vector3(-141f, -62f, -3f);
                    Transform roundButtonBase1 = bInteractable.transform.parent.Find("roundButtonBase");
                    roundButtonBase1.localPosition = new Vector3(-141f, -62f, -3f);
                    LoadoutConfigurator config = Resources.FindObjectsOfTypeAll<LoadoutConfigurator>()[0];
                    VehicleConfigSceneSetup setup = Resources.FindObjectsOfTypeAll<VehicleConfigSceneSetup>()[0];
                    Traverse setupHelper = Traverse.Create(setup);
                    Traverse configTraverse = Traverse.Create(config);
                    WeaponManager wm = VTOLAPI.GetPlayersVehicleGameObject().GetComponentInChildren<WeaponManager>();
                    bInteractable.OnInteract.AddListener(delegate
                    {
                        if (config == null)
                        {
                            config = Resources.FindObjectsOfTypeAll<LoadoutConfigurator>()[0];
                            configTraverse = Traverse.Create(config);
                        }

                        do
                            for (int i = 0; i < EquipConstants.allPaths.Length; i++)
                            {
                                if (EquipConstants.allPaths[i] == AEAT.path)
                                {
                                    if (i == EquipConstants.allPaths.Length - 1)
                                        i = -1;
                                    AEAT.path = EquipConstants.allPaths[i + 1];
                                    break;
                                }
                            }
                        while (AEAT.path == "ah-94" && !Steamworks.SteamApps.IsDlcInstalled(1770480));

                        bInteractable.interactableName = "Switch vehicle weapons\n (Current Weapons: " + AEAT.path + ")";
                        Debug.Log(AEAT.path + " is AEAT.path");
                        wm.resourcePath = "hpequips/" + AEAT.path;
                        config.wm.resourcePath = wm.resourcePath;
                        AEAT.SetupConfigurator(config);
                        alreadyAll = false;
                    });
                    Debug.Log("Made right hand bottom button.");


                    button = GameObject.Instantiate(bInteractable.gameObject, bInteractable.transform.parent);
                    Debug.Log("Current vehicle name is " + PilotSaveManager.currentVehicle.name);
                    button.transform.localPosition = new Vector3(147f, -62f, -3f);
                    VRInteractable bInteractable2 = button.GetComponent<VRInteractable>();
                    bInteractable2.interactableName = "Switch Vehicles";
                    bInteractable2.OnInteract = new UnityEvent();
                    foreach (var vehicle in VTResources.GetPlayerVehicles())
                    {
                        Debug.Log(vehicle.name);
                        Debug.Log(vehicle.vehicleName);
                    }
                    bInteractable2.OnInteract.AddListener(delegate
                    {
                        if (PilotSaveManager.currentVehicle.name == "AV-42C")
                        {
                            PilotSaveManager.currentVehicle = VTResources.GetPlayerVehicle("F/A-26B");
                        }
                        else if (PilotSaveManager.currentVehicle.name == "FA-26B")
                        {
                            PilotSaveManager.currentVehicle = VTResources.GetPlayerVehicle("F-45A");
                        }
                        else
                        {
                            PilotSaveManager.currentVehicle = VTResources.GetPlayerVehicle("AV-42C");
                        }
                        //text2.text = PilotSaveManager.currentVehicle.vehicleName;
                        AEAT.selectedVehicle = PilotSaveManager.currentVehicle;
                        if (VTOLAPI.currentScene == VTOLScenes.VehicleConfiguration)
                        {
                            AEAT.buttonMade = false;
                            Debug.Log("Resetting up scene.");
                            SceneManager.LoadScene("VehicleConfiguration");
                        }
                    });
                    Transform bigButton2 = GameObject.Instantiate(roundButtonBase1, roundButtonBase1.parent);
                    bigButton2.localPosition = bInteractable2.transform.localPosition;
                    AEAT.trueSave = PilotSaveManager.current.GetVehicleSave(PilotSaveManager.currentVehicle.vehicleName).GetCampaignSave(PilotSaveManager.currentCampaign.campaignID);
                    Debug.Log("Made right hand top button, and true save's id is " + AEAT.trueSave.campaignID);

                    button = GameObject.Instantiate(bInteractable.gameObject, bInteractable.transform.parent);
                    Debug.Log("Current vehicle name is " + PilotSaveManager.currentVehicle.name);
                    button.transform.localPosition = new Vector3(0.101411f, -62f, -3f);
                    VRInteractable bInteractable3 = button.GetComponent<VRInteractable>();
                    bInteractable3.interactableName = "Super Button";
                    bInteractable3.OnInteract = new UnityEvent();
                    bInteractable3.OnInteract.AddListener(delegate
                    {
                        if (alreadyAll)
                            return;
                        alreadyAll = true;
                        bInteractable.interactableName = "Switch vehicle weapons\n (Current Weapons: All)";
                        if (config == null)
                        {
                            config = Resources.FindObjectsOfTypeAll<LoadoutConfigurator>()[0];
                            configTraverse = Traverse.Create(config);
                        }
                        Dictionary<string, EqInfo> allEquips = new Dictionary<string, EqInfo>();
                        for (int i = 0; i < EquipConstants.allPaths.Length; i++)
                        {
                            foreach (GameObject eq in EquipConstants.allEquips[i])
                            {
                                if (!allEquips.ContainsKey(eq.name))
                                {
                                    GameObject equipObject = GameObject.Instantiate(eq);
                                    equipObject.name = eq.name;
                                    equipObject.SetActive(false);
                                    allEquips.Add(eq.name, new EqInfo(equipObject, "hpequips/" + EquipConstants.allPaths[i] + "/" + eq.name));
                                }
                            }
                        }
                        configTraverse.Field("unlockedWeaponPrefabs").SetValue(allEquips);
                        configTraverse.Field("allWeaponPrefabs").SetValue(allEquips);
                        config.lockedHardpoints = new List<int>(); // making sure all hardpoints are unlocked
                        for (int i = 0; i < config.wm.hardpointTransforms.Length; i++)
                        {
                            config.Detach(i);
                        }
                        PilotSaveManager.currentVehicle.equipsResourcePath = "hpequips/" + AEAT.path;
                        //config.Initialize(PilotSaveManager.current.GetVehicleSave(PilotSaveManager.currentVehicle.vehicleName).GetCampaignSave(PilotSaveManager.currentCampaign.campaignID), false);
                        if (config.fullInfo != null)
                        {
                            config.fullInfo.CloseInfo();
                        }
                    });
                    Transform bigButton3 = GameObject.Instantiate(roundButtonBase1, roundButtonBase1.parent);
                    bigButton3.localPosition = bInteractable3.transform.localPosition;
                    bigButton3.Find("roundButton").gameObject.GetComponent<Renderer>().material.color = new Color(255f, 231f, 10f);
                    Debug.Log("Made super button.");
                    break;
                }
            }
            AEAT.buttonMade = true;
        }
        equip.allowedHardpoints = "0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30";
        equip.unitCost = 0f;
        return true;
    }
}

[HarmonyPatch(typeof(LoadoutConfigurator), "Attach")]
public static class Debug_Attach
{
    public static bool Prefix(ref string weaponName, LoadoutConfigurator __instance)
    {
        if (!EquipConstants.debug)
            return true;
        Dictionary<string, EqInfo> allPrefabs = (Dictionary<string, EqInfo>)Traverse.Create(__instance).Field("allWeaponPrefabs").GetValue();
        if (!allPrefabs.ContainsKey(weaponName))
        {
            AEATDebugLogger.Log("allPrefabs does not contain " + weaponName + ".", LogType.Error);
            return false;
        }
        else
            AEATDebugLogger.Log("allPrefabs contains " + weaponName);
        return true;
    }
}

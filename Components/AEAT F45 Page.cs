using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;
using Harmony;

public class AEATF45MFDPage : MonoBehaviour
{
    private void Awake()
    {
        AEATDebugLogger.Log("Awake F45 AEAT Page");
        GameObject playerVehicle = VTOLAPI.GetPlayersVehicleGameObject();
        if (playerVehicle == null)
            return;
        page = this.GetComponent<MFDPortalPage>();
        OnEnable();
    }
    private void OnEnable()
    {
        if (!addedPage)
        {
            AEATDebugLogger.Log("Do AEAT Page buttons");
            GameObject playerVehicle = VTOLAPI.GetPlayersVehicleGameObject();
            if (playerVehicle == null)
                return;
            inf = playerVehicle.AddComponent<InfiniteAmmo>();
            Image infAmmoImage = transform.Find("display Obj").Find("Infinite Ammo").Find("bg").GetComponent<Image>();
            inf.enabled = false;
            foreach (VRInteractable interactable in GetComponentsInChildren<VRInteractable>())
            {
                if (interactable.interactableName == "Infinite Ammo")
                    interactable.OnInteract.AddListener(delegate
                    {
                        inf.enabled = !inf.enabled;
                        if (inf.enabled)
                            infAmmoImage.color = new Color(0, 69, 0);
                        else
                            infAmmoImage.color = new Color(69, 0, 0);
                        page.quarter.half.manager.PlayInputSound();
                    });
            }
            addedPage = true;
        }
    }

    private bool addedPage = false;
    private InfiniteAmmo inf;
    private MFDPortalPage page;
}

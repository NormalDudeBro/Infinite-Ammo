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
            pGuns = playerVehicle.AddComponent<PairedGuns>();
            Image pGunsImage = transform.Find("display Obj").Find("Paired Guns").Find("bg").GetComponent<Image>();
            rpm = playerVehicle.AddComponent<BigRPM>();
            Image rpmImage = transform.Find("display Obj").Find("Big RPM").Find("bg").GetComponent<Image>();
            inf.enabled = false;
            pGuns.enabled = false;
            rpm.enabled = false;
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
                else if (interactable.interactableName == "Paired Guns")
                    interactable.OnInteract.AddListener(delegate
                    {
                        pGuns.enabled = !pGuns.enabled;
                        if (pGuns.enabled)
                            pGunsImage.color = new Color(0, 69, 0);
                        else
                            pGunsImage.color = new Color(69, 0, 0);
                        page.quarter.half.manager.PlayInputSound();
                    });
                else
                    interactable.OnInteract.AddListener(delegate
                    {
                        rpm.enabled = !rpm.enabled;
                        if (rpm.enabled)
                            rpmImage.color = new Color(0, 69, 0);
                        else
                            rpmImage.color = new Color(69, 0, 0);
                        page.quarter.half.manager.PlayInputSound();
                    });
            }
            addedPage = true;
        }
    }

    private bool addedPage = false;
    private InfiniteAmmo inf;
    private PairedGuns pGuns;
    private BigRPM rpm;
    private MFDPortalPage page;
}
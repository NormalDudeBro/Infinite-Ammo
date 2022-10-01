using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;
using Harmony;

public class AEATMFDPage : MonoBehaviour
{
    private void Awake()
    {
        AEATDebugLogger.Log("Awake AEAT Page");
        GameObject playerVehicle = VTOLAPI.GetPlayersVehicleGameObject();
        if (playerVehicle == null)
            return;
        AEATDebugLogger.Log("Do AEAT Page buttons");
        inf = playerVehicle.AddComponent<InfiniteAmmo>();
        Image infAmmoImage = transform.Find("Infinite Ammo").Find("bg").GetComponent<Image>();
        inf.enabled = false;
        page = GetComponent<MFDPage>();
        MFDPage.MFDButtonInfo infButton = new MFDPage.MFDButtonInfo();
        infButton.button = MFD.MFDButtons.L3;
        infButton.toolTip = "Guns and missiles don't deplete";
        infButton.label = "Infinite Ammo";
        infButton.OnPress.AddListener(delegate
        {
            inf.enabled = !inf.enabled;
            if (inf.enabled)
                infAmmoImage.color = new Color(0, 69, 0);
            else
                infAmmoImage.color = new Color(69, 0, 0);
        });
        page.buttons = new MFDPage.MFDButtonInfo[] { infButton };
        //page.buttons.AddToArray(infButton);
        //page.buttons.AddToArray(pGunsButton);
        //page.buttons.AddToArray(rpmButton);
        addedPage = true;
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
            Image infAmmoImage = transform.Find("Infinite Ammo").Find("bg").GetComponent<Image>();
            inf.enabled = false;
            page = GetComponent<MFDPage>();
            MFDPage.MFDButtonInfo infButton = new MFDPage.MFDButtonInfo();
            infButton.button = MFD.MFDButtons.L3;
            infButton.toolTip = "Guns and missiles don't deplete";
            infButton.label = "Infinite Ammo";
            infButton.OnPress.AddListener(delegate
            {
                inf.enabled = !inf.enabled;
                if (inf.enabled)
                    infAmmoImage.color = new Color(0, 69, 0);
                else
                    infAmmoImage.color = new Color(69, 0, 0);
            });
            page.buttons = new MFDPage.MFDButtonInfo[] { infButton };
            //page.buttons.AddToArray(infButton);
            //page.buttons.AddToArray(pGunsButton);
            //page.buttons.AddToArray(rpmButton);
            addedPage = true;
        }
    }

    private bool addedPage = false;
    private InfiniteAmmo inf;
    private MFDPage page;
}

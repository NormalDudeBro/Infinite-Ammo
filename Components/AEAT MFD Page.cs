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
        pGuns = playerVehicle.AddComponent<PairedGuns>();
        Image pGunsImage = transform.Find("Paired Guns").Find("bg").GetComponent<Image>();
        rpm = playerVehicle.AddComponent<BigRPM>();
        Image rpmImage = transform.Find("Big RPM").Find("bg").GetComponent<Image>();
        inf.enabled = false;
        pGuns.enabled = false;
        rpm.enabled = false;
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
        MFDPage.MFDButtonInfo pGunsButton = new MFDPage.MFDButtonInfo();
        pGunsButton.button = MFD.MFDButtons.L4;
        pGunsButton.toolTip = "If one gun fires, all guns fire";
        pGunsButton.label = "Paired Guns";
        pGunsButton.OnPress.AddListener(delegate
        {
            pGuns.enabled = !pGuns.enabled;
            if (pGuns.enabled)
                pGunsImage.color = new Color(0, 69, 0);
            else
                pGunsImage.color = new Color(69, 0, 0);
        });
        MFDPage.MFDButtonInfo rpmButton = new MFDPage.MFDButtonInfo();
        rpmButton.button = MFD.MFDButtons.L5;
        rpmButton.toolTip = "Guns shoot really fast";
        rpmButton.label = "Big RPM";
        rpmButton.OnPress.AddListener(delegate
        {
            rpm.enabled = !rpm.enabled;
            if (rpm.enabled)
                rpmImage.color = new Color(0, 69, 0);
            else
                rpmImage.color = new Color(69, 0, 0);
        });
        page.buttons = new MFDPage.MFDButtonInfo[] { infButton, pGunsButton, rpmButton };
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
            pGuns = playerVehicle.AddComponent<PairedGuns>();
            Image pGunsImage = transform.Find("Paired Guns").Find("bg").GetComponent<Image>();
            rpm = playerVehicle.AddComponent<BigRPM>();
            Image rpmImage = transform.Find("Big RPM").Find("bg").GetComponent<Image>();
            inf.enabled = false;
            pGuns.enabled = false;
            rpm.enabled = false;
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
            MFDPage.MFDButtonInfo pGunsButton = new MFDPage.MFDButtonInfo();
            pGunsButton.button = MFD.MFDButtons.L4;
            pGunsButton.toolTip = "If one gun fires, all guns fire";
            pGunsButton.label = "Paired Guns";
            pGunsButton.OnPress.AddListener(delegate
            {
                pGuns.enabled = !pGuns.enabled;
                if (pGuns.enabled)
                    pGunsImage.color = new Color(0, 69, 0);
                else
                    pGunsImage.color = new Color(69, 0, 0);
            });
            MFDPage.MFDButtonInfo rpmButton = new MFDPage.MFDButtonInfo();
            rpmButton.button = MFD.MFDButtons.L5;
            rpmButton.toolTip = "Guns shoot really fast";
            rpmButton.label = "Big RPM";
            rpmButton.OnPress.AddListener(delegate
            {
                rpm.enabled = !rpm.enabled;
                if (rpm.enabled)
                    rpmImage.color = new Color(0, 69, 0);
                else
                    rpmImage.color = new Color(69, 0, 0);
            });
            page.buttons = new MFDPage.MFDButtonInfo[] { infButton, pGunsButton, rpmButton };
            //page.buttons.AddToArray(infButton);
            //page.buttons.AddToArray(pGunsButton);
            //page.buttons.AddToArray(rpmButton);
            addedPage = true;
        }
    }

    private bool addedPage = false;
    private InfiniteAmmo inf;
    private PairedGuns pGuns;
    private BigRPM rpm;
    private MFDPage page;
}
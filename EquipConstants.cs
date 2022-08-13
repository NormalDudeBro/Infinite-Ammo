using Harmony;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class EquipConstants // THIS IS A THICC BOI
{
    public const bool debug = true;

    public static bool isAllowed(string name)
    {
        foreach (string[] strArr in allowedEquips)
            foreach (string str in strArr)
                if (str == name)
                    return true;
        return false;
    }

    public static int GetPathIndex(string path)
    {
        for (int i = 0; i < allPaths.Length; i++)
            if (allPaths[i] == path)
                return i;
        Debug.Log(path = " not found as an index.");
        return -1;
    }

    public static void Load(string path)
    {
        AssetBundle mfdPages = AssetBundle.LoadFromFile(path + "/aeat.mfdpages");
        AEATMFDPage = (GameObject)mfdPages.LoadAsset("AEAT Page.prefab");
        AEATF45MFDPage = (GameObject)mfdPages.LoadAsset("AEAT F45 Page.prefab");
        allEquips = new GameObject[allPaths.Length][];
        for (int i = 0; i < allPaths.Length; i++)
        {
            List<GameObject> allObjects = new List<GameObject>();
            foreach (string allowedEquip in allowedEquips[i])
            {
                GameObject equip = (GameObject)Resources.Load("hpequips/" + allPaths[i] + "/" + allowedEquip);
                if (equip == null)
                    Debug.LogError(allowedEquip + " is null.");
                else
                    allObjects.Add(equip);
                AEATDebugLogger.Log("Tried adding " + allowedEquip + " to list.");
            }
            allEquips[i] = allObjects.ToArray();
        }
        if (debug)
        {
            foreach (GameObject[] objectArr in allEquips)
                foreach (GameObject obj in objectArr)
                    Debug.Log(obj.name + " is in allEquips.");
        }
    }

    public static GameObject AEATMFDPage = null;
    public static GameObject AEATF45MFDPage = null;
    public static GameObject[][] allEquips = null;
    public static string[][] allowedEquips = new string[][]
    {
       new string[] // abomber
       {
           "abomber_agm89x2",
           "abomber_mk82AIRRack",
           "abomber_mk82Rack",
           "abomber_mk83Rack"
       },
       new string[] // fa26
       {
            "af_aim9",
            "af_amraam",
            "af_amraamRail",
            "af_amraamRailx2",
            "af_dropTank",
            "af_maverickx1",
            "af_maverickx3",
            "af_mk82",
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
            "h70-x7ld-under",
            "h70-x7ld",
            "h70-x14ld-under",
            "h70-x14ld"
       },
       new string[] // ah94 
       {

       },
       new string[] // asf30
       {
           "asf-srmx1",
           "asf-srmx2",
           "asf-srmx3",
           "asf30_droptank",
           "asf30_gun",
           "asf_mrmDrop",
           "asf_mrmRail",
           "sb1x3",
           "wr-25"
       },
       new string[] // asf33
       {
           "af_gun",
           "asf58_droptank",
           "asf58_gun",
           "asf58_mrmx8",
           "asf58_srmx2Left",
           "asf58_srmx2Right",
           "asf_droptank",
           "asf_mrmx6",
           "asf_srmx2Left"
       },
       new string[] //ebomber
       {
           "ebomber_stdRack"
       },
       new string[] // eucav
       {
           "eucav_gun",
           "eucav_hellfire"
       },
       new string[] // f45
       {
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
            "f45_sidewinderx2"
       },
       new string[]  //gav25
       {
           "asf-srmx1",
           "asf-srmx2",
           "asf-srmx3",
           "gav_gun",
           "gma-6x2",
           "gma-14x3",
           "sb1x3",
           "wr-25"
       },
       new string[] //j4
       {
           "j4_gun"
       },
       new string[] // mq31
       {
           "mq31-46lt"
       },
       new string[]  // av42
       {
           "agm89x1",
           "av42_agm161",
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
       }
    };
    public static string[] allPaths =
    {
        "abomber",
        "afighter",
        "ah-94",
        "asf-30",
        "asf-33",
        "ebomber",
        "eucav",
        "f45a",
        "gav-25",
        "j4",
        "mq-31",
        "vtol"
    };
}
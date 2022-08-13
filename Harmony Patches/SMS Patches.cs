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

[HarmonyPatch(typeof(SMSInternalWeaponAnimator), "UpdateCurrentProfile")]
public static class patch2
{
    public static bool Prefix(SMSInternalWeaponAnimator __instance)
    {
        return Traverse.Create(__instance).Field("currProfile").GetValue() != null;
    }
}
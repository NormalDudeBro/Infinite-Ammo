using System;
using UnityEngine;

public static class AEATDebugLogger
{
    public static void Log(string message, LogType severity = LogType.Log, Exception e = null)
    {
        if (!EquipConstants.debug)
            return;
        if (severity == LogType.Log)
            Debug.Log(message);
        else if (severity == LogType.Error)
            Debug.Log(message);
        else if (severity == LogType.Exception)
        {
            Debug.Log(message);
            if (e != null)
                Debug.LogException(e);
            else
                throw new NullReferenceException("You tried to debug a null exception good job idiot");
        }
    }
}

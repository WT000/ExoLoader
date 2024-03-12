using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ExoLoader
{
    [BepInEx.BepInPlugin("ExoLoaderInject", "ExoLoader", "1.0")]
    public class Injector : BaseUnityPlugin
    {
        public void Awake()
        {
            Logger.LogInfo("Doing ExoLoader patches...");
            var harmony = new Harmony("ExoLoader");
            harmony.PatchAll();
            Logger.LogInfo("ExoLoader patches done.");
            ModInstance.instance = this;
        }

        public void Log(string message)
        {
            Logger.LogInfo(message);
        }
    }
}

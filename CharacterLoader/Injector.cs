using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CharacterLoader
{
    [BepInEx.BepInPlugin("CharacterLoaderInject","Character Loader","0.0.1")]
    public class Injector : BaseUnityPlugin
    {
        public void Awake()
        {
            Logger.LogInfo("Doing CharacterLoader patches...");
            var harmony = new Harmony("CharacterLoader");
            harmony.PatchAll();
            Logger.LogInfo("CharacterLoader patches done.");
            ModInstance.instance = this;
        }

        public void Log(string message)
        {
            Logger.LogInfo(message);
        }
    }
}

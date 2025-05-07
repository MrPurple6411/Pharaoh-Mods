using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;

namespace EternalAbodes
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {        // Create a logger instance for the plugin
        internal static ManualLogSource Log;
        
        // Configuration entries
        public static ConfigEntry<bool> PreventDevolution;
        public static ConfigEntry<int> HousingCapacityMultiplier;
        
        // Harmony instance for patching
        private Harmony _harmony;

        private void Awake()
        {
            // Initialize logger
            Log = Logger;
            Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} is loaded!");            // Initialize configuration options
            PreventDevolution = Config.Bind("Housing", "PreventDevolution", true, 
                "If true, houses will not devolve under normal conditions");
            
            HousingCapacityMultiplier = Config.Bind("Housing", "CapacityMultiplier", 1,
                "Multiplier for housing capacity (1 = vanilla, 2 = double capacity, etc.)",
                new BepInEx.Configuration.AcceptableValueRange<int>(1, 10));
            
            // Apply Harmony patches
            _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
            
            Log.LogInfo($"{MyPluginInfo.PLUGIN_NAME} initialized with Harmony patches applied");
        }

        private void OnDestroy()
        {
            // Clean up harmony patches when the plugin is unloaded
            _harmony?.UnpatchSelf();
            Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} has been unloaded!");
        }
    }
}

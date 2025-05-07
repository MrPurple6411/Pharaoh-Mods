using HarmonyLib;
using System;
using UnityEngine;

namespace EternalAbodes.Patches
{
    /// <summary>
    /// All patches targeting the House class
    /// </summary>
    [HarmonyPatch(typeof(House))]
    public static class House_Patches
    {    /// <summary>
         /// Patch to apply housing capacity multiplier
         /// </summary>
        [HarmonyPatch(nameof(House.GetMaxOccupancy)), HarmonyPostfix]
        public static void GetMaxOccupancy_Postfix(ref int __result)
        {
            // Apply the housing capacity multiplier from config
            int multiplier = Plugin.HousingCapacityMultiplier.Value;

            // Safety check to ensure multiplier is at least 1
            if (multiplier < 1)
            {
                Plugin.Log.LogWarning($"Housing capacity multiplier was set to an invalid value ({multiplier}). Changing config to 1.");
                Plugin.HousingCapacityMultiplier.Value = 1; // Update the actual config value
                multiplier = 1;
            }

            // Only modify if multiplier is different from default value of 1
            if (multiplier != 1)
            {
                int originalCapacity = __result;
                __result = originalCapacity * multiplier;

                // Log the capacity change if debugging is enabled
                if (Plugin.Log.IsLogLevelEqualOrHigher(BepInEx.Logging.LogLevel.Debug))
                {
                    Plugin.Log.LogDebug($"Housing capacity changed: {originalCapacity} → {__result} (multiplier: {multiplier})");
                }
            }
        }
        /// <summary>
        /// Patch to prevent house devolution
        /// </summary>
        [HarmonyPatch(nameof(House.ShouldDowngrade)), HarmonyPostfix]
        public static void ShouldDowngrade_Postfix(ref bool __result)
        {
            // If devolution prevention is enabled, force the method to return false
            bool preventDevolution = Plugin.PreventDevolution.Value;

            if (preventDevolution)
            {
                if (Plugin.Log.IsLogLevelEqualOrHigher(BepInEx.Logging.LogLevel.Debug))
                {
                    Plugin.Log.LogDebug("House devolution prevented by Eternal Abodes");
                }

                // Set result to false to prevent devolution
                __result = false;
            }
        }

        /// <summary>
        /// Patch to prevent uncontrolled house evolution when devolution prevention is enabled
        /// </summary>
        [HarmonyPatch(nameof(House.ShouldUpgrade)), HarmonyPrefix]
        public static bool ShouldUpgrade_Prefix(House __instance, ref bool __result)
        {
            // Only need to perform this check if devolution prevention is enabled
            bool preventDevolution = Plugin.PreventDevolution.Value;

            if (!preventDevolution)
            {
                // If devolution prevention is disabled, let the original method run
                return true;
            }

            // Get the current house level
            HouseLevel currentLevel = __instance.CurrentHouseLevel;

            // For each level below the current one, check if conditions are valid
            for (int i = 0; i < (int)currentLevel; i++)
            {
                // If conditions aren't valid for a lower level, prevent upgrade
                if (!__instance.HasValidConditions((HouseLevel)i, __instance._levelManager.ActiveServiceInDays))
                {
                    __result = false;
                    return false; // Skip original method
                }
            }

            // All lower levels have valid conditions, let the original method run
            return true;
        }
    }
}

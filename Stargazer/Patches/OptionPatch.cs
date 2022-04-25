using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine;
using System.Linq;

namespace Stargazer.Patches
{
    [HarmonyPatch(typeof(GameOptionsData), nameof(GameOptionsData.ToggleMapFilter))]
    public static class GameOptionsData_ToggleMapFilter_Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(GameOptionsData __instance, [HarmonyArgument(0)] byte mapId)
        {
            byte b = (byte)(((int)__instance.MapId ^ 1 << (int)mapId) & 0b110111);
            if (b != 0)
            {
                __instance.MapId = b;
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(GameSettingMenu), nameof(GameSettingMenu.InitializeOptions))]
    public static class GameSettingMenuInitializePatch
    {
        public static void Prefix(GameSettingMenu __instance)
        {
            var defaultTransform = __instance.AllItems.FirstOrDefault(x => x.gameObject.activeSelf && x.name.Equals("ResetToDefault", StringComparison.OrdinalIgnoreCase));
            if (defaultTransform != null)
                __instance.HideForOnline = new Transform[] { defaultTransform };
            else
                __instance.HideForOnline = new Transform[] { };
        }
    }
}

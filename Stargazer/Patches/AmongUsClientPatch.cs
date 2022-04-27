using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using HarmonyLib;

namespace Stargazer.Patches
{
    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.Awake))]
    public static class AmongUsClientAwakePatch
    {
        [HarmonyPrefix]
        public static void Postfix(AmongUsClient __instance)
        {
            Module.CustomSystemTypes.LoadVanillaSystemTypes();
            Module.CustomTaskTypes.LoadVanillaTaskTypes();
            
            Assets.MapAssets.LoadAssets(__instance);
            Map.Builder.Task.TaskBuilder.LoadVanillaTaskBuilders();
            Map.Builder.CustomDoorType.LoadVanillaDoorType();

            /* ここに追加マップ読み込み部を入れる */

            Map.AdditionalMapManager.AddPrefabs(__instance);
        }
    }
}

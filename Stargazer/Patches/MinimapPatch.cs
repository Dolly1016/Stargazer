using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace Stargazer.Patches
{
    [HarmonyPatch(typeof(MapBehaviour), nameof(MapBehaviour.Awake))]
    public static class MinimapEnablePatch
    {
        public static void Postfix(MapBehaviour __instance)
        {
            if (!__instance.countOverlay) return;

            var pool = __instance.countOverlay.gameObject.GetComponent<ObjectPoolBehavior>();

            foreach(var ca in __instance.countOverlay.CountAreas) {
                ca.pool = pool;
            }
        }
    }

}

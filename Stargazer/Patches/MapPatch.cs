using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using HarmonyLib;

namespace Stargazer.Patches
{
    public static class MapPatch
    {
        public static void SetupCustomMap(Map.Blueprint blueprint,ShipStatus __instance)
        {
            if (blueprint.RequirePlainMap)
            {
                Map.MapReformer.Dismantle(__instance);
                Map.MapReformer.CreateStandardMap(__instance);
                Map.MapReformer.CreateDefaultSystemTypes(__instance);
            }

            blueprint.PreBuild(blueprint, __instance, __instance.transform);
            blueprint.PostBuild(blueprint, __instance, __instance.transform);

            __instance.AssignTaskIndexes();
        }

        public static bool Prefix(ShipStatus __instance)
        {
            //初期化
            Behaviours.CustomShipStatus.Instance = null;

            if (!Helpers.PlayingModMap()) return true;

            var blueprint = Map.AdditionalMapManager.GetBlueprint(PlayerControl.GameOptions.MapId);
            if (blueprint == null) return true;
            if (!blueprint.RequirePlainMap) return true;

            SetupCustomMap(blueprint,__instance);
            ShipStatus.Instance = __instance;

            return false;
        }

        public static void Postfix(ShipStatus __instance)
        {
            if (!Helpers.PlayingModMap()) return;

            var blueprint = Map.AdditionalMapManager.GetBlueprint(PlayerControl.GameOptions.MapId);
            if (blueprint == null) return;
            if (blueprint.RequirePlainMap) return;

            SetupCustomMap(blueprint, __instance);
        }
    }

    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.OnEnable))]
    public static class SkeldAndMiraMapPatch
    {
        public static bool Prefix(ShipStatus __instance) => MapPatch.Prefix(__instance);
        public static void Postfix(ShipStatus __instance) => MapPatch.Postfix(__instance);
    }

    [HarmonyPatch(typeof(PolusShipStatus), nameof(PolusShipStatus.OnEnable))]
    public static class PolusMapPatch
    {
        public static bool Prefix(PolusShipStatus __instance) => MapPatch.Prefix(__instance);
        public static void Postfix(PolusShipStatus __instance) => MapPatch.Postfix(__instance);
    }

    [HarmonyPatch(typeof(AirshipStatus), nameof(AirshipStatus.OnEnable))]
    public static class AirshipMapPatch
    {
        public static bool Prefix(AirshipStatus __instance) => MapPatch.Prefix(__instance);
        public static void Postfix(AirshipStatus __instance) => MapPatch.Postfix(__instance);
    }
}

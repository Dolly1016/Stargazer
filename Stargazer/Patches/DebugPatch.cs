using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;

namespace Stargazer.Patches
{
    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Update))]
    public class IgnoreMinPlayerPatch
    {
        public static void Postfix(GameStartManager __instance)
        {
            if (AmongUsClient.Instance.AmHost) __instance.MinPlayers = 0;
            __instance.StartButton.color = __instance.startLabelText.color = Palette.EnabledColor;
        }
    }

    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.CheckEndCriteria))]
    class IgnoreEndCriteriaPatch
    {
        public static bool Prefix(ShipStatus __instance)
        {
            return false;
        }
    }
}
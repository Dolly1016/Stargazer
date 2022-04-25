using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using Hazel;
using UnityEngine;


namespace Stargazer.Patches
{
    //はしご昇降の処理を書きかえ
    [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.HandleRpc))]
    public static class PlayerPhysicsRPCPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(PlayerPhysics __instance, [HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader)
        {
            if (!Helpers.InModMap()) return true;
            if (callId != 31) return true;

            byte ladderId = reader.ReadByte();
            byte climbLadderSid = reader.ReadByte();

            var ladders = Behaviours.CustomShipStatus.Instance.Ladders;
            __instance.ClimbLadder(ladders[ladderId], climbLadderSid);

            return false;
        }
    }
}

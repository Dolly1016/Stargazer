using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine;
using Assets.CoreScripts;

namespace Stargazer.Patches
{
    public static class TaskPatches
    {
        public static void InitializePrefix(NormalPlayerTask __instance)
        {
            var opt = __instance.gameObject.GetComponent<Behaviours.NormalPlayerTaskOption>();
            if (!opt) return;
            opt.Initialize();
        }
    }

    [HarmonyPatch(typeof(NormalPlayerTask), nameof(NormalPlayerTask.Initialize))]
    public static class TaskInitializePatch
    {
        public static void Prefix(NormalPlayerTask __instance) {
            TaskPatches.InitializePrefix(__instance);
        }
    }

    [HarmonyPatch(typeof(Console), nameof(Console.Use))]
    public static class ConsoleUsePatch
    {
        public static bool Prefix(Console __instance)
        {
            if (!Helpers.InModMap()) return true;

            bool flag;
            bool flag2;
            __instance.CanUse(PlayerControl.LocalPlayer.Data, out flag, out flag2);
            if (!flag)
            {
                return false;
            }
            PlayerControl localPlayer = PlayerControl.LocalPlayer;
            PlayerTask playerTask = __instance.FindTask(localPlayer);
            if (playerTask.MinigamePrefab)
            {
                Minigame minigame = UnityEngine.Object.Instantiate<Minigame>(playerTask.GetMinigamePrefab());
                minigame.transform.SetParent(Camera.main.transform, false);
                minigame.transform.localPosition = new Vector3(0f, 0f, -50f);
                minigame.Console = __instance;

                var opt = playerTask.gameObject.GetComponent<Behaviours.NormalPlayerTaskOption>();
                if (opt) Behaviours.CustomShipStatus.Instance.TaskActions.RunPrebeginReformer(opt.PrebeginReformerId,minigame);

                minigame.Begin(playerTask);
                DestroyableSingleton<Telemetry>.Instance.WriteUse(localPlayer.PlayerId, playerTask.TaskType, __instance.transform.position);
            }

            return false;
        }
    }

}

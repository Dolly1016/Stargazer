using System;
using UnityEngine;
using HarmonyLib;
using BepInEx;
using BepInEx.IL2CPP;

namespace Stargazer
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInProcess("Among Us.exe")]
    public class StargazerPlugin : BasePlugin
    {
        public const string AmongUsVersion = "2022.3.29";
        public const string PluginGuid = "jp.dreamingpig.amongus.stargazer";
        public const string PluginName = "Stargazer";
        public const string PluginVersion = "1.0.0";
        
        public static StargazerPlugin Instance;
        public static System.Random Random;

        public Harmony Harmony = new Harmony(PluginGuid);

        override public void Load()
        {
            Instance = this;
            Random = new System.Random();

            //TODO: 言語データを読み込む
            //Language.Language.Load();

            // Harmonyパッチ全てを適用する
            Harmony.PatchAll();

            //追加マップを読み込む
            Map.AdditionalMapManager.Load();
        }

    }


}

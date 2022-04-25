using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using Stargazer.Module;
using UnityEngine;

namespace Stargazer.Patches
{
    [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.GetString), typeof(SystemTypes))]
    public class CustomSystemTypesPatch
    {
        public static bool Prefix([HarmonyArgument(0)] SystemTypes systemType, ref string __result)
        {
            if (CustomSystemTypes.IsCustomSystemTypes(systemType))
            {
                __result = CustomSystemTypes.GetCustomSystemTypes(systemType)?.GetTranslatedString() ?? "";
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.GetString), typeof(SystemTypes))]
    public class CustomTaskTypesPatch
    {
        public static bool Prefix([HarmonyArgument(0)] TaskTypes taskType, ref string __result)
        {
            if (CustomTaskTypes.IsCustomTaskTypes(taskType))
            {
                __result = CustomTaskTypes.GetCustomTaskTypes(taskType)?.GetTranslatedString() ?? "";
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.GetStringWithDefault),typeof(StringNames),typeof(string),typeof(UnhollowerBaseLib.Il2CppReferenceArray<Il2CppSystem.Object>))]
    public class CustomStringsPatch
    {
        public static bool Prefix([HarmonyArgument(0)] StringNames stringNames, ref string __result)
        {
            if (CustomStrings.IsCustomStrings(stringNames))
            {
                __result = CustomStrings.GetCustomStrings(stringNames)?.GetTranslatedString() ?? "";
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(LanguageUnit), nameof(LanguageUnit.GetImage))]
    public class CustomImagePatch
    {
        public static bool Prefix([HarmonyArgument(0)] ImageNames imageNames, ref Sprite __result)
        {
            if (CustomImageNames.IsCustomImageNames(imageNames))
            {
                __result = CustomImageNames.GetCustomImageNames(imageNames)?.GetSprite() ?? null;
                return false;
            }
            return true;
        }
    }
}

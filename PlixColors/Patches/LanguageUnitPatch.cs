using System;
using System.Collections.Generic;
using HarmonyLib;
using UnhollowerBaseLib;

namespace PlixColors.Patches
{
    [HarmonyPatch(typeof(LanguageUnit), nameof(LanguageUnit.GetString), 
        typeof(StringNames), typeof(string), typeof(Il2CppReferenceArray<Il2CppSystem.Object>))]
    public static class LanguageUnitPatch
    {
        private static readonly Dictionary<int, string> Strings = new();
        private static int _id = -9999;
        
        public static StringNames RegisterStringName(string str)
        {
            Strings[_id] = str;
            return (StringNames) _id--;
        }

        public static bool Prefix(ref string __result, StringNames stringId)
        {
            var intName = (int) stringId;
            if (intName > -9999) return true;
            
            var text = Strings.GetValueOrDefault(intName);
            if (text is null) return true;
            
            __result = text;
            return false;
        }
    }
}
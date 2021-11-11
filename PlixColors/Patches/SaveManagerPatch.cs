using BepInEx.Configuration;
using HarmonyLib;

namespace PlixColors.Patches
{
    [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.BodyColor))]
    public static class SaveManagerPatch
    {
        private static byte currentValue;
        
        [HarmonyPrefix]
        [HarmonyPatch(MethodType.Setter)]
        private static bool SetterPatch(byte value)
        {
            currentValue = value;
            return value <= Manager.OriginalCount;
        }
        
        [HarmonyPrefix]
        [HarmonyPatch(MethodType.Getter)]
        private static bool GetterPatch(ref byte __result)
        {
            __result = currentValue;
            return false;
        }
    }
}
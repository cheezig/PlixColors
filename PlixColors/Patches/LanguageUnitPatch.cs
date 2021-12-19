using HarmonyLib;

namespace PlixColors.Patches
{
    [HarmonyPatch(typeof(Palette), nameof(Palette.GetColorName))]
    public static class GetColorNamePatch
    {
        public static bool Prefix(ref string __result, int colorId)
        {
            if (Manager.GetOrDefault(colorId) is var cc && cc != null)
            {
                __result = cc.Name.Replace('_', ' ');
                return false;
            }

            return true;
        }
    }
}
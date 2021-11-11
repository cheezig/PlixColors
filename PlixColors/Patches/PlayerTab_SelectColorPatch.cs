using System.Linq;
using HarmonyLib;

namespace PlixColors.Patches
{
    public static partial class PlayerTabPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(PlayerTab.SelectColor))]
        private static void UpdateAvailableColors(int colorId, PlayerTab __instance)
        {
            __instance.PlayerPreview.HatSlot.SetHat(SaveManager.LastHat, colorId);
        }
    }
}
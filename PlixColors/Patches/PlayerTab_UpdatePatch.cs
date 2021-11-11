using System.Linq;
using HarmonyLib;

namespace PlixColors.Patches
{
    public static partial class PlayerTabPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(PlayerTab.Update))]
        public static void UpdatePostfix(PlayerTab __instance)
        {
            var chips = __instance.ColorChips.ToArray();

            for (var i = 0; i < chips.Count; i++)
                chips[i].Inner.color = Palette.PlayerColors.ElementAtOrDefault(i);
        }
    }
}
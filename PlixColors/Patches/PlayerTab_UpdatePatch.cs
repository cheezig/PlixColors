using HarmonyLib;
using System.Linq;
using TMPro;
using UnityEngine;

namespace PlixColors.Patches
{
    public static partial class PlayerTabPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(PlayerTab.Update))]
        public static void UpdatePostfix(PlayerTab __instance)
        {
            if (
                !__instance.transform.FindChild("setflag")
                && __instance.transform.FindChild("Text").gameObject.GetComponent<TextMeshPro>() is var tmp
                && tmp
            )
            {
                var noAuth = string.IsNullOrWhiteSpace(PlixColorsPlugin.ConfAuthor);
                var auth = noAuth ? "PlixColors" : $"{PlixColorsPlugin.ConfAuthor} with PlixColors";

                tmp.text = $"Colors\n<size=80%>(by {auth})</size>";

                var flag = new GameObject("setflag");
                flag.transform.SetParent(__instance.transform);
            }

            var chips = __instance.ColorChips.ToArray();

            for (var i = 0; i < chips.Count; i++)
            {
                chips[i].Inner.color = Palette.PlayerColors.ElementAtOrDefault(i);
            }
        }
    }
}
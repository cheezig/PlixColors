using System.Linq;
using HarmonyLib;
using PlixColors.Components;
using UnhollowerBaseLib;
using UnityEngine;

namespace PlixColors.Patches
{
    [HarmonyPatch(typeof(PlayerTab))]
    [HarmonyPriority(Priority.VeryLow)]
    public static partial class PlayerTabPatch
    {
        public const int Columns = 5;

        [HarmonyPrefix]
        [HarmonyPatch(nameof(PlayerTab.OnEnable))]
        private static void OnEnablePrefix(PlayerTab __instance)
        {
            var hatsTab = __instance.transform.parent.parent.GetComponentInChildren<HatsTab>(true);
                    
            if (__instance.scroller != null || !hatsTab) return;

            __instance.scroller = Object.Instantiate(hatsTab.scroller, __instance.ColorTabArea);
            
            __instance.scroller.name = "Scroller";
            __instance.scroller.Hitbox.transform.localPosition = Vector3.zero;
            
            UpdateChips(__instance);
        }
        
        [HarmonyPostfix]
        [HarmonyPatch(nameof(PlayerTab.OnEnable))]
        public static void OnEnablePostfix(PlayerTab __instance)
        {
            var chips = __instance.ColorChips.ToArray();

            foreach (var (colorChip, i) in chips.Select((x, y) => (x, y))) 
            {
                var x = __instance.XRange.Lerp(i % Columns / 4f);
                var y = __instance.YStart - (i / Columns) * 0.6f;

                var transform = colorChip.transform;
                transform.localPosition = new Vector3(x, y, -1f);
                colorChip.transform.SetParent(__instance.scroller.Inner.transform);
                
                colorChip.Inner.color = Palette.PlayerColors.ElementAtOrDefault(i);
            }
            
            var rows = Mathf.Max(0, (chips.Count / Columns) - 6);
            __instance.scroller.YBounds.max = (rows * 0.6f) + 0.5f;
        }

        private static bool _chipsUpdated;
        private static void UpdateChips(InventoryTab instance)
        {
            if (_chipsUpdated) return;
           
            var cc = instance.ColorTabPrefab;
            cc.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            
            SetMask(cc.GetComponentsInChildren<SpriteRenderer>());
            SetMask(cc.PlayerEquippedForeground.GetComponentsInChildren<SpriteRenderer>());

            var fg = cc.transform.GetChild(0).gameObject;
            var oldShade = fg.transform.GetChild(0).gameObject;
            var newShade = fg.transform.GetChild(1).gameObject;

            var shadeRenderer = newShade.GetComponent<SpriteRenderer>();
            shadeRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            shadeRenderer.color = Color.Lerp(shadeRenderer.color, Color.black, .5f);

            Object.Destroy(fg.GetComponent<SpriteMask>());
            Object.Destroy(oldShade);
            
            _chipsUpdated = true;

            void SetMask(Il2CppArrayBase<SpriteRenderer> spriteRenderers)
            {
                foreach (var spr in spriteRenderers)
                    spr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            }
        }
    }
}
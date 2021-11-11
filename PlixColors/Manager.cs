using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using PlixColors.Types;
using UnityEngine;

namespace PlixColors
{
    public static class Manager
    {
        public static int OriginalCount { get; } = Palette.PlayerColors.Count;
        public static List<IBaseColor> AllColors { get; } = new();

        public static void RegisterColor(IBaseColor color)
        {
            AllColors.Add(color);

            Palette.PlayerColors = Palette.PlayerColors.AddItem(color.Body).ToArray();
            Palette.ShadowColors = Palette.ShadowColors.AddItem(color.Shadow).ToArray();
            Palette.ColorNames = Palette.ColorNames.AddItem(color.Name).ToArray();
        }

        public static void RegisterColors(IEnumerable<IBaseColor> colors)
        {
            var baseColors = colors as List<IBaseColor> ?? colors.ToList();
            AllColors.AddRange(baseColors);
                
            Palette.PlayerColors = Palette.PlayerColors.Concat(baseColors.Select(x =>  (Color32) x.Body)).ToArray();
            Palette.ShadowColors = Palette.ShadowColors.Concat(baseColors.Select(x => (Color32) x.Shadow)).ToArray();
            Palette.ColorNames = Palette.ColorNames.Concat(baseColors.Select(x => x.Name)).ToArray();
        }
            
        public static IBaseColor GetOrDefault(int colorId)
        {
            var isCustomColor = colorId >= OriginalCount;
            return isCustomColor? AllColors[colorId - OriginalCount] : null;
        }
    }
}
using HarmonyLib;
using PlixColors.Types;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace PlixColors
{
    public static class Manager
    {
        public static int OriginalCount { get; } = Palette.PlayerColors.Count;
        public static List<IBaseColor> AllColors { get; } = new();
        public static List<IBaseColor> ConfColors { get; } = new();


        public static void RegisterColor(IBaseColor color)
        {
            AllColors.Add(color);

            Palette.PlayerColors = Palette.PlayerColors.AddItem(color.Body).ToArray();
            Palette.ShadowColors = Palette.ShadowColors.AddItem(color.Shadow).ToArray();
        }

        public static void RegisterColors(IEnumerable<IBaseColor> colors)
        {
            var baseColors = colors as List<IBaseColor> ?? colors.ToList();
            AllColors.AddRange(baseColors);

            Palette.PlayerColors = Palette.PlayerColors.Concat(baseColors.Select(x => (Color32)x.Body)).ToArray();
            Palette.ShadowColors = Palette.ShadowColors.Concat(baseColors.Select(x => (Color32)x.Shadow)).ToArray();
        }

        public static IBaseColor? GetOrDefault(int colorId)
        {
            if (PlixColorsPlugin.LoadDefaultColors)
            {
                var isCustomColor = colorId >= OriginalCount;
                return isCustomColor ? AllColors[colorId - OriginalCount] : null;
            }
            else
            {
                return AllColors.ElementAtOrDefault(colorId);
            }
        }

        public static void LoadSkidColors()
        {
            var ccPath = Path.Combine(Directory.GetCurrentDirectory(), "colorConf.txt");
            if (!File.Exists(ccPath)) return; // no skid mode

            PlixColorsPlugin.Instance!.Log.LogInfo("Skid mode activated");

            foreach (var line in File.ReadAllLines(ccPath))
            {
                if (line.StartsWith('#')) continue;

                if (line.StartsWith("options:"))
                {
                    var split = line.Trim().Split(':');
                    if (split.Length != 2) continue;

                    SetOptions(split[1]);
                    continue;
                }

                var vals = line.Split(' ');
                var type = vals?
                    .FirstOrDefault(x => x.StartsWith("type"))?
                    .Split(':')?
                    .ElementAtOrDefault(1);

                IBaseColor? color = type switch
                {
                    "static" => Static.Deserialize(line),
                    "hue" => Hue.Deserialize(line),
                    _ => null
                };

                if (color is null) continue;

                ConfColors.Add(color);
            }
        }

        public static void SetOptions(string optionName)
        {
            switch (optionName)
            {
                case "disablePlixColors":
                    PlixColorsPlugin.DisablePlixColors();
                    break;
                case "disableDefaultColors":
                    PlixColorsPlugin.LoadDefaultColors = false;
                    break;
                case var a when a.StartsWith("auth"):
                    var spl = a.Split(' ').Skip(1).Aggregate((x, y) => x + " " + y);
                    PlixColorsPlugin.ConfAuthor = spl;
                    break;
            }
        }
    }
}
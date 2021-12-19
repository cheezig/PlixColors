using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using PlixColors.Components;
using PlixColors.Types;
using System.Linq;
using UnityEngine;

namespace PlixColors
{
    [BepInProcess("Among Us.exe")]
    [BepInAutoPlugin("plix.colors")]
    public sealed partial class PlixColorsPlugin : BasePlugin
    {
        public static PlixColorsPlugin? Instance { get; private set; }

        public static void DisablePlixColors() => IncludeColors = false;

        public static bool IncludeColors { get; set; } = true;
        public static bool LoadDefaultColors { get; set; } = true;
        public static string? ConfAuthor { get; set; } = null;

        public Harmony Harmony { get; } = new(Id);

        public override void Load()
        {
            Instance ??= this;
            LoadDefaultColors = true;
            Manager.LoadSkidColors();
            Harmony.PatchAll();

            AwakeHooks.OnAwake += () =>
            {
                ModManager.Instance.ShowModStamp();
                PlixComponent.Register();
                InitColors();
            };
        }

        private static void InitColors()
        {
            if (IncludeColors)
            {
                Manager.RegisterColors(new IBaseColor[] {
                    new Static(new Color32(214, 186, 0, 255), "Gold"),
                    new Static(new Color32(239, 189, 191, 255), "Salmon"),
                    new Static(new Color32(154, 140, 61, 255), "Olive"),
                    new Static(new Color32(22, 132, 176, 255), "Turquoise"),
                    new Static(new Color32(112, 143, 46, 255), "Wasabi"),
                    new Static(new Color32(37, 184, 191, 255), "Teal"),
                    new Hue(1f, 1f, 5f, "Rainbow")
                    }
                );
            }

            if (Manager.ConfColors.Any())
            {
                Manager.RegisterColors(Manager.ConfColors);
            }

            if (!LoadDefaultColors && (Manager.ConfColors.Any() || IncludeColors))
            {
                Palette.PlayerColors = Palette.PlayerColors.Skip(Manager.OriginalCount).ToArray();
                Palette.ShadowColors = Palette.ShadowColors.Skip(Manager.OriginalCount).ToArray();
            }

        }
    }
}
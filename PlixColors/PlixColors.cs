using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using PlixColors.Components;
using PlixColors.Types;
using UnhollowerRuntimeLib;
using UnityEngine;
using static PlixColors.Patches.LanguageUnitPatch;

namespace PlixColors
{
    [BepInProcess("Among Us.exe")]
    [BepInPlugin(Id, "PlixColors", "0.1.0")]
    public sealed class PlixColorsPlugin : BasePlugin
    {
        public static PlixColorsPlugin Instance { get; private set; }

        public static void DisablePlixColors() => IncludeColors = false;
        public static bool IncludeColors { get; private set; } = true;
        
        public const string Id = "plix.colors";
        public Harmony Harmony { get; } = new(Id);

        public override void Load()
        {
            Instance ??= this;
            Harmony.PatchAll();

            AwakeHooks.OnAwake += () =>
            {
                ModManager.Instance.ShowModStamp();
                PlixComponent.Register();
                RegisterPlixColors();
            };
        }

        private static void RegisterPlixColors()
        {
            if (!IncludeColors) return;
            Manager.RegisterColors(new IBaseColor[] {
                new Static(new Color32(214, 186, 0, 255), RegisterStringName("Gold")),
                new Static(new Color32(239, 189, 191, 255), RegisterStringName("Salmon")),
                new Static(new Color32(154, 140, 61, 255), RegisterStringName("Olive")),
                new Static(new Color32(22, 132, 176, 255), RegisterStringName("Turquoise")),
                new Static(new Color32(112, 143, 46, 255), RegisterStringName("Wasabi")),
                new Static(new Color32(37, 184, 191, 255), RegisterStringName("Teal")),
                new Hue(0.99f, 1f, 5f, RegisterStringName("Rainbow"))
                }
            );

            DisablePlixColors();
        }
    }
}
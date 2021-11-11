using HarmonyLib;
using PlixColors.Components;
using UnityEngine;

namespace PlixColors.Patches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetPlayerMaterialColors), typeof(int), typeof(Renderer))]
    public static class PlayerControlPatch
    {
        public static void Postfix(int colorId, Renderer rend)
        {
            var customColor = Manager.GetOrDefault(colorId);
            if (customColor is { IsActive: true })
            {
                PlayerColorComponent.Initialize(rend.gameObject, colorId);
                return;
            }
            
            PlayerColorComponent.Clear(rend.gameObject);
        }
    }
}
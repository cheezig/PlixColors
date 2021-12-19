using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using IL2CG = Il2CppSystem.Collections.Generic;

namespace PlixColors.Patches
{
    [HarmonyPatch(typeof(PlayerParticles), nameof(PlayerParticles.Start))]
    public static class PlayerParticlesPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(PlayerParticles __instance)
        {
            __instance.fill = new RandomFill<PlayerParticleInfo>();
            __instance.fill.Set(__instance.Sprites.Cast<IL2CG.IEnumerable<PlayerParticleInfo>>());

            var possible = Enumerable.Range(0, Palette.PlayerColors.Length).ToList();
            var rand = new System.Random();

            while (__instance.pool.NotInUse > 0)
            {
                var index = rand.Next(possible.Count);
                var num = possible[index];
                possible.RemoveAt(index);

                var playerParticle = __instance.pool.Get<PlayerParticle>();
                PlayerControl.SetPlayerMaterialColors(num, playerParticle.myRend);
                __instance.PlacePlayer(playerParticle, true);
            }

            return false;
        }
    }
}

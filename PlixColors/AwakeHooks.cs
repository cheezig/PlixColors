using HarmonyLib;

namespace PlixColors
{
    public static class AwakeHooks
    {
        public delegate void AwakeHandler();
        public static event AwakeHandler OnAwake;

        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.Awake))]
        private static class LoadPluginPatch
        {
            public static void Postfix() => OnAwake?.Invoke();
        }
    }
}
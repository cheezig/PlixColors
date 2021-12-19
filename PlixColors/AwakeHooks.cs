using HarmonyLib;

namespace PlixColors
{
    public static class AwakeHooks
    {
        public delegate void AwakeHandler();
        public static event AwakeHandler? OnAwake;

        private static bool _executed;

        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.Awake))]
        private static class LoadPluginPatch
        {
            public static void Postfix()
            {
                if (!_executed)
                {
                    OnAwake?.Invoke();
                    _executed = true;
                }
            }
        }
    }
}
using System;
using PlixColors.Types;
using UnhollowerBaseLib.Attributes;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace PlixColors.Components
{
    public sealed class PlixComponent : MonoBehaviour
    {
        private static string _gameObjectName = nameof(PlixComponent) + new Guid();
        
        [HideFromIl2Cpp]
        public static void Register()
        {
            if (GameObject.Find(_gameObjectName)) return;
            
            ClassInjector.RegisterTypeInIl2Cpp<PlixComponent>();
            ClassInjector.RegisterTypeInIl2Cpp<PlayerColorComponent>();
                
            var epicColors = new GameObject(_gameObjectName);
            DontDestroyOnLoad(epicColors);
            epicColors.AddComponent<PlixComponent>();
        }
        
        public PlixComponent(IntPtr ptr) : base(ptr) { }

        public void Update()
        {
            for (var i = 0; i < Palette.PlayerColors.Length; i++)
            {
                var customColor = Manager.GetOrDefault(i);
                if (customColor is null or { Update: null }) continue;
                
                customColor.Update(i);
            }
        }
    }
}
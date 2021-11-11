using System;
using PlixColors.Types;
using UnhollowerBaseLib.Attributes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PlixColors.Components
{
    public sealed class PlayerColorComponent : MonoBehaviour
    {
        private static readonly int BodyColor = Shader.PropertyToID("_BodyColor");
        private static readonly int BackColor = Shader.PropertyToID("_BackColor");
        private static readonly int VisorColor = Shader.PropertyToID("_VisorColor");
        
        public SpriteRenderer SpriteRenderer { get; set; }
        public int Id { get; set; }
        
        
        public PlayerColorComponent(IntPtr ptr) : base(ptr) {}
        
        [HideFromIl2Cpp]
        public static void Initialize(GameObject gameObject, int id)
        {
            var component = gameObject.GetComponent<PlayerColorComponent>();
            if (!component) component = gameObject.AddComponent<PlayerColorComponent>();

            component.Id = id;
        }
        
        [HideFromIl2Cpp]
        public static void Clear(GameObject gameObject)
        {
            var component = gameObject.GetComponent<PlayerColorComponent>();
            if (component) DestroyImmediate(component);
        }

        private void Awake()
        {
            SpriteRenderer ??= gameObject.GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (!SpriteRenderer || Id < 0 || Id > Palette.PlayerColors.Length) return;

            SpriteRenderer.material.SetColor(BodyColor, Palette.PlayerColors[Id]);
            SpriteRenderer.material.SetColor(BackColor, Palette.ShadowColors[Id]);
            SpriteRenderer.material.SetColor(VisorColor, Palette.VisorColor);
        }
    }
}
using System;
using UnityEngine;

namespace PlixColors.Types
{
    public sealed class Hue : IBaseColor
    {
        private float _timer;
        private float Duration = 5f;
        
        private readonly (float saturation, float value) _body;
        private readonly (float saturation, float value) _shadow;

        public Color Body => Color.HSVToRGB(_timer, _body.saturation, _body.value);
        public Color Shadow => Color.HSVToRGB(_timer, _shadow.saturation, _shadow.value);
        public StringNames Name { get; }
        
        public Hue(float saturation, float value, float duration, StringNames name)
        {
            Name = name;
            _body = (saturation, value);
            _shadow = (saturation, value * 0.65f);

            Duration = duration;
        }
        
        public Hue(float saturation, float value, float duration, 
            float shadowSaturation, float shadowValue, StringNames name)
        {
            Name = name;
            _body = (saturation, value);
            _shadow = (shadowSaturation, shadowValue);

            Duration = duration;
        }

        public Action<int> Update => id =>
        {
            _timer = (Time.deltaTime / Duration + _timer) % 1f;

            Palette.PlayerColors[id] = Body;
            Palette.ShadowColors[id] = Shadow;
        };
    }
}
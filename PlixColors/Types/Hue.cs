using System;
using UnityEngine;

namespace PlixColors.Types
{
    public sealed class Hue : IBaseColor
    {
        private float _timer;
        private float Duration = 5f;

        private (float saturation, float value) _body;
        private (float saturation, float value) _shadow;

        public Color Body => Color.HSVToRGB(_timer, _body.saturation, _body.value);
        public Color Shadow => Color.HSVToRGB(_timer, _shadow.saturation, _shadow.value);
        public string Name { get; private set; }

        public Hue(float saturation, float value, float duration, string name)
        {
            Name = name;
            _body = (saturation, value);
            _shadow = (saturation, value * 0.65f);

            Duration = duration;
        }

        public Hue(float saturation, float value,
            float shadowSaturation, float shadowValue, float duration,
            string name)
        {
            Name = name;
            _body = (saturation, value);
            _shadow = (shadowSaturation, shadowValue);

            Duration = duration;
        }

        private Action<int>? _update;
        public Action<int> Update => _update ??= id =>
        {
            _timer = (Time.deltaTime / Duration + _timer) % 1f;

            Palette.PlayerColors[id] = Body;
            Palette.ShadowColors[id] = Shadow;
        };

        public static Hue? Deserialize(string data)
        {
            var colorData = data.Split(' ');

            (float saturation, float value)? main = null;
            (float saturation, float value)? shadow = null;
            string? name = null;
            float duration = 5f;

            foreach (String colorFieldString in colorData)
            {
                var colorField = colorFieldString.Split(':');
                if (colorField.Length != 2) continue;

                var type = colorField[0];
                var value = colorField[1];

                switch (type)
                {
                    case "name":
                        name = value;
                        break;

                    case "duration":
                        if (float.TryParse(value, out var dur))
                        {
                            dur = duration;
                        }
                        break;

                    case "main":
                        main = ParseColor(value);
                        break;

                    case "shadow":
                        shadow = ParseColor(value);
                        break;
                }
            }

            if (!main.HasValue)
            {
                PlixColorsPlugin.Instance!.Log.LogError($"Unable to parse the line:\n{data}");
                return null;
            }

            shadow ??= (main.Value.saturation, main.Value.value * 0.65f);

            return new Hue(
                main.Value.saturation, main.Value.value,
                shadow.Value.saturation, shadow.Value.value,
                duration, name ?? string.Empty
            );
        }

        private static (float saturation, float value)? ParseColor(string field)
        {
            var colorHSV = field.Split(',');

            if (colorHSV.Length != 2
                || TryParseHundred(colorHSV[0], out var saturation)
                || TryParseHundred(colorHSV[1], out var value)
            )
            {
                return null;
            }

            return ((float)saturation / 100, (float)value / 100);
        }

        private static bool TryParseHundred(String field, out Byte value)
        {
            var canChange = !Byte.TryParse(field, out value);
            if (value > 100f) return true;
            return canChange;
        }
    }
}
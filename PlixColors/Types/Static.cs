using System;
using UnityEngine;

namespace PlixColors.Types
{
    public sealed class Static : IBaseColor
    {
        private Color _body;
        private Color _shadow;

        public Static(Color body, string name)
        {
            _body = body;
            _shadow = Color.Lerp(body, Color.black, .3f);
            Name = name;
        }

        public Static(Color body, Color shadow, string name)
        {
            _body = body;
            _shadow = shadow;
            Name = name;
        }

        public Color Body => _body;
        public Color Shadow => _shadow;
        public string Name { get; private set; }
        public Action<int>? Update => null;

        public static Static Deserialize(string data)
        {
            var colorData = data.Split(' ');

            Color? main = null;
            Color? shadow = null;
            string? name = null;

            foreach (var colorFieldString in colorData)
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
                return null!;
            }

            shadow ??= Color.Lerp(main.Value, Color.black, .3f);

            return new Static(main.Value, shadow.Value, name ?? string.Empty);
        }

        private static Color? ParseColor(string rgb)
        {
            var rgbs = rgb.Split(',');
            if (rgbs.Length != 3)
                throw new ArgumentOutOfRangeException("Data contained more or less than 3");

            try
            {
                return new Color(byte.Parse(rgbs[0]) / 255f, byte.Parse(rgbs[1]) / 255f, byte.Parse(rgbs[2]) / 255f);
            }
            catch
            {
                return null;
            }
        }
    }
}
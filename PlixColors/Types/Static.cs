using System;
using UnityEngine;

namespace PlixColors.Types
{
    public sealed class Static : IBaseColor
    {
        private Color _body;
        private Color _shadow;
        private StringNames _name;

        public Static(Color body, StringNames name)
        {
            _body = body;
            _shadow = Color.Lerp(body, Color.black, .3f);
            _name = name;
        }
        
        public Static(Color body, Color shadow, StringNames name)
        {
            _body = body;
            _shadow = shadow;
            _name = name;
        }

        public Color Body => _body;
        public Color Shadow => _shadow;
        public StringNames Name => _name;
        public Action<int> Update => null;
    }
}
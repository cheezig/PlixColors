using System;
using JetBrains.Annotations;
using UnityEngine;

namespace PlixColors.Types
{
    public interface IBaseColor
    {
        public Color Body { get; }
        public Color Shadow { get; }
        public StringNames Name { get; }
        
        public Action<int> Update { get; }
    }
}
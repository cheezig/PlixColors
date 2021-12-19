using JetBrains.Annotations;
using System;
using UnityEngine;

namespace PlixColors.Types
{
    public interface IBaseColor
    {
        public Color Body { get; }
        public Color Shadow { get; }
        public string Name { get; }

        public Action<int> Update { get; }
    }
}
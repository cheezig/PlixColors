using UnityEngine;

namespace PlixColors.Types
{
    public interface IBaseColor
    {
        public Color Body { get; }
        public Color Shadow { get; }
        public StringNames Name { get; }
        
        public bool IsActive { get; }
        public void Update(int id);
    }
}
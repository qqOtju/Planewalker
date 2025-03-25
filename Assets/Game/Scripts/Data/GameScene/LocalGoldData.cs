using System;

namespace Game.Scripts.Data.GameScene
{
    public class LocalGoldData
    {
        private int _count;
        
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                OnCountChange?.Invoke(_count);
            }
        }
        
        public event Action<int> OnCountChange; 

        public LocalGoldData() => Count = 0;
    }
}
using System;
using UnityEngine;

namespace Game.Scripts.Data.Project
{
    public class GoldData
    {
        private int _count; 
        
        public int Count
        {
            get { return _count; }
            set
            {
                _count = value;
                PlayerPrefs.SetInt("GoldCount", _count);
                OnCountChanged?.Invoke(_count);
            }
        }

        public event Action<int> OnCountChanged;

        public GoldData()
        {
            _count = PlayerPrefs.GetInt("GoldCount");
        }
    }
}
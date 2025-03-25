using System;
using UnityEngine;

namespace Game.Scripts.Data.Project
{
    public class DataData
    {
        private int _data;
        
        public int Data
        {
            get => _data;
            set
            {
                _data = value;
                OnDataCountChanged?.Invoke(_data);
                PlayerPrefs.SetInt("DataCount", _data);
            }
        }
        
        public event Action<int> OnDataCountChanged;
        
        public DataData() =>
            _data = PlayerPrefs.GetInt("DataCount");
    }
}
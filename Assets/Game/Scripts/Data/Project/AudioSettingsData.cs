using System;
using UnityEngine;

namespace Game.Scripts.Data.Project
{
    public class AudioSettingsData
    {
        private float _soundVol;
        private float _musVol;
        
        public float MusVol
        {
            get => _musVol;
            set
            {
                _musVol = value;
                OnMusVolChanged?.Invoke(value);
                PlayerPrefs.SetFloat("MusVol", value);
            }
        }
        
        public float SoundVol
        {
            get => _soundVol;
            set
            {
                _soundVol = value;
                OnSoundVolChanged?.Invoke(value);
                PlayerPrefs.SetFloat("SoundVol", value);
            }
        }
        
        public event Action<float> OnMusVolChanged;
        public event Action<float> OnSoundVolChanged;
        
        public AudioSettingsData() => GetPrefs();

        private void GetPrefs()
        {
            _musVol = PlayerPrefs.GetFloat("MusVol", 0.5f);
            _soundVol = PlayerPrefs.GetFloat("SoundVol", 0.5f);
        }
    }
}
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Menu.Settings
{
    public class ASlider: MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _valueText;
        
        public event Action<float> OnValueChanged;

        private void Awake()
        {
            _slider.onValueChanged.AddListener(value =>
            {
                _valueText.text = $"{(int)(value * 100f)}%";
                OnValueChanged?.Invoke(value);
            });
        }

        private void OnDestroy()
        {
            _slider.onValueChanged.RemoveAllListeners();
        }

        public void SetValue(float volume)
        {
            _slider.value = volume;
            _valueText.text = $"{(int)(volume * 100f)}%";
        }
        
    }
}
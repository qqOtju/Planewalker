using Game.Scripts.Data.Project;
using UnityEngine;
using Zenject;

namespace Game.Scripts.UI.Menu.Settings
{
    public class UISliderSettingsPanel: UIPanelTemplate
    {
        [SerializeField] private ASlider _musicSlider;
        [SerializeField] private ASlider _soundSlider;
        
        private AudioSettingsData _audioSettingsData;
        
        [Inject]
        private void Construct(AudioSettingsData audioSettingsData)
        {
            _audioSettingsData = audioSettingsData;
            _musicSlider.SetValue(_audioSettingsData.MusVol);
            _soundSlider.SetValue(_audioSettingsData.SoundVol);
        }
        
        protected override void Awake()
        {
            base.Awake();
            _musicSlider.OnValueChanged += obj => _audioSettingsData.MusVol = obj;
            _soundSlider.OnValueChanged += obj => _audioSettingsData.SoundVol = obj;
        }
    }
}
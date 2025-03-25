using Game.Scripts.Data.Project;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Other
{
    public class SoundManager: MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSourceMusic;
        [SerializeField] private AudioSource _audioSourceSound;
        [SerializeField] private AudioClip _buttonClick;
        [SerializeField] private AudioClip _music;
        
        private AudioSettingsData _audioSettingsData;
        
        [Inject]
        private void Construct(AudioSettingsData audioSettingsData)
        {
            _audioSettingsData = audioSettingsData;
            SetMusVol(_audioSettingsData.MusVol);
            SetSoundVol(_audioSettingsData.SoundVol);
            _audioSettingsData.OnMusVolChanged += SetMusVol;
            _audioSettingsData.OnSoundVolChanged += SetSoundVol;
        }

        private void Awake()
        {
            var go = gameObject;
            go.transform.parent = null;
            DontDestroyOnLoad(go);
        }
        
        private void OnDestroy()
        {
            _audioSettingsData.OnMusVolChanged -= SetMusVol;
            _audioSettingsData.OnSoundVolChanged -= SetSoundVol;
        }

        private void SetMusVol(float value) => _audioSourceMusic.volume = value;

        private void SetSoundVol(float value) => _audioSourceSound.volume = value;
        
        public void PlaySound(AudioClip clip)
        {
            _audioSourceSound.PlayOneShot(clip);
        }

        public void PlayButtonClick()
        {
            _audioSourceSound.PlayOneShot(_buttonClick);
        }
        
        public void PlayMusic()
        {
            _audioSourceMusic.clip = _music;
            _audioSourceMusic.loop = true;
            _audioSourceMusic.Play();
        }
        
        public void StopMusic()
        {
            _audioSourceMusic.Stop();
        }
    }
}
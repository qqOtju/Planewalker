using Game.Scripts.Other;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Scripts.UI.Menu.Main
{
    public class UIMenuPanel: MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _infoButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _planesButton;
        [SerializeField] private Button _upgradesButton;
        [SerializeField] private UIPanelTemplate _settingsPanel;
        [SerializeField] private UIPanelTemplate _tutorial;
        [SerializeField] private UIPanelTemplate _planesPanel;
        [SerializeField] private UIPanelTemplate _upgradesPanel;
        [SerializeField] private UIPanelTemplate _missionsPanel;

        private bool _menuOpened;

        [Inject]
        private void Construct(SoundManager s)
        {
            s.StopMusic();
        }
        
        private void Awake()
        {
            _playButton.onClick.AddListener(() => _missionsPanel.Show());
            _infoButton.onClick.AddListener(() => _tutorial.Show());
            _settingsButton.onClick.AddListener(() => _settingsPanel.Show());
            _planesButton.onClick.AddListener(() => _planesPanel.Show());
            _upgradesButton.onClick.AddListener(() => _upgradesPanel.Show());
        }
    }
}
using Game.Scripts.Data.GameScene;
using Game.Scripts.Logic;
using LeanTween.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Game.Scripts.UI.Game
{
    public class UIGame: MonoBehaviour
    {
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private Slider _xSlider;
        [SerializeField] private Slider _ySlider;
        
        private PlayerControlHandler _playerControlHandler;
        private LocalGoldData _localGoldData;
        private Color _baseColor;
        
        [Inject]
        private void Construct(LocalGoldData localGoldData,  PlayerControlHandler playerControlHandler)
        {
            _localGoldData = localGoldData;
            _playerControlHandler = playerControlHandler;
        }

        private void Awake()
        {
            _baseColor = _scoreText.color;
            _scoreText.text = _localGoldData.Count.ToString();
            _localGoldData.OnCountChange += RefreshLocalGold; 
            _homeButton.onClick.AddListener(() => SceneManager.LoadScene(1, LoadSceneMode.Single));
            _restartButton.onClick.AddListener(() => SceneManager.LoadScene(2, LoadSceneMode.Single));
            _xSlider.onValueChanged.AddListener(OnXSliderChange);
            _ySlider.onValueChanged.AddListener(OnYSliderChange);
            _playerControlHandler.MoveInput = new Vector2(0f, 0f);
        }

        private void OnDestroy()
        {
            _xSlider.onValueChanged.RemoveAllListeners();
            _ySlider.onValueChanged.RemoveAllListeners();
            _homeButton.onClick.RemoveAllListeners();
            _restartButton.onClick.RemoveAllListeners();
            _localGoldData.OnCountChange -= RefreshLocalGold;
        }

        private void OnXSliderChange(float arg0) => 
            _playerControlHandler.MoveInput = new Vector2(arg0, _playerControlHandler.MoveInput.y);

        private void OnYSliderChange(float arg0) => 
            _playerControlHandler.MoveInput = new Vector2(_playerControlHandler.MoveInput.x, arg0);

        private void RefreshLocalGold(int obj)
        {
            _scoreText.text = obj.ToString();
            LeanTween.Framework.LeanTween.scale(_scoreText.gameObject, Vector3.one * 1.2f, 0.1f)
                .setEase(LeanTweenType.easeOutBack)
                .setOnComplete(() =>
                {
                    LeanTween.Framework.LeanTween.scale(_scoreText.gameObject, Vector3.one, 0.1f)
                        .setEase(LeanTweenType.easeInBack);
                });
        }
    }
}
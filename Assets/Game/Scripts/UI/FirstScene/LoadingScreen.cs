using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Scripts.Infrastructure.AppBootstrap.Operations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.FirstScene
{
    [RequireComponent(typeof(Canvas), typeof(CanvasGroup))]
    public class LoadingScreen: MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _progressText;
        [SerializeField] private float _barFillSpeed = 1f;
        [SerializeField] private int _fadeSpeed = 10;
        
        private CanvasGroup _group;
        private float _progress;
        private Canvas _canvas;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _canvas = GetComponent<Canvas>();
            _group = GetComponent<CanvasGroup>();
        }

        public async Task Load(Queue<ILoadingOperation> loadingOperations)
        {
            _canvas.enabled = true;
            foreach (var operation in loadingOperations)
            {
                _slider.value = 0;
                _progress = 0;
                var cor = StartCoroutine(UpdateBar());
                await operation.UseOperation(progress => _progress = progress);
                while (_slider.value < _progress)
                    await Task.Delay(1);
                StopCoroutine(cor);
            }

            var value = 0.5f;
            while (value >= 0f)
            {
                _group.alpha = Mathf.Lerp(0, 1, value);
                value -= Time.deltaTime;
                await Task.Delay(_fadeSpeed);
            }
            _group.alpha = 0;
            _canvas.enabled = false;
            Destroy(gameObject);
        }

        private IEnumerator UpdateBar()
        {
            while (_canvas.enabled)
            {
                if(_slider.value < _progress)
                {
                    _slider.value += Time.deltaTime * _barFillSpeed;
                    _progressText.text = $"{Mathf.RoundToInt(_slider.value * 100)}%";
                }
                yield return null;
            }
        }
    }
}
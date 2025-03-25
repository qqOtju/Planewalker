using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.UI.Menu;
using TMPro;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class UITutorial: UIPanelTemplate
    {
        [SerializeField] private TutorialData[] _tutorialData;
        [SerializeField] private Layout _layout;
        [SerializeField] private Sprite _activeIndicator;
        [SerializeField] private Sprite _inactiveIndicator;
        [SerializeField] private Button _prevButton;
        [SerializeField] private Button _nextButton;
        [SerializeField] private TMP_Text _guideText;
        [SerializeField] private Image _guideImage;

        private List<Image> _indicators = new ();
        private int _index = 0;
        
        private void Start()
        {
            Canvas.enabled = false;
            _layout.ColumnCount = _tutorialData.Length;
            for (var index = 0; index < _tutorialData.Length; index++)
            {
                var indicator = new GameObject("Indicator", typeof(Image)).GetComponent<Image>();
                indicator.sprite = _inactiveIndicator;
                indicator.transform.SetParent(_layout.transform);
                indicator.color = new Color(1,1,1,.3f);
                indicator.transform.localScale = Vector3.one;
                indicator.preserveAspect = true;
                _indicators.Add(indicator);
            }
            _indicators[0].sprite = _activeIndicator;
            _indicators[0].color = Color.white;
            _layout.Align();
            _nextButton.onClick.AddListener(OnNextButtonClicked);
            _prevButton.onClick.AddListener(OnPrevButtonClicked);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _nextButton.onClick.RemoveListener(OnNextButtonClicked);
        }

        private void OnNextButtonClicked()
        {
            _index++;
            _prevButton.gameObject.SetActive(true);
            if(_index == _tutorialData.Length - 1)
            {
                _nextButton.gameObject.SetActive(false);
            }
            else if(_index > _tutorialData.Length - 1) return;
            Set();
        }
        
        private void OnPrevButtonClicked()
        {
            _index--;
            _nextButton.gameObject.SetActive(true);
            if (_index == 0)
                _prevButton.gameObject.SetActive(false);
            else if(_index < 0) return;
            Set();
        }

        private void Set()
        {
            foreach (var indicator in _indicators)
            {
                indicator.sprite = _inactiveIndicator;
                indicator.color = new Color(1,1,1,.3f);
            }
            _indicators[_index].sprite = _activeIndicator;
            _indicators[_index].color = Color.white;
            _guideText.text = _tutorialData[_index].Text;
            var image = _tutorialData[_index].Image;
            if (image == null)
                _guideImage.enabled = false;
            else
            {
                _guideImage.enabled = true;
                _guideImage.color = _tutorialData[_index].Color;
                _guideImage.sprite = image;
            }
        }

        protected override IEnumerator HideAnimation()
        {
            yield return base.HideAnimation();
        }

        public override void Show()
        {
            base.Show();
            Canvas.enabled = true;
            _prevButton.gameObject.SetActive(false);
            _nextButton.gameObject.SetActive(true);
            _index = 0;
            Set();
        }
    }
    
    [Serializable]
    public struct TutorialData
    {
        [SerializeField] private string _text;
        [SerializeField] private Sprite _image;
        [SerializeField] private Color _color;

        public string Text => _text;
        public Sprite Image => _image;
        public Color Color => _color;
    }
}
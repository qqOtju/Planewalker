using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Menu
{
    [RequireComponent(typeof(Canvas))]
    public class UIPanelTemplate: MonoBehaviour
    {
        [SerializeField] private Transform _content;
        [SerializeField] private Button _closeButton;
        [SerializeField] private float _animationSpeed = 3;
        
        private UIAnimation _uiAnimation;
        
        protected Canvas Canvas;
        
        protected virtual void Awake()
        {
            Canvas = GetComponent<Canvas>();
            _uiAnimation = new UIAnimation(_content, Canvas, _animationSpeed);
            _closeButton.onClick.AddListener(() => StartCoroutine(HideAnimation()));
        }

        protected virtual void OnDestroy()
        {
            _closeButton.onClick.RemoveListener(() => StartCoroutine(HideAnimation()));
        }

        private IEnumerator ShowAnimation()
        {
            yield return _uiAnimation.Show();
        }
        
        protected virtual IEnumerator HideAnimation()
        {
            yield return _uiAnimation.Hide();
        }
        
        public virtual void Show()
        {
            StartCoroutine(ShowAnimation());
        }
    }
}
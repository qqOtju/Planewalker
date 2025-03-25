using System;
using Game.Scripts.Other;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Scripts.UI
{
    [Serializable]
    public class AButton: Button
    {
        [SerializeField] protected Graphic _graphic;
        [SerializeField] protected AnimationCurve _animationCurve = AnimationCurve.Linear(0, 0, 1, 1);

        private SoundManager _soundManager;
        private Vector3 _baseScale;
        private bool _animation;
        
        [Inject]
        private void Construct(SoundManager soundManager) => _soundManager = soundManager;

        protected override void Awake()
        {
            base.Awake();
            _animation = _graphic != null;
            if(_animation)
                _baseScale = _graphic.transform.localScale;
        }
        
        public override void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            Animation();
        }

        public void Animation()
        {
            _soundManager.PlayButtonClick();
            if (!_animation) return;
            LeanTween.Framework.LeanTween.scale(_graphic.gameObject, _baseScale * 1.1f, 0.1f).setEase(_animationCurve)
                .setOnComplete(() =>
                    LeanTween.Framework.LeanTween.scale(_graphic.gameObject, _baseScale, 0.1f).setEase(_animationCurve));
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onClick.RemoveAllListeners();
        }
    }
}
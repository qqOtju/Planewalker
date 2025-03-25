using System;
using Game.Scripts.Logic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Scripts.UI.Game
{
    public class UIBackground: MonoBehaviour
    {
        [SerializeField] private Image _targetImage;

        private PlayerControlHandler _controlHandler;
        private RectTransform _rectTransform;


        [Inject]
        private void Construct(PlayerControlHandler controlHandler)
        {
            _controlHandler = controlHandler;
        }

        private void Awake()
        {
            _controlHandler.OnMoveInputChanged += MoveImage;
        }

        private void Start()
        {
            _rectTransform = _targetImage.GetComponent<RectTransform>();
        }

        private void MoveImage(Vector2 inputVector)
        {
            var invertedVector = -inputVector * 0.1f;
            var newPosition = _rectTransform.anchoredPosition + invertedVector;//
            newPosition.x = Mathf.Clamp(newPosition.x, -0.1f * _rectTransform.rect.width, 1.1f * _rectTransform.rect.width);
            newPosition.y = Mathf.Clamp(newPosition.y, -0.1f * _rectTransform.rect.height, 1.1f * _rectTransform.rect.height);
            _rectTransform.anchoredPosition = newPosition;
        }
    }
}
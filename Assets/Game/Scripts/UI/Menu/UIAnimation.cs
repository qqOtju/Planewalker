using System.Collections;
using UnityEngine;

namespace Game.Scripts.UI.Menu
{
    public class UIAnimation
    {
        private readonly Transform _object;
        private readonly Canvas _canvas;
        private readonly float _speed;
        
        public UIAnimation(Transform @object, Canvas canvas, float speed)
        {
            _object = @object;
            _canvas = canvas;
            _speed = speed;
        }

        public IEnumerator Show()
        {
            _canvas.enabled = true;
            _object.localScale = Vector3.zero;
            while (_object.localScale.x < 1.2 && _object.localScale.y < 1.2 && _object.localScale.z < 1.2)
            {
                _object.localScale += Vector3.one * (Time.deltaTime * _speed);
                yield return null;
            }
            while (_object.localScale.x > 1 && _object.localScale.y > 1 && _object.localScale.z > 1)
            {
                _object.localScale -= Vector3.one * (Time.deltaTime * _speed);
                yield return null;
            }
            _object.localScale = Vector3.one;
        }

        public IEnumerator Hide()
        {
            _object.localScale = Vector3.one;
            while (_object.localScale.x > 0.2 && _object.localScale.y > 0.2 && _object.localScale.z > 0.2)
            {
                _object.localScale -= Vector3.one * (Time.deltaTime * _speed);
                yield return null;
            }
            _canvas.enabled = false;
            _object.localScale = Vector3.zero;
        }
    }
}
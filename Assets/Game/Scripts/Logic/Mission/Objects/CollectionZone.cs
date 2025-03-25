using System;
using LeanTween.Framework;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Logic.Mission.Objects
{
    public class CollectionZone: MonoBehaviour
    {
        [SerializeField] private int _timeToCollect;
        [SerializeField] private ParticleSystem _ps;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private SpriteRenderer _zone;
        [SerializeField] private Color _collectedColor;
        
        private bool _collected;
        private bool _inZone;
        private float _time;

        public event Action<CollectionZone> OnCollected; 
        
        private void Start()
        {
            _collected = false;
            _inZone = false;
            _time = 0;
            _text.text = $"{_timeToCollect:0.0} sec";
        }

        private void FixedUpdate()
        {
            if (!_inZone || _collected) return;
            _time += Time.deltaTime;
            _text.text = $"{_timeToCollect - _time:0.0} sec";
            if (_time <= _timeToCollect || _collected) return;
            _collected = true;
            _text.text = "Collected";
            LeanTween.Framework.LeanTween.scale(_text.gameObject, Vector3.one * 1.2f, 0.1f)
                .setEase(LeanTweenType.easeOutBack)
                .setOnComplete(() =>
                {
                    LeanTween.Framework.LeanTween.scale(_text.gameObject, Vector3.one, 0.1f)
                        .setEase(LeanTweenType.easeInBack);
                });
            OnCollected?.Invoke(this);
            _zone.color = _collectedColor;
            _ps.Play();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            _inZone = true;
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            _inZone = false;
        }
    }
}
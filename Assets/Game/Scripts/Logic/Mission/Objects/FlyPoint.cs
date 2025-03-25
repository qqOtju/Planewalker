using System;
using UnityEngine;

namespace Game.Scripts.Logic.Mission.Objects
{
    public class FlyPoint: MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sr;
        [SerializeField] private Color _collectColor;
        [SerializeField] private ParticleSystem _ps;
        
        private bool _collected;
        
        public event Action<FlyPoint> OnCollected; 
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || _collected) return;
            _sr.color = _collectColor;
            _collected = true;
            _ps.Play();
            OnCollected?.Invoke(this);
        }
    }
}
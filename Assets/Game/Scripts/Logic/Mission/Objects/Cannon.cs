using System.Collections;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Logic.Mission.Objects
{
    public class Cannon: MonoBehaviour
    {
        [SerializeField] private Transform _muzzlePoint;
        [SerializeField] private Transform _barrelView;
        [SerializeField] private Rocket _rocketPrefab;
        [SerializeField] private Vector2 _fireRateRange;

        private Coroutine _fireCoroutine;
        private DiContainer _container;
        private Plane _plane;
        
        [Inject]
        private void Construct(Plane plane, DiContainer container)
        {
            _plane = plane;
            _container = container;
        }
        
        private void Start()
        {
            _fireCoroutine = StartCoroutine(Fire());
        }

        private void Update()
        {
            if (_plane == null) return;
            var direction = _plane.transform.position - transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            _barrelView.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        private void OnDestroy()
        {
            StopCoroutine(_fireCoroutine);
        }

        private IEnumerator Fire()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(_fireRateRange.x, _fireRateRange.y));
                var rocket = _container.InstantiatePrefab(_rocketPrefab);
                rocket.transform.position = _muzzlePoint.position;
                rocket.transform.rotation = _muzzlePoint.rotation;
            }
        }
    }
}
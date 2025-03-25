using System;
using Game.Scripts.Data.Enums;
using Game.Scripts.Data.GameScene;
using Game.Scripts.Data.Project;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Logic
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class Plane: MonoBehaviour
    {
        [SerializeField] private float _minSpeed;
        [SerializeField] private float _baseMaxSpeed;
        [SerializeField] private float _minRotationSpeed;
        [SerializeField] private float _maxRotationSpeed;
        [SerializeField] private ParticleSystem _engineParticles;
        [SerializeField] private SpriteRenderer _sr;
        
        private float _acceleration;
        private float _rotationSpeed;
        private float _maxSpeed;

        private GameStateData _gameStateData;
        private LevelData _levelData;
        private Rigidbody2D _rb;
        private float _currentSpeed;
        private PlayerControlHandler _playerControlHandler;
        private bool _isDead;
        
        private Vector2 MoveInput => _playerControlHandler.MoveInput;

        [Inject]
        private void Construct(GameStateData gameState, LevelData levelData, 
            PlayerControlHandler playerControlHandler)
        {
            _gameStateData = gameState;
            _levelData = levelData;
            _playerControlHandler = playerControlHandler;
        }

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _currentSpeed = _minSpeed;
            _sr.sprite = _levelData.PlaneData.Plane.Icon;
            _acceleration = _levelData.PlaneData.Acceleration;
            _rotationSpeed = _levelData.PlaneData.Controllability;
            _maxSpeed = _levelData.PlaneData.MaxSpeed + _baseMaxSpeed;
        }

        private void FixedUpdate()
        {
            if(_gameStateData.State != GameState.Game)
            {
                if (!_isDead && _gameStateData.State == GameState.Death)
                {
                    _isDead = true;
                    _sr.color = Color.red;
                    GameObject go;
                    LeanTween.Framework.LeanTween.scale((go = gameObject), go.transform.localScale * 1.2f, 0.3f).setOnComplete(() =>
                    {
                        var scale = go.transform.localScale;
                        LeanTween.Framework.LeanTween.value(go, 1, 0, 5f).setOnUpdate(value =>
                        {
                            go.transform.localScale = scale * value;
                            go.transform.rotation = Quaternion.Euler(0, 0,
                                go.transform.rotation.eulerAngles.z + 360 * Time.deltaTime);
                        });
                    });
                }
                _engineParticles.Stop();
                var velocity = _rb.velocity;
                velocity -= velocity * Time.deltaTime;
                _rb.velocity = velocity;
                return;
            }
            MovePlane();
            RotatePlane();
        }

        private void RotatePlane()
        {
            if (MoveInput == Vector2.zero || MoveInput.x == 0) return;
            var angle = transform.rotation.eulerAngles.z + MoveInput.x * _rotationSpeed * -1 * Mathf.Rad2Deg;
            var speedClamp = Normalize(_currentSpeed, _minSpeed, _maxSpeed);
            speedClamp = Math.Abs(speedClamp - 1);
            var rotationSpeed = Denormalize(speedClamp, _minRotationSpeed, _maxRotationSpeed);
            var currentRotation = transform.rotation.eulerAngles.z;
            var newRotation = Mathf.LerpAngle(currentRotation, angle, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, newRotation);
        }

        private void MovePlane()
        {
            if (MoveInput == Vector2.zero || MoveInput.y == 0)
            {
                _rb.velocity = transform.up * _currentSpeed;
                return;
            }
            _currentSpeed += Time.deltaTime * MoveInput.y * _acceleration;
            _currentSpeed = Mathf.Clamp(_currentSpeed, _minSpeed, _maxSpeed);
            _rb.velocity = transform.up * _currentSpeed;
        }
        
        private float Normalize(float value, float min, float max)
        {
            return (value - min) / (max - min);
        }
        
        private float Denormalize(float normalizedValue, float min, float max)
        {
            return (normalizedValue * (max - min)) + min;
        }
    }
}
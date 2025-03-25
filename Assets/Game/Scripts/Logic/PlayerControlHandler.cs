using System;
using Game.Scripts.Data.Project;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Logic
{
    public class PlayerControlHandler: MonoBehaviour
    {
        private Vector2 _moveInput;
        private GoldData _goldData;
        private DataData _dataData;
        private int _count;
        
        public Vector2 MoveInput
        {
            get => _moveInput;
            set
            {
                _moveInput = value;
                OnMoveInputChanged?.Invoke(value);
            }
        }
        
        public event Action<Vector2> OnMoveInputChanged;

        [Inject]
        private void Construct(GoldData goldData, DataData dataData)
        {
            _goldData = goldData;
            _dataData = dataData;
        }
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _count = PlayerPrefs.GetInt("ScreenshotsCount");
        }

        #if UNITY_EDITOR
        private void Update()
        {
            var horizontalInput = Input.GetAxis("Horizontal");
            var verticalInput = Input.GetAxis("Vertical");
            MoveInput = new Vector2(horizontalInput, verticalInput);
            if(Input.GetKeyDown(KeyCode.S))
            {
                _count++;
                ScreenCapture.CaptureScreenshot($"screenshot{_count}.png");
                PlayerPrefs.SetInt("ScreenshotsCount", _count);
                Debug.Log("A screenshot was taken!");
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                _goldData.Count += 200;
                _dataData.Data += 200;
            }
        }
        #endif
    }
}
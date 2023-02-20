using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services.UserInput;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(CharacterController))]
    public class HeroMove : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private float _movementSpeed = 4.0f;
        [SerializeField] private CharacterController _characterController;
        private IInputService _inputService;
        private IPersistentProgressService _progressService;
        private Camera _camera;

        private PositionOnLevel PositionOnLevel => _progressService.Progress.WorldData.PositionOnLevel;
        
        [Inject]
        public void Construct(IInputService inputService, IPersistentProgressService progressService)
        {
            _inputService = inputService;
            _progressService = progressService;
            
            _camera = Camera.main;
        }

        private void Update()
        {
            Vector3 movementVector = Vector3.zero;

            if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
            {
                movementVector = _camera.transform.TransformDirection(_inputService.Axis);
                movementVector.y = 0;
                movementVector.Normalize();

                transform.forward = movementVector;
            }

            movementVector += Physics.gravity;

            _characterController.Move(_movementSpeed * movementVector * Time.deltaTime);
        }

        public void LoadProgress()
        {
            if (GetCurrentLevelName() != PositionOnLevel.Level)
                return;

            Vector3Data savedPosition = PositionOnLevel.Position;

            Warp(savedPosition.AsUnityVector());
        }

        public void UpdateProgress()
        {
            PositionOnLevel.Level = GetCurrentLevelName();
            PositionOnLevel.Position = transform.position.AsVectorData();
        }

        private void Warp(Vector3 to)
        {
            _characterController.enabled = false;
            transform.position = to.AddY(_characterController.height);
            _characterController.enabled = true;
        }

        private string GetCurrentLevelName() => 
            SceneManager.GetActiveScene().name;
    }
}
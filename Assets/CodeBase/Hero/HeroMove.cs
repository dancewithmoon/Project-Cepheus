using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services.Input;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Hero
{
    public class HeroMove : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private float _movementSpeed = 4.0f;
        [SerializeField] private CharacterController _characterController;
        private IInputService _inputService;
        private Camera _camera;

        [Inject]
        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
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

        public void LoadProgress(PlayerProgress progress)
        {
            if (GetCurrentLevelName() != progress.WorldData.PositionOnLevel.Level)
                return;

            Vector3Data savedPosition = progress.WorldData.PositionOnLevel.Position;

            if (savedPosition == null)
                return;

            Warp(savedPosition.AsUnityVector());
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.WorldData.PositionOnLevel =
                new PositionOnLevel(GetCurrentLevelName(), transform.position.AsVectorData());
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
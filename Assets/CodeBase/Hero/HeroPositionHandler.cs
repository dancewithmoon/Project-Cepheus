using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(CharacterController))]
    public class HeroPositionHandler : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private CharacterController _characterController;

        private IPersistentProgressService _progressService;

        private PositionOnLevel PositionOnLevel => _progressService.Progress.WorldData.PositionOnLevel;

        [Inject]
        private void Construct(IPersistentProgressService progressService)
        {
            _progressService = progressService;
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
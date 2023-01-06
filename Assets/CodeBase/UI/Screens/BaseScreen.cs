using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Screens
{
    public abstract class BaseScreen : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        
        protected IPersistentProgressService ProgressService;
        protected PlayerProgress Progress => ProgressService.Progress;
        
        public void Construct(IPersistentProgressService progressService)
        {
            ProgressService = progressService;
        }
        
        private void Awake() => OnAwake();

        protected virtual void OnAwake() => 
            _closeButton.onClick.AddListener(() => Destroy(gameObject));

        private void Start()
        {
            Initialize();
            SubscribeOnUpdates();
        }

        protected virtual void Initialize(){}
        protected virtual void SubscribeOnUpdates(){}
        protected virtual void UnsubscribeOnUpdates(){}

        private void OnDestroy() => UnsubscribeOnUpdates();
    }
}
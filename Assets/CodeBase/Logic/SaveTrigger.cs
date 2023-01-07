using CodeBase.Infrastructure.Services.SaveLoad;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic
{
    public class SaveTrigger : MonoBehaviour
    {
        [SerializeField] private BoxCollider _collider;
        private ISaveLoadService _saveLoadService;

        private void OnDrawGizmos()
        {
            if(_collider == null)
                return;
            
            Gizmos.color = new Color32(30, 200, 30, 130);
            Gizmos.DrawCube(transform.position + _collider.center, _collider.size);
        }
        
        public void Start()
        {
            _saveLoadService = ProjectContext.Instance.Container.Resolve<ISaveLoadService>();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            _saveLoadService.SaveProgress();
            
            Debug.Log("PROGRESS SAVED");
            
            gameObject.SetActive(false);
        }
    }
}

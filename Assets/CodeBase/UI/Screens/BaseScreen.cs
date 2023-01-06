using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Screens
{
    public abstract class BaseScreen : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;

        private void Awake()
        {
            OnAwake();
        }

        protected virtual void OnAwake()
        {
            _closeButton.onClick.AddListener(() => Destroy(gameObject));
        }
    }
}
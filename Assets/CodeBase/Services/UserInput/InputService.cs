using System;
using System.Collections;
using CodeBase.Infrastructure.Services.CoroutineRunner;
using UnityEngine;

namespace CodeBase.Services.UserInput
{
    public abstract class InputService : IInputService
    {
        protected const string Horizontal = "Horizontal";
        protected const string Vertical = "Vertical";
        private const string Button = "Fire1";

        private readonly ICoroutineRunner _coroutineRunner;
        private readonly Camera _camera;
        
        public abstract Vector2 Axis { get; }

        public event Action<GameObject, Vector3> EnvironmentClicked;

        protected InputService(ICoroutineRunner coroutineRunner)
        {
            _camera = Camera.main;
            _coroutineRunner = coroutineRunner;
            _coroutineRunner.StartCoroutine(Update());
        }

        public bool IsAttackButtonUp() => SimpleInput.GetButtonUp(Button);

        protected static Vector2 SimpleInputAxis() => 
            new Vector2(SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical));

        private IEnumerator Update()
        {
            while (_coroutineRunner != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Hit();
                }
                
                yield return null;
            }
        }

        private void Hit()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                EnvironmentClicked?.Invoke(hit.collider.gameObject, hit.point);
            }
        }
    }
}
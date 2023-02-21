using CodeBase.Infrastructure.Services.CoroutineRunner;
using UnityEngine;

namespace CodeBase.Services.UserInput
{
    public class StandaloneInputService : InputService
    {
        public override Vector2 Axis
        {
            get
            {
                Vector2 axis = SimpleInputAxis();
                if (axis == Vector2.zero)
                {
                    axis = UnityAxis();
                }
                return axis;
            }
        }

        public StandaloneInputService(ICoroutineRunner coroutineRunner) : base(coroutineRunner)
        {
        }

        private static Vector2 UnityAxis() => 
            new Vector2(Input.GetAxis(Horizontal), Input.GetAxis(Vertical));
    }
}
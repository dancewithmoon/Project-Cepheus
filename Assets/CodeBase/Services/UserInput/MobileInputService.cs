using CodeBase.Infrastructure.Services.CoroutineRunner;
using UnityEngine;

namespace CodeBase.Services.UserInput
{
    public class MobileInputService : InputService
    {
        public override Vector2 Axis => SimpleInputAxis();

        public MobileInputService(ICoroutineRunner coroutineRunner) : base(coroutineRunner)
        {
        }
    }
}
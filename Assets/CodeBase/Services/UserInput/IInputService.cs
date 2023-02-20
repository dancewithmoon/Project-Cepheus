using System;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Services.UserInput
{
    public interface IInputService : IService
    {
        Vector2 Axis { get; }
        event Action<GameObject, Vector3> EnvironmentClicked;

        bool IsAttackButtonUp();
    }
}
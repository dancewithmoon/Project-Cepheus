using UnityEngine;

namespace CodeBase.Hero
{
    public abstract class CharacterVelocity : MonoBehaviour
    {
        public abstract Vector3 Velocity { get; }
    }
}
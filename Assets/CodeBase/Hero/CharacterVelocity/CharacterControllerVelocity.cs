using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterControllerVelocity : CharacterVelocity
    {
        [SerializeField] private CharacterController _characterController;
        public override Vector3 Velocity => _characterController.velocity;
    }
}
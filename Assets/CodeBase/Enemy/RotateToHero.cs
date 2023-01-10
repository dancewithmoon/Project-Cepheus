using UnityEngine;

namespace CodeBase.Enemy
{
    public class RotateToHero : Aggrable
    {
        [SerializeField] private float _speed;

        private Transform _heroTransform;
        private Vector3 _positionToLook;

        public void Construct(Transform hero)
        {
            _heroTransform = hero;
        }

        private void Update()
        {
            if (IsHeroInitialized()) 
                RotateTowardsHero();
        }

        private void RotateTowardsHero()
        {
            UpdatePositionToLook();

            transform.rotation = GetSmoothedRotation(transform.rotation, _positionToLook);
        }

        private void UpdatePositionToLook()
        {
            Vector3 positionDiff = _heroTransform.position - transform.position;
            _positionToLook = new Vector3(positionDiff.x, transform.position.y, positionDiff.z);
        }

        private Quaternion GetSmoothedRotation(Quaternion rotation, Vector3 positionToLook) => 
            Quaternion.Lerp(rotation, GetTargetRotation(positionToLook), GetSpeedFactor());

        private Quaternion GetTargetRotation(Vector3 positionToLook) => 
            Quaternion.LookRotation(positionToLook);

        private float GetSpeedFactor() => _speed * Time.deltaTime;

        private bool IsHeroInitialized() => _heroTransform != null;
    }
}
using UnityEngine;

namespace Assets.Code
{
    public class NipplyKeyholeBehaviour : MonoBehaviour
    {
        private ActivatableObjectBehaviour _object;
        private Transform _targetTransform;

        public void Awake()
        {
            _object = GetComponentInParent<ActivatableObjectBehaviour>();
            _targetTransform = Camera.main.transform;
        }

        public void ApplyFreeze(float amountOfFreeze)
        {
            _object.Activate();
        }

        public void Update()
        {
            transform.LookAt(_targetTransform);
        }
    }
}

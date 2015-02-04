using UnityEngine;

namespace Assets.Code
{
    public class NipplyKeyholeBehaviour : MonoBehaviour
    {
        private ActivatableObjectBehaviour _object;
        private Transform _playerTransform;

        public void Awake()
        {
            _object = GetComponentInParent<ActivatableObjectBehaviour>();
            _playerTransform = GameObject.FindGameObjectWithTag("player").transform;
        }

        public void ApplyFreeze(float amountOfFreeze)
        {
            _object.Activate();
        }

        public void Update()
        {
            transform.LookAt(_playerTransform);
            transform.Rotate(Vector3.up, 270);
        }
    }
}

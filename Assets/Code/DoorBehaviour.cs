using UnityEngine;

namespace Assets.Code
{
    [RequireComponent(typeof(Collider))]
    public class DoorBehaviour : MonoBehaviour
    {
        public float LerpSpeed = 1f;
        public float FallSpeed = 1f;

        private Vector3 _basePosition;

        private Vector3 _oldPosition;
        private Vector3 _targetPosition;
        private float _lerpyProgress;

        public void Start()
        {
            AstarPath.active.UpdateGraphs(collider.bounds);

            _basePosition = transform.position;
            _oldPosition = transform.position;
            _targetPosition = transform.position;
            _lerpyProgress = 0f;
        }

        public void RaiseWall(float amountToRaise)
        {
            _targetPosition = transform.position + new Vector3(0, amountToRaise, 0);
            if (_targetPosition.y < _basePosition.y)
                _targetPosition = _basePosition;

            _lerpyProgress = LerpSpeed;
        }

        public void Update()
        {
            // move ourselves down if we're higher than base
            if(_targetPosition.y > _basePosition.y)
                RaiseWall(-Time.deltaTime * FallSpeed);

            if (_lerpyProgress > 0)
            {
                _lerpyProgress -= Time.deltaTime;
                transform.position = Vector3.Lerp(_oldPosition, _targetPosition, _lerpyProgress);
                if (_lerpyProgress <= 0)
                    _oldPosition = _targetPosition;
            }
        }
    }
}

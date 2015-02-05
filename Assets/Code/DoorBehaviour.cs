using UnityEngine;

namespace Assets.Code
{
    [RequireComponent(typeof(Collider))]
    public class DoorBehaviour : ActivatableObjectBehaviour
    {
        public float Speed = 1f;
        public float ShakeScale = 1f;
        public float MaximumShake = 0.1f;
        public bool GoesUp;

        private Vector3 _basePosition;
        private Vector3 _technicalPosition;
        private float _shakeAmount;

        public void Start()
        {
            AstarPath.active.UpdateGraphs(collider.bounds);

            _basePosition = transform.position;
            _technicalPosition = transform.position;
            _shakeAmount = 0f;
        }

        public override void Activate()
        {
            var delta = new Vector3(0, Speed*Time.deltaTime*3, 0);
            _technicalPosition += delta * (GoesUp ? 1f : -1f);
            _shakeAmount += ShakeScale*Time.deltaTime;
        }

        public void Update()
        {
            _shakeAmount -= Time.deltaTime;
            if (_shakeAmount < 0) _shakeAmount = 0;

            // move ourselves down if we're higher than base
            if ((GoesUp && transform.position.y > _basePosition.y) ||
               (!GoesUp && transform.position.y < _basePosition.y))
            {
                var delta = new Vector3(0, Speed*Time.deltaTime, 0);
                _technicalPosition -= delta*(GoesUp ? 1f : -1f);
                _shakeAmount += ShakeScale*Time.deltaTime;
            }

            if (_shakeAmount > MaximumShake)
                _shakeAmount = MaximumShake;

            transform.position = _technicalPosition + new Vector3(Random.Range(-_shakeAmount, _shakeAmount), 0,
                                                                  Random.Range(-_shakeAmount, _shakeAmount));
        }
    }
}

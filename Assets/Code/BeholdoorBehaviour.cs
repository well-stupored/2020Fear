using UnityEngine;

namespace Assets.Code
{
    public class BeholdoorBehaviour : ActivatableObjectBehaviour
    {
        public float BobSpeed = 0.1f;
        public float BobDistance = 1f;
        private float _baseBob;
        private bool _isBobbingUp;

        public float HideTime = 3f;
        private float _currentHideProgress;
        private bool _isHiding;

        private ParticleSystem _particles;
        private MeshRenderer _renderer;
        private SphereCollider _collider;

        public void Start()
        {
            _particles = GetComponentInChildren<ParticleSystem>();
            _renderer = GetComponentInChildren<MeshRenderer>();
            _collider = GetComponentInChildren<SphereCollider>();
            _particles.Stop();

            AstarPath.active.UpdateGraphs(_collider.bounds);

            _isBobbingUp = true;
            _baseBob = transform.position.y;

            StopHiding();
        }

        public override void Activate()
        {
            _isHiding = true;

            _renderer.enabled = false;
            _collider.enabled = false;
            _particles.Play();

            _currentHideProgress = HideTime;
        }

        private void StopHiding()
        {
            _isHiding = false;

            _renderer.enabled = true;
            _collider.enabled = true;
            _particles.Stop();

            _currentHideProgress = 0f;
        }

        public void Update()
        {
            if (_isHiding)
            {
                _currentHideProgress -= Time.deltaTime;
                if(_currentHideProgress <= 0)
                    StopHiding();
            }

            if (_isBobbingUp && transform.position.y > _baseBob + BobDistance)
                _isBobbingUp = false;
            if (!_isBobbingUp && transform.position.y < _baseBob)
                _isBobbingUp = true;

            transform.position += new Vector3(0, (_isBobbingUp) ? BobSpeed : -BobSpeed, 0);
        }
    }
}

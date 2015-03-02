using UnityEngine;

namespace Assets.Code
{
    public class SunNodeBehaviour : MonoBehaviour
    {
        private Vector3 _axis;
        private float _speed;

        public void Start()
        {
            _axis = new Vector3(Random.Range(-1, 1), Random.Range(-5, 5), Random.Range(-1, 1)).normalized;
            _speed = Random.Range(-1, 1);
        }

        public void Update()
        {
            transform.Rotate(_axis, _speed * Time.deltaTime);
        }
    }
}

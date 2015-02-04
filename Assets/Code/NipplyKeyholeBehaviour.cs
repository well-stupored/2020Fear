using UnityEngine;

namespace Assets.Code
{
    public class NipplyKeyholeBehaviour : MonoBehaviour
    {
        private DoorBehaviour _wall;

        public void Awake()
        {
            _wall = GetComponentInParent<DoorBehaviour>();
        }

        public void ApplyFreeze(float amountOfFreeze)
        {
            _wall.transform.position -= new Vector3(0, -amountOfFreeze, 0);
        }
    }
}

using UnityEngine;

namespace Assets.Code
{
    public class SunKingBehaviour : MonoBehaviour
    {
        public void Update()
        {
            transform.Rotate(Vector3.up, Time.deltaTime * 10);
        }
    }
}

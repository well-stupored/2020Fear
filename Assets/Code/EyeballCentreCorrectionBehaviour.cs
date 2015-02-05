using UnityEngine;

namespace Assets.Code
{
    public class EyeballCentreCorrectionBehaviour : MonoBehaviour
    {
        public float Jitter = 2f;

        public void Start()
        {
            transform.localRotation = Quaternion.Euler(27.819f, -71.927f, 10.126f);
        }

        public void Update()
        {
            transform.localRotation = Quaternion.Euler(27.819f + Random.Range(-Jitter, Jitter),
                                                  -71.927f + Random.Range(-Jitter, Jitter),
                                                  10.126f + Random.Range(-Jitter, Jitter));
        }
    }
}

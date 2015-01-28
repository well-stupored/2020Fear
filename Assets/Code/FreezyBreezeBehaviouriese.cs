using UnityEngine;

namespace Assets.Code
{
    public class FreezyBreezeBehaviouriese : MonoBehaviour
    {
        public void Start()
        {
            particleSystem.Emit(1);
        }

        public void Update()
        {
            if(particleSystem.particleCount < 1)
                Destroy(gameObject);
        }
    }
}

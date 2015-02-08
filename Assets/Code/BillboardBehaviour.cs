using UnityEngine;

namespace Assets.Code
{
    public class BillboardBehaviour : MonoBehaviour
    {
        public Transform Target;

        public void Awake()
        {
            if (Target == null && Camera.main != null)
                Target = Camera.main.transform;
        }

        public void Update()
        {
            if (Target == null && Camera.main != null)
                Target = Camera.main.transform;

            if (Target != null)
            {
                var v = Target.position - transform.position;

                v.x = v.z = 0.0f;

                transform.LookAt(Target.position - v);

                transform.Rotate(0, 180, 0);
            }
        }
    }
}

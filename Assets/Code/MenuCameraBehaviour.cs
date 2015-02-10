using UnityEngine;

namespace Assets.Code
{
    public class MenuCameraBehaviour : MonoBehaviour
    {
        public Vector3 Target = Vector3.zero;
        public float CameraSpeed = 2;

        public void Update()
        {
            transform.RotateAround(Target, Vector3.up, CameraSpeed * Time.deltaTime);
            transform.LookAt(Target);
        }
    }
}

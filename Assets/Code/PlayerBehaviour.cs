using UnityEngine;

namespace Assets.Code
{
    [RequireComponent (typeof(Rigidbody))]
    public class PlayerBehaviour : MonoBehaviour
    {
        public GameObject FreezyBreezePrefab;
        public float Speed = 5;
        public Quaternion AimDirection = Quaternion.identity;

        private GazePointDataComponent _gazePoint;

        public void Awake()
        {
            _gazePoint = GetComponent<GazePointDataComponent>();
        }

        // Update is called once per frame
        public void Update ()
        {
            var targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
	        rigidbody.AddForce(transform.TransformDirection(targetVelocity * Speed));

            if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

            if (Input.GetMouseButton(0)) RaytraceFreeze();
            EyeGazeFreeze();
        }

        private void RaytraceFreeze()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                hit.collider.gameObject.SendMessage("ApplyFreeze", 4f, SendMessageOptions.DontRequireReceiver);
                Instantiate(FreezyBreezePrefab, hit.point, Quaternion.identity);
            }
        }

        private void EyeGazeFreeze()
        {
            if (!_gazePoint.LastGazePoint.IsValid) return;

            RaycastHit hit;
            var cameraGazeRay = Camera.main.ScreenPointToRay(_gazePoint.LastGazePoint.Screen);
            if (Physics.Raycast(cameraGazeRay, out hit))
            {
                hit.collider.gameObject.SendMessage("ApplyFreeze", 4f, SendMessageOptions.DontRequireReceiver);
                Instantiate(FreezyBreezePrefab, hit.point, Quaternion.identity);
            }
        }
    }
}

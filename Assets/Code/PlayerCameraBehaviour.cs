using UnityEngine;

namespace Assets.Code
{
    public class PlayerCameraBehaviour : MonoBehaviour {
        public PlayerBehaviour TargetPlayer;
        public float CameraDistance = -2;
        public float CameraXDelta = 1;
        public float CameraYDelta = 1;

        public float CameraSpeed = 2;

        public float MinYRotation = -20;
        public float MaxYRotation = 90;

        private float _cameraXRotation;
        private float _cameraYRotation;

		public void Start()
		{
			_cameraXRotation = 211;
		}

        public void Update()
        {
            if (TargetPlayer == null) return;

            _cameraXRotation += Input.GetAxis ("Mouse X") * CameraSpeed * Time.deltaTime * 60f;
            _cameraYRotation -= Input.GetAxis ("Mouse Y") * CameraSpeed * Time.deltaTime * 60f;

            _cameraYRotation = ClampAngle (_cameraYRotation,
                MinYRotation, MaxYRotation);

            var rotation = Quaternion.Euler(
                new Vector3(_cameraYRotation, _cameraXRotation, 0));

            transform.rotation = rotation;
            transform.position = TargetPlayer.transform.position + 
                                 // use transform.TransformDirection instead
                                 transform.TransformVector(
                                     new Vector3 (CameraXDelta, CameraYDelta,
                                         CameraDistance));
		
            TargetPlayer.transform.rotation = Quaternion.Euler (0, _cameraXRotation, 0);

            TargetPlayer.AimDirection = rotation;
        }

        private static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F)
                angle += 360F;
            if (angle > 360F)
                angle -= 360F;
            return Mathf.Clamp(angle, min, max);
        }

    }
}

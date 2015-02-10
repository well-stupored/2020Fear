using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code
{
    [RequireComponent (typeof(Rigidbody))]
    public class PlayerBehaviour : MonoBehaviour
    {
        public Camera MainCamera;
        public GameObject FreezyBreezePrefab;
        public float Speed = 5;
        public Quaternion AimDirection = Quaternion.identity;

        private GazePointDataComponent _gazePoint;
        private Image _crosshairImage;
        private Image _flashlightImage;

        // Vars for smoothing the gaze point / averaging
        private const int NumOfPointsToAvg = 100;
        private List<Vector2> _gazePoints;
        private int _counter;
        private Vector2 _avgGazePoint;
        private Vector2 _finalGazePoint;

        public void Awake()
        {
            _crosshairImage = GameObject.Find("play_canvas").transform.FindChild("crosshair_image").GetComponent<Image>();
            _flashlightImage = GameObject.Find("play_canvas").transform.FindChild("flashlight_image").GetComponent<Image>();

            _gazePoint = GetComponent<GazePointDataComponent>();

            _gazePoints = new List<Vector2>();
            for (var i = 0; i < NumOfPointsToAvg; i++)
                _gazePoints.Add(new Vector2(Screen.width / 2f, Screen.height / 2f));
            _counter = 0;
        }

        public void Start()
        {
            _flashlightImage.transform.localScale = new Vector2(Screen.width / 1600f, Screen.height / 900f);

            _flashlightImage.transform.position = new Vector2(0,0);
            _crosshairImage.transform.position = new Vector2(0,0);
        }

        // Update is called once per frame
        public void Update ()
        {
            //temp

            _crosshairImage.transform.position = Input.mousePosition;
            _flashlightImage.transform.position = Input.mousePosition;

			var targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
	        //_controller.SimpleMove(transform.TransformDirection(targetVelocity * Speed));

            // Add the current gaze point to our list
            if (!Single.IsNaN(_gazePoint.LastGazePoint.Screen.x) && !Single.IsNaN(_gazePoint.LastGazePoint.Screen.y))
            {
                // if the new gaze point is far enough from our old info, we flush the entire gazePoint List and restart;
                if ((_gazePoint.LastGazePoint.Screen - _gazePoints[_counter]).magnitude > 50.0f)
                {
                    for (var i = 0; i < NumOfPointsToAvg; i++)
                        _gazePoints[i] = _gazePoint.LastGazePoint.Screen;
                    _counter = 0;
                }
                else
                {
                    _gazePoints[_counter] = _gazePoint.LastGazePoint.Screen;
                    _counter = (_counter < _gazePoints.Count - 1 ? _counter + 1 : 0);
                }
            }

            // calculate the average gaze point
            _avgGazePoint = Vector2.zero;
            foreach (var point in _gazePoints)
                _avgGazePoint += point;
            _avgGazePoint /= _gazePoints.Count;
            _finalGazePoint = Vector2.Lerp(_finalGazePoint, _avgGazePoint, 0.2f);

            if (Input.GetMouseButton(0))         RaytraceFreeze(new Vector2(Screen.width / 2f, Screen.height / 2f));
            if(_gazePoint.LastGazePoint.IsValid) RaytraceFreeze(_finalGazePoint);
        }

        private void RaytraceFreeze(Vector2 screenPosition)
        {
            var ray = MainCamera.ScreenPointToRay(screenPosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                hit.collider.gameObject.SendMessage("ApplyFreeze", 4f, SendMessageOptions.DontRequireReceiver);
                Instantiate(FreezyBreezePrefab, hit.point, Quaternion.identity);
            }

            _crosshairImage.transform.position = screenPosition;
            _flashlightImage.transform.position = screenPosition;
        }
    }
}

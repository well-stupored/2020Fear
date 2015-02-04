using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    [RequireComponent (typeof(Rigidbody))]
    [RequireComponent (typeof(CharacterController))]
    public class PlayerBehaviour : MonoBehaviour
    {
        public GameObject FreezyBreezePrefab;
        public float Speed = 5;
        public Quaternion AimDirection = Quaternion.identity;

        private GazePointDataComponent _gazePoint;
        private UiLinkerBehaviour _ui;
        private CharacterController _controller;

        // Vars for smoothing the gaze point / averaging
        private const int NumOfPointsToAvg = 100;
        private List<Vector2> _gazePoints;
        private int _counter;
        private Vector2 _avgGazePoint;
        private Vector2 _finalGazePoint;

        public void Awake()
        {
            _gazePoint = GetComponent<GazePointDataComponent>();
            _ui = GameObject.FindGameObjectWithTag("ui_linker").GetComponent<UiLinkerBehaviour>();
            _controller = GetComponent<CharacterController>();
            _gazePoints = new List<Vector2>();
            for (var i = 0; i < NumOfPointsToAvg; i++)
                _gazePoints.Add(new Vector2(Screen.width / 2f, Screen.height / 2f));
            _counter = 0;
        }

        public void Start()
        {
			Screen.lockCursor = true;
            Screen.showCursor = false;

            _ui.FlashlightImage.transform.localScale = new Vector2(Screen.width / 1600f, Screen.height / 900f);
        }

        // Update is called once per frame
        public void Update ()
        {
			var targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
	        _controller.SimpleMove(transform.TransformDirection(targetVelocity * Speed));

            if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
            if (Input.GetKeyDown(KeyCode.P))
            {
                Screen.lockCursor = !Screen.lockCursor;
                Screen.showCursor = !Screen.showCursor;
            }

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
            if(_gazePoint.LastGazePoint.IsValid) RaytraceFreeze(_gazePoint.LastGazePoint.Screen);
        }

        private void RaytraceFreeze(Vector2 screenPosition)
        {
            var ray = Camera.main.ScreenPointToRay(screenPosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                hit.collider.gameObject.SendMessage("ApplyFreeze", 4f, SendMessageOptions.DontRequireReceiver);
                Instantiate(FreezyBreezePrefab, hit.point, Quaternion.identity);
            }

            _ui.CrosshairImage.transform.position = screenPosition;
            _ui.FlashlightImage.transform.position = screenPosition;
        }
    }
}

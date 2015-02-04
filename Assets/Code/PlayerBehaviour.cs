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

        public void Awake()
        {
            _gazePoint = GetComponent<GazePointDataComponent>();
            _ui = GameObject.FindGameObjectWithTag("ui_linker").GetComponent<UiLinkerBehaviour>();
            _controller = GetComponent<CharacterController>();
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

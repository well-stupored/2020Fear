using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code
{
    [RequireComponent (typeof(CharacterController)), RequireComponent (typeof(Seeker))]
    public class EnemyBehaviour : AIPath
    {
        public Color ChaseColor = Color.red;
        public Color FrozeColor = Color.blue;
        public float MaxFreeze = 100f;
        public float UnfreezePerSecond = 1f;
        private float _currentFreeze;

        private Text _timerText;
        private TrailRenderer _line;

        public new void Awake()
        {
            _timerText = GetComponentInChildren<Text>();
            _line = GetComponent<TrailRenderer>();

            base.Awake();
        }

        public new void Start()
        {
            var playerObject = GameObject.FindGameObjectWithTag("player");
            target = playerObject.transform;

            _currentFreeze = MaxFreeze;

            base.Start();
        }

        protected new void Update()
        {
            // if we are not frozen, chase
            if (_currentFreeze <= 0)
            {
                HandleChase();
                _timerText.text = "";
            }
            // otherwise we unfreeze and update our timer
            else
            {
                _currentFreeze -= UnfreezePerSecond * Time.deltaTime * 60f;
                _timerText.text = "" + (int) _currentFreeze;
            }

            // update our color
            renderer.material.color = Color.Lerp(ChaseColor, FrozeColor, _currentFreeze/MaxFreeze);
            _line.material.color = Color.Lerp(ChaseColor, FrozeColor, _currentFreeze / MaxFreeze);
        }

        private void HandleChase()
        {
            //Calculate desired velocity
            var dir = CalculateVelocity(tr.position);

            //Rotate towards targetDirection (filled in by CalculateVelocity)
            RotateTowards(targetDirection);
            dir.y = 0;

            controller.SimpleMove(dir);
        }

        public void ApplyFreeze(float freezyBreezyLemonSqueezy)
        {
            _currentFreeze += freezyBreezyLemonSqueezy * Time.deltaTime * 60;
            if (_currentFreeze > MaxFreeze)
                _currentFreeze = MaxFreeze;
        }
    }
}
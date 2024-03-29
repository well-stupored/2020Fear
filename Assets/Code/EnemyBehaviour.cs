﻿using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code
{
    [RequireComponent (typeof(CharacterController)), RequireComponent (typeof(Seeker))]
    public class EnemyBehaviour : AIPath
    {
        public float WanderSpeed = 5f;
        public float ChaseSpeed = 50f;

        public AudioClip ChaseSound;
        public AudioClip WanderSound;
        public AudioClip FrozedSound;
        public float ChaseVolume;
        public float WanderVolume;
        public float FrozedVolume;

        public Color ChaseColor = Color.red;
        public Color FrozeColor = Color.cyan;
        public float MaxFreeze = 100f;
        public float UnfreezePerSecond = 1f;
        private float _currentFreeze;

        private bool _isChasing;

        private Text _timerText;
        private AudioSource _audio;

        public new void Awake()
        {
            _timerText = GetComponentInChildren<Text>();
            _audio = GetComponentInChildren<AudioSource>();

            base.Awake();
        }

        public new void Start()
        {
         	var playerObject = GameObject.FindGameObjectWithTag("player");
            target = playerObject.transform;

            _currentFreeze = MaxFreeze;
            _isChasing = false;

            SetState(FrozedSound, FrozedVolume, 0);

            base.Start();
        }

        protected new void Update()
        {
            // update whether we are chasing or not
            _isChasing = CanSeeTarget() && _currentFreeze <= 0;

            if (_isChasing && _currentFreeze <= 0)
                SetState(ChaseSound, ChaseVolume, ChaseSpeed);
            else if (!_isChasing && _currentFreeze <= 0)
                SetState(WanderSound, WanderVolume, WanderSpeed);

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
            SetState(FrozedSound, FrozedVolume, 0f);

            _currentFreeze += freezyBreezyLemonSqueezy * Time.deltaTime * 60;
            if (_currentFreeze > MaxFreeze)
                _currentFreeze = MaxFreeze;
        }

        private void SetState(AudioClip clip, float volume, float newSpeed)
        {
            if (_audio.clip.name == clip.name) return;

            _audio.Stop();
            _audio.clip = clip;
            _audio.volume = volume;
            _audio.Play();

            speed = newSpeed;
        }

        private bool CanSeeTarget()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, target.position - transform.position, out hit))
                return hit.collider.tag == "player";

            return false;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.tag == "player" && _currentFreeze <= 0)
                other.gameObject.SendMessage("Kill", SendMessageOptions.DontRequireReceiver);
        }
    }
}
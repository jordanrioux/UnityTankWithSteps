using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tank
{ 
    public class TankShooting : MonoBehaviour
    {
        [SerializeField] private Rigidbody shell;
        [SerializeField] private Transform fireTransform;

        [SerializeField] private AudioSource shootingAudio;
        [SerializeField] private AudioClip chargingAudioClip;
        [SerializeField] private AudioClip fireAudioClip;
        [SerializeField] private Slider aimSlider;
        [SerializeField] private float minLaunchForce = 15f;
        [SerializeField] private float maxLaunchForce = 30f;
        [SerializeField] private float maxChargeTime = 0.75f;
        [SerializeField] private int playerNumber;

        private bool _fired;
        private float _currentLaunchForce;
        private float _chargeSpeed;

        private void OnEnable()
        {
            _currentLaunchForce = minLaunchForce;
        }

        private void Start()
        {
            _chargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime;
        }

        private void Update()
        {
            aimSlider.value = minLaunchForce;
            UpdateAndHandleShootingState();
        }

        private void UpdateAndHandleShootingState()
        {
            // Fire the shell if we reach the maximum launch force
            if (_currentLaunchForce >= maxLaunchForce && !_fired)
            {
                _currentLaunchForce = maxLaunchForce;
                Fire();
            }
            // When fire button is pressed, prepare the charging and play the sound
            else if (Input.GetButtonDown($"Fire{playerNumber}"))
            {
                _fired = false;
                _currentLaunchForce = minLaunchForce;

                // Play charging sound
                shootingAudio.clip = chargingAudioClip;
                shootingAudio.Play();
            }
            // While fire button is being held, increase the launch force
            else if (Input.GetButton($"Fire{playerNumber}") && !_fired)
            {
                _currentLaunchForce += _chargeSpeed * Time.deltaTime;
                aimSlider.value = _currentLaunchForce;
            }
            // When fire button is released, fire the shell
            else if (Input.GetButtonUp($"Fire{playerNumber}") && !_fired)
            {
                Fire();
            }
        }

        private void Fire()
        {
            _fired = true;
            var shellInstance = Instantiate(shell, fireTransform.position, fireTransform.rotation);
            shellInstance.velocity = fireTransform.forward * _currentLaunchForce;

            // Play shooting sound
            shootingAudio.clip = fireAudioClip;
            shootingAudio.Play();

            _currentLaunchForce = minLaunchForce;
        }
    }
}
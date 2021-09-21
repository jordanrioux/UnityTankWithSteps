using UnityEngine;

namespace Tank
{
    public class TankMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 12f;
        [SerializeField] private float turnSpeed = 180f;
        [SerializeField] private float pitchRange = 0.2f;
        [SerializeField] private AudioSource movementAudio;
        [SerializeField] private AudioClip engineDrivingAudioClip;
        [SerializeField] private AudioClip engineIdleAudioClip;
        [SerializeField] private int playerNumber;

        private Rigidbody _rigidbody;
        private float _movementInputValue;
        private float _turnInputValue;
        private float _originalPitch; 
    
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            _rigidbody.isKinematic = false;
            _movementInputValue = 0f;
            _turnInputValue = 0f;
        }


        private void OnDisable ()
        {
            _rigidbody.isKinematic = true;
        }

        private void Start()
        {
            _originalPitch = movementAudio.pitch;
        }

        private void FixedUpdate()
        {
            Move();
            Turn();
        }

        private void Update()
        {
            // Gets the corresponding input axis based on the player number: Vertical1, Vertical2, etc.
            // The axes need to be created to be available: Edit > Project Settings > Input Manager
            _movementInputValue = Input.GetAxis($"Vertical{playerNumber}");
            _turnInputValue = Input.GetAxis($"Horizontal{playerNumber}");
            PlayAudio();
        }
    
        /**
         *  The movement is based on vectors:
         *      transform.forward is an unit vector to determine the moving direction only.
         *      transform.forward is multiplied by the speed to determine the distance.
         *      The _movementInputValue determines if the tank is moving forward or backward (positive/negative value).
         *       Time.deltaTime is only to move the tank since the last Update elapsed time (e.g. based on elapsed frames).
         */
        private void Move()
        {
            var movement = transform.forward * _movementInputValue * speed * Time.deltaTime;
            _rigidbody.MovePosition(_rigidbody.position + movement);
        }
    
        /**
         *  The turn is based on angles:
         *      _turnInputValue determines the angle the tank is rotating to.
         *      The turnSpeed determines at which speed the tank is rotating.
         *      Time.deltaTime is only to rotate the tank since the last Update elapsed time (e.g. based on elapsed frames)
         */
        private void Turn()
        {
            var turn = _turnInputValue * turnSpeed * Time.deltaTime;
            var turnRotation = Quaternion.Euler(0f, turn, 0f);
            _rigidbody.MoveRotation(_rigidbody.rotation * turnRotation);
        }
    
        /**
         *  Play the audio based on the tank movement. 
         *  By default, the EngineIdle audio is always playing but is replaced by the EngineDriving audio when the tank is moving or rotating.
         *  NOTE: The code should refactored into smaller methods.
         */
        private void PlayAudio()
        {
            if (Mathf.Abs(_movementInputValue) < 0.1f && Mathf.Abs(_turnInputValue) < 0.1f)
            {
                if (movementAudio.clip == engineDrivingAudioClip)
                {
                    movementAudio.clip = engineIdleAudioClip;
                    movementAudio.pitch = Random.Range(_originalPitch - pitchRange, _originalPitch + pitchRange);
                    movementAudio.Play();
                }   
            }
            else
            {
                if (movementAudio.clip == engineIdleAudioClip)
                {
                    movementAudio.clip = engineDrivingAudioClip;
                    movementAudio.pitch = Random.Range(_originalPitch - pitchRange, _originalPitch + pitchRange);
                    movementAudio.Play();
                }  
            }
        }
    }
}

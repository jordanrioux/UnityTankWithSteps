using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace Tank
{
    public class TankHealth : NetworkBehaviour
    {
        [SerializeField] private float startingHealth = 100f;
        [SerializeField] private Slider slider;
        [SerializeField] private Image fillImage;
        [SerializeField] private Color fullHealthColor = Color.green;
        [SerializeField] private Color lowHealthColor = Color.red;
        [SerializeField] private GameObject explosionPrefab;

        [SyncVar(hook = "OnChangeHealth")]
        private float _currentHealth;
        
        private AudioSource _explosionAudio;
        private ParticleSystem _explosionParticles;
        private bool Alive => _currentHealth <= 0f;

        private void Start()
        {
            _explosionParticles = Instantiate(explosionPrefab).GetComponent<ParticleSystem>();
            _explosionAudio = _explosionParticles.GetComponent<AudioSource>();
            _explosionParticles.gameObject.SetActive(false);
          
            _currentHealth = startingHealth;
        }

        private void OnChangeHealth(float oldValue, float newValue)
        {
            slider.value = _currentHealth;
            fillImage.color = Color.Lerp(lowHealthColor, fullHealthColor, _currentHealth / startingHealth);            
        }

        [ClientRpc]
        private void RpcSetHealthUi()
        {
            slider.value = _currentHealth;
            fillImage.color = Color.Lerp(lowHealthColor, fullHealthColor, _currentHealth / startingHealth);
        }

        public void TakeDamage(float amount)
        {
            _currentHealth -= amount;            
            if (Alive)
            {
                RpcOnDeath();
            }
        }
        
        [ClientRpc]
        private void RpcOnDeath()
        {
            _explosionParticles.transform.position = transform.position;
            _explosionParticles.gameObject.SetActive(true);
            _explosionParticles.Play();

            _explosionAudio.Play();

            gameObject.SetActive(false);
        }
    }
}

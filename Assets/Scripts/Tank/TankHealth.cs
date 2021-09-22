using UnityEngine;
using UnityEngine.UI;

namespace Tank
{
    public class TankHealth : MonoBehaviour
    {
        [SerializeField] private float startingHealth = 100f;
        [SerializeField] private Slider slider;
        [SerializeField] private Image fillImage;
        [SerializeField] private Color fullHealthColor = Color.green;
        [SerializeField] private Color lowHealthColor = Color.red;
        [SerializeField] private GameObject explosionPrefab;

        private AudioSource _explosionAudio;
        private ParticleSystem _explosionParticles;
        private float _currentHealth;
        private bool _alive = true;

        private void Start()
        {
            _explosionParticles = Instantiate(explosionPrefab).GetComponent<ParticleSystem>();
            _explosionAudio = _explosionParticles.GetComponent<AudioSource>();
            _explosionParticles.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _currentHealth = startingHealth;
            _alive = true;

            SetHealthUI();
        }

        private void SetHealthUI()
        {
            slider.value = _currentHealth;
            fillImage.color = Color.Lerp(lowHealthColor, fullHealthColor, _currentHealth / startingHealth);
        }

        public void TakeDamage(float amount)
        {
            _currentHealth -= amount;
            SetHealthUI();

            if (_currentHealth <= 0f && _alive)
            {
                OnDeath();
            }
        }

        private void OnDeath()
        {
            _alive = false;
            _explosionParticles.transform.position = transform.position;
            _explosionParticles.gameObject.SetActive(true);
            _explosionParticles.Play();

            _explosionAudio.Play();

            gameObject.SetActive(false);
        }
    }
}

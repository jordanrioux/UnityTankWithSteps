using System.Collections;
using System.Collections.Generic;
using Tank;
using UnityEngine;

namespace Shell
{ 
    public class ShellExplosion : MonoBehaviour
    {
        [SerializeField] private LayerMask tankMask;
        [SerializeField] private ParticleSystem explosionParticles;
        [SerializeField] private AudioSource explosionAudio;

        [SerializeField] private float maxDamage = 100f;
        [SerializeField] private float explosionForce = 1000f;
        [SerializeField] private float maxLifeTime = 2f;
        [SerializeField] private float explosionRadius = 5f;

        private void Start()
        {
            Destroy(gameObject, maxLifeTime);
        }

        // For debugging purposes, use Gizmos to draw the collision in the Scene panel
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }

        private void OnTriggerEnter(Collider other)
        {
            var colliders = Physics.OverlapSphere(transform.position, explosionRadius, tankMask);
            foreach (var c in colliders)
            {
                var targetRigidbody = c.GetComponent<Rigidbody>();
                targetRigidbody?.AddExplosionForce(explosionForce, transform.position, explosionRadius);

                var targetHealth = targetRigidbody?.GetComponent<TankHealth>();
                if (!targetHealth)
                    continue;

                var damage = CalculateDamage(targetRigidbody.position);
                targetHealth.TakeDamage(damage);
            }

            explosionParticles.transform.parent = null;
            explosionParticles.Play();
            explosionAudio.Play();

            Destroy(explosionParticles.gameObject, explosionParticles.main.duration);
            Destroy(gameObject);
        }

        private float CalculateDamage(Vector3 targetPosition)
        {
            // Find distance between explosion and target
            var explosionToTarget = targetPosition - transform.position;
            var explosionDistance = explosionToTarget.magnitude; // between 0 and radius

            // If close, high damage. If far, low damage
            var relativeDistance = (explosionRadius - explosionDistance) / explosionRadius;
            var damage = Mathf.Max(0f, (relativeDistance * maxDamage));
            return damage;
        }
    }
}

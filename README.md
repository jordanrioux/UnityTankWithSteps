# Introduction

The `4/shells` starts from the `3/tank-health` branch code. The steps will guide you into creating the shell prefab that will be used as the shooting projectile to attack the tanks.
# Steps that have already been done

1. Drag the **Shell** model from the **Models** folder to the **Hierarchy** panel to create a **Shell** object
2. Add a **Capsule Collider** component to the **Shell** object and set:
    * Check **Is Trigger**
    * **Center** to (0, 0, 0.2)
    * **Radius** to 0.15
    * **Height** to 0.55
    * **Direction** to Z-axis
3. Add a **Rigidbody** component to the **Shell** object
4. Add an **Light** component to **Shell** object
5. Create an **empty Game Object** and name it **ShellExplosion**
6. Drag the **ShellExplosion** object to the **Shell** object to make it a child
7. Add an **Audio Source** component to **ShellExplosion** object and set:
    * **Audio Clip** to **ShellExplosion**
    * Uncheck **Play on Awake**
8. Add a **Particle System** component to **ShellExplosion** and set:
    * Make sure **Position** is set to (0, 0, 0)
    * **Duration** to 1.50
    * Uncheck **Looping**
    * Uncheck **Play on Awake**
    * **Start Delay** to 0
    * **Start Lifetime** to 1.5
    * **Start Speed** to 2
    * **Start Size** to **Random between two constants** which are 1 and 2.5
    * Under **Emission**:
        * **Rate over Time** to 0
        * **Bursts** click + to add a default line
            * **Count** to 8
    * Under **Shape**:
        * **Shape** to **Circle**
        * **Radius** to 0.367
        * **Radius Thickness** to 0
    * Under **Renderer**:
        * Make sure Material is set to **Explosion**
9. Create the **ShellExplosion** script in the **Scripts/Shell** folder
10. Add the **ShellExplosion** script as a component to the **Shell** object
11. Double click the **ShellExplosion** script to open it in your IDE

The serialized and private fields
```csharp
[SerializeField] private LayerMask tankMask;
[SerializeField] private ParticleSystem explosionParticles;
[SerializeField] private AudioSource explosionAudio;

[SerializeField] private float maxDamage = 100f;
[SerializeField] private float explosionForce = 1000f;
[SerializeField] private float maxLifeTime = 2f;
[SerializeField] private float explosionRadius = 5f;
```

The Unity events
```csharp
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
```
The specific code of the movement
```csharp
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
```

12. Click on the **Shell** object:
    * For **ShellExplosion** script fields:
        * Set **Tank Mask** to **Players**
        * Drag the **ShellExplosion** child object to the **Explosion Particles** field
        * Drag the **ShellExplosion** child object to the **Explosion Audio** field

13. Drag the **Shell** object into the **Prefabs** folder to make it a prefab
# Explanation

1. We create a **Shell** object from the model provided. We add a rigidbody to ensure the physics is affecting the object and we use a capsule collider to add collision to the shell. The provided values are simply to make sure the collision is matching the shell size.
2. A **Light** is added to the **Shell** simply for visual effects.
3. We create a **ShellExplosion** object and make it a child to the **Shell** object. The **ShellExplosion** will be used to handle the animation and sound effect of when a shell collides and explodes so we add an audio source and a particle system to it.
4. The **ShellExplosion** is a script to determine if a tank is hit by a shell and how much damage it should take. The logic applied is basically making a sphere around the shell impact and retrieve all the targets (e.g. tanks) in the area and apply a damage value based on the relative distance between the impact center and the tank position. A tank closer to the impact will take more damage while a tank further away will take less damage.
5. The Gizmos are used to display a wire frame sphere of the impact/collision in the Scene panel to help out in debugging and make sure all is working well.

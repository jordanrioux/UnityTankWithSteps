# Introduction

The `5/tank-shooting` starts from the `4/shells` branch code. The steps will guide you into creating an aim slider using a slider UI element for shooting shells. The force at which the shell is launched will be increased while the button is behind held.
# Steps that have already been done

1. Select the **Tank** object:
    * Right-click on the **Tank** and choose **Create empty** and name it **FireTransform**
    * Set the **FireTransform** values:
        * **Position** to (0, 1.7, 1.35)
        * **Rotation** to (350, 0, 0)
2. Create a new **Slider** (UI > Slider) and name it **AimSlider**:
3. Expand all the child objects of the **AimSlider** object
    * Delete the **Background** object
    * Delete the **Handle Slide Area** object
4. Select the **AimSlider** and set:
    * Uncheck **Interactable**
    * **Transition** to **None**
    * **Direction** to **Bottom to Top**
    * **Min Value** to 15
    * **Max Value** to 30
5. Multi-select **AimSlider**, **Fill Area** and **Fill**
6. Click on the **Anchor Presets** dropdown and **Alt-click** on the **lower-right** preset to stretch the GameObjects over the entire canvas
    * It's the square with arrows on the left right under the **Rect Transform**
7. Select the **Fill** object and set:
    * On **Image**:
        * **Source Image** to **Aim Arrow**
8. Select the **AimSlider** object and set:
    * On **Rect Transform**:
        * **(X, Y, Z)** to (1, -9, -1)
        * **(Right, Bottom)** to (1, 3)
9. Create the **TankShooting** script in the **Scripts/Tank** folder
10. Add the **TankShooting** script as a component to the **Tank** object
11. Double click the **TankShooting** script to open it in your IDE

The serialized and private fields
```csharp
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
```

The Unity events
```csharp
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
```
The specific code of the movement
```csharp
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
```

12. Click on the **Tank** object:
    * For **TankShooting** script fields:
        * Drag the **Shell** prefab to the **Shell** field
        * Drag the **FireTransform** object to the **Fire Transform** field
        * Drag the **AimSlider** object to the **Aim Slider** field
        * Drag the second audio source of the tank (the one with no audio assigned) to the **Shooting Audio** field
        * Set the **Charging Clip** to **Shot Charging**
        * Set the **Fire Clip** to **Shot Firing**

13. Make sure to save the Tank prefab
14. Select the **Terrain** object and add a **Mesh Collider** to it

# Explanation

1. To determine the spawning of the shells, we will be using an empty game object **FireTransform**. This will allow us to move the object easily via the editor and the code will simply use its position and rotation to know where the bullet needs to be spawn.
2. We use a **Slider** UI element to present the growing arrow to indicate the launching force. We simply set the values to position it in front of the tank and grow in the proper direction. The graphics are done using a simple 2D image.
3. For the shooting, we are simply doing a few checks for all the possible scenarios. If we reach the maximum launch force, fire it automatically. If the Fire button is pressed, we initialize with the default minimum values and increase the launching force while the button is behind held. We fire the shell if the button is released. For each scenario, we also play the corresponding sound effect (e.g. charging or shooting).
4. We are using the same logic with the **player number** to determine the input axes but we are simply using the default existing axes for now (e.g. Fire1, Fire2, etc.). You can remap them as needed.
5. The **Shell** prefab that we have previously created will handle the the animation and sound effect for the shell explosion.
6. The **Mesh Collider** on the **Terrain** is simply to have the shells collide and explode on contact with it.
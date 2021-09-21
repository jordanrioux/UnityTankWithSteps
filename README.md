# Introduction

The `1/tank-creation` starts from the `master` branch code. The steps will guide you into creating the Tank prefab with its corresponding components for collision, audio and scripting.

The resulting tank will be able to move via a custom script and play corresponding audio clips based on its movement.

# Steps that have already been done

1. Change the **Tank** object (if not created, you can simply drag the **Tank** model from the **Models** folder into the **Hierarchy** panel):
    * Set the **Layer** to **Players** (create a new layer)
    * Choose "No, this object only" when asked if the layer should applied to all child objects or not.
    * Add a **Rigidbody** component and set its values:
        * Expand **Constraints**:
            * Check **Freeze Position** for the Y axis
            * Check **Freeze Rotation** for the X and Z axes
    * Add a **BoxCollider** component and set its values:
        * **Center** to (0, 0.85, 0)
        * **Size** to (1.5, 1.7, 1.6)
    * Add a **Audio Source** component and set its values:
        * **AudioClip** to **EngineIdle** using the circle button
        * Check **Loop**
    * Add another **Audio Source** component and set its values:
        * Uncheck **Play on Awake**
2. Open the **Project Settings** (Edit > Project Settings)
    * Select **Input Manager**
        * Rename **Vertical** to **Vertical1**
        * Rename **Horizontal** to **Horizontal1**
        * Create two new axes for the second player
            * Right-click on **Horizontal1** and **Duplicate Array Element**
            * Rename to **Horizontal2**
            * Assign the hotkeys of your choice for the second player
            * Repeat using **Vertical1** for **Vertical2**
            * NOTE: The hotkeys assigned are i, k, j and l for the second player.
3. Create a **Tank** folder in the **Scripts** folder
4. Create a **TankMovement** script in the **Scripts/Tank** folder
5. Add the **TankMovement** script as a component to the **Tank** object
6. Double the **TankMovement** script to open it in your IDE

The serialized and private fields
```csharp
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
```

The Unity events
```csharp
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
```

The specific code of the movement
```csharp
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
```

7. Click on the **Tank** object:
    * Assign the **TankMovement** scripts fields by dragging or selecting the corresponding objects:
        * **Movement Audio** use the first **AudioSource** of the Tank (the one with an assigned audio clip)
        * **Engine Driving Audio Clip** click on the circle button and select **EngineDriving**
        * **Engine Idle Audio Clip** click on the circle button and select **EngineIdle**
        * **Player Number** to 1
8. Create a **Prefabs** folder
9. Create the **Tank** prefab from the **Tank** object
    * Click and drag the **Tank** object from the **Hierarchy** to the **Prefabs** folder


# Explanation

1. The **Tank** object can be created by dragging the **Tank** model from the **Models** folder to the **Hierarchy** panel.
2. We select the **Tank** object to add/set components and their values for our need:
    * By assigning the **Tank** to a different layer, we will be able to use it layer for our collision logic by only checking the objects that will be on a specific layer (e.g. only tanks will be on the Players layer).
    * Only the **Tank** object needs to be on the layer for our collision logic, the child are not required to be on this layer which is why we don't need to apply the layer to the child objects otherwise our collision logic will be targeting the child objects as well as part of the collision detection.
    * We add a **Rigidbody** to the tank to allow the Unity Physics engine to be applied to it.
    * We freeze the position and the rotation to prevent undesired behaviors on the tank for when we will be adding the explosions, etc. For example, we only want the rotation on the Y axis to ensure the tank doesn't get flipped and can't move anymore. The same logic is applied to his position as we don't want the tank to be propulsed in the air so we freeze the Y axis.
    * We add a **BoxCollider** to the tank to allow the collision detection. By default, the collider might not be adjusted properly to the object so it needs to be resized to match the tank dimension hence the modification to the **Center** and **Size** values.
    * We add a **AudioSource** that will be used for the engine sound. By default, it will play the **EngineIdle** sound but the clip will be changed to **EngineDriving** sound when the tank is moving or rotating which will be done via the **TankMovement** script. As this clip is always running, we check the **Loop** checkbox.
    * We add another **AudioSource** that will be used for other sound effects that will be coming later on. As we are going to control when these sounds are played, we disabled the **Play on Awake** to ensure the sound is not played when the tank is created.
3. We are going to support local multiplayers which mean that we need to create proper **Input Manager** axes for each player for them to have separate keys for moving their tank. By using the **InputAxe** name plus the player number (e.g. Vertical1, Vertical2, etc.), we can easily assigned a different input axis to each player and easily support multiple players.
4. The **TankMovement** script handles the movement and rotation of the tank. The movement is based on vectors and the rotation is based on angles.
5. Once the **Tank** object is created with all its component, we can drag it from **Hierarchy** to a folder (e.g. Prefabs) to create a Prefab of the object. This will allow us to easily create new tank object with all the components already included and correctly set with all their values. Once an object become a prefab, it will appear in **Blue** in the **Hierarchy** panel.

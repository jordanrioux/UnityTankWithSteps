# Introduction

The `3/tank-health` starts from the `2/camera` branch code. The steps will guide you into creating the UI element to display the tank health using a slider which will position in a circle around the tank.
# Steps that have already been done

1. Create a new **Slider** (UI > Slider):
2. Select the **EventSystem** object and set:
    * On **Standalone Input Module**:
        * Change the **Horizontal Axis** to **HorizontalUI**
        * Change the **Vertical Axis** to **VerticalUI**
3. Make sure to go create the **Axes** in the **Input Manager**
    * You can simply duplicate the **Horizontal1** and rename to **HorizontalUI**
    * You can simply duplicate the **Vertical1** and rename to **VerticalUI**
4. Select the **Canvas** object and set:
    * On **Canvas Scaler** component:
        * **Reference Pixels per Unit** to 1
    * On **Canvas** component:
        * **Render Mode** to **World Space**
5. Drag the **Canvas** object onto the **Tank** object to make it a child
6. Select the **Canvas** object and set:
    * Under **Rect Transform**:
        * **Position** to (0, 0.1, 0)
        * **Width** to 3.5
        * **Height** to 3.5
        * **Rotation** to (90, 0, 0)
7. Expand the **Canvas** and all of it's children
8. Select the **HandleSlideArea** and delete it
9. Multi-select **Slider**, **Background**, **Fill Area** and **Fill**
10. Click on the **Anchor Presets** dropdown and **Alt-click** on the **lower-right** preset to stretch the GameObjects over the entire canvas
    * It's the square with arrows on the left right under the **Rect Transform**
11. Select the **Slider** object and set:
    * On **Slider** component:
        * Uncheck **Interactable**
        * **Transition** to **None**
        * **Max Value** to 100
        * **Value** to 100
12. Rename the **Slider** to **HealthSlider**
13. Select the **Background** object and set:
    * On **Image** component:
        * **Source Image** to **Health Wheel** using the circle-select
        * **Color** to (255, 255, 255, 80)
14. Select the **Fill** object and set:
    * On **Image** component:
        * **Source Image** to **Health Wheel** using the circle-select
        * **Color** to (255, 255, 255, 150)
        * **Image Type** to **Filled**
        * **Fill Origin** to **Left**
        * Uncheck **Clockwise**
15. Create the **UI** folder in the **Scripts** folder
16. Create the **UIDirectionControl** script in the **Scripts/UI** folder
16. Add the **UIDirectionControl** script as a component to the **Tank** object
17. Double click the **UIDirectionControl** script to open it in your IDE

The serialized and private fields
```csharp
[SerializeField] private bool useRelativeRotation = true;

private Quaternion _relativeRotation;
```

The Unity events
```csharp
private void Start()
{
    _relativeRotation = transform.parent.localRotation;
}

private void Update()
{
    if (useRelativeRotation)
    {
        transform.rotation = _relativeRotation;
    }
}
```
18. Create an **empty Game Object** and name it **TankExplosion**
19. Add an **Audio Source** component to **TankExplosion** object and set:
    * **Audio Clip** to **Tank Explosion**
    * Uncheck **Play on Awake**
20. Add a **Particle System** component to **TankExplosion** and set:
    * **Duration** to 1.05
    * Uncheck **Looping**
    * Uncheck **Play on Awake**
    * **Start Delay** to 0.15
    * **Start Lifetime** to 0.9
    * **Start Speed** to 20
    * **Start Size** to **Random between two constants** which are 0.05 and 0.3
    * Under **Emission**:
        * **Rate over Time** to 0.34
        * **Bursts** click + to add a default line
    * Under **Renderer**:
        * Make sure Material is set to **Default-Particle**
21. Drag the **TankExplosion** into the **Prefabs** folder to make it a prefab
22. Delete the **TankExplosion** object
23. Create the **TankHealth** script in the **Scripts/Tank** folder
24. Add the **TankHealth** script as a component to the **Tank** object
25. Double click the **TankHealth** script to open it in your IDE

The serialized and private fields
```csharp
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
```

The Unity events
```csharp
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
```
The specific code of the movement
```csharp
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
```

26. Click on the **Tank** object:
    * For **TankHealth** script fields:
        * Drag the **HealthSlider** object to the **Slider** field
        * Drag the **Fill** object to the **Fill Image** field
        * Drag the **TankExplosion** prefab to the **Explosion Prefab** field

27. Select the **Tank** object and click **Overrides > Apply** to update the prefab
# Explanation

1. To display the tank health, we will be using the **Slider** UI element. It can be customized to be a circle and non-interactable. 
2. By default, the canvas will appear huge and out of proportion in the regard to the game world. We are setting a few settings first to bring it back to a "normal" behavior in regard to its dimension in the world and we are also adjusting it to have the proper size so it correctly fit around the tank.
3. As we are only interested in the **Slider** behavior of the UI element, we make sure that the **Slider** is non-interactable by removing components that we don't need so we can use it as a display only (e.g. we don't want the user to be able to slide his health value as he wants).
4. As a slider is interactable by default, there are input axes assigned by default. To ensure no suprises, we simply renamed to a proper "UI" naming such as **HorizontalUI** and **VerticalUI**.
5. The **UIDirectionControl** is a script to determine if the component should use relative positioning or not. If we don't use this script, the slider will not turn around with the tank which would make his health move in weird ways. The script ensure that the slider is being rotated when the tank is being rotated to ensure the position is always the same and the health is easy to see.
6. To play some effects when the tank is destroyed, we create a **TankExplosion** prefab that will be re-used in the script when the tank is taking damage and destroyed. 
7. The **TankExplosion** prefab is simply having an **Audio Source** for the tank explosion sound and a dummy particle system. Currently, the values provided for the particle system are not important. 
    * We will see later if we have the time to dig into the particle system to create better animations.
8. The **TankHealth** script is responsible for holding the tank life, updating the UI elements and playing any animations or sounds. It uses a linear interpolation (lerp) to determine the correct health color to display. Its **TakeDamage** method will be re-used later on when we will be able to shoot the other tanks.
9. You can add the following code in the **TankHealth** to check if everything is setup properly:
```csharp
private void Update()
{
    TakeDamage(1f);
}
```
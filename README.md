# Introduction

The `2/camera` starts from the `1/tank-creation` branch code. The steps will guide you into creating the first draft of the camera which will have targets (e.g. the tanks) and will ensure that all targets are always showing in the camera by moving/zooming appropriately.

# Steps that have already been done

1. Create a new **empty Game Object** named **CameraRig** and set:
    * **Position** to (0, 0, 0)
    * **Rotation** to (40, 60, 0)
2. Drag the **Main Camera** object onto the **CameraRig** object to make it a child
3. Set the **Main Camera** object values:
    * **Position** to (0, 0, -65)
    * **Rotation** to (0, 0, 0)
5. Create a **Camera** folder in the **Scripts** folder
5. Create a **CameraControl** script in the **Scripts/Camera** folder
5. Add the **CameraControl** script as a component to the **CameraRig** object
6. Double the **CameraControl** script to open it in your IDE

The serialized and private fields
```csharp
[SerializeField] private float dampTime = 0.2f;
[SerializeField] private float screenEdgeBuffer = 4f;
[SerializeField] private float minSize = 6.5f;
[SerializeField] private Transform[] targets;

private UnityEngine.Camera _camera;
private float _zoomSpeed;
private Vector3 _moveVelocity;
```

The Unity events
```csharp
private void Awake()
{
    _camera = GetComponentInChildren<UnityEngine.Camera>();
}

private void FixedUpdate()
{
    var desiredPosition = CenterOnTargets();
    ZoomOnTargetsFromPosition(desiredPosition);
}
```

The specific code of the movement
```csharp
private Vector3 CenterOnTargets()
{
    var desiredPosition = FindTargetsAveragePosition();
    transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _moveVelocity, dampTime);
    return desiredPosition;
}

private Vector3 FindTargetsAveragePosition()
{
    var averagePos = targets.Aggregate(new Vector3(), (seed, target) => seed + target.position) / targets.Length;
    averagePos.y = transform.position.y;
    return averagePos;
}

private void ZoomOnTargetsFromPosition(Vector3 desiredPosition)
{
    var requiredSize = FindCameraRequiredSize(desiredPosition);
    _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, requiredSize, ref _zoomSpeed, dampTime);
}

private float FindCameraRequiredSize(Vector3 desiredPosition)
{
    var desiredLocalPos = transform.InverseTransformPoint(desiredPosition);
    var size = targets.Max(target => FindTargetDistanceAsSize(target, desiredLocalPos));
    size += screenEdgeBuffer;
    size = Mathf.Max(size, minSize);
    return size;
}

private float FindTargetDistanceAsSize(Transform target, Vector3 desiredLocalPos)
{
    if (!target || !target.gameObject || !target.gameObject.activeSelf)
        return 0f;

    var size = 0f;
    var targetLocalPos = transform.InverseTransformPoint(target.position);
    var desiredPosToTarget = targetLocalPos - desiredLocalPos;
    size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));
    size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / _camera.aspect);
    return size;
}

public void SetStartPositionAndSize()
{
    var desiredPosition = FindTargetsAveragePosition();
    transform.position = desiredPosition;
    _camera.orthographicSize = FindCameraRequiredSize(desiredPosition);
}
```

7. Click on the **CameraRig** object:
    * Drag the **Tank** object to **Targets** field of the **CameraControl** script

# Explanation

1. The **CameraRig** object act as a camera man which is holding the camera. The **CameraRig** will be moving the camera around to center on all the specified targets while the camera will be zooming in and out to show all targets in the plane.
2. The **CameraControl** contains all the mathematics required to correctly move and zoom the camera.
3. For the **Position**, we simply iterate through all the targets' position (e.g. all tanks) and average it to find the center of all the targets. This is easily done via Unity as the position are Vector3 and they provide operators overload to sum up all the positions (e.g. vectors) together and then divide them all by the number of targets to find the average.
    * The code is using LINQ to provide a *one liner* for the iteration instead of the classic *foreach loop*.

    If Tank1 is on the top left and Tank2 is on the bottom right, the desired position will be the average of these two positions which will be the center of the tanks.
---
    Tank1                  
                     
                Center        
                     
                            Tank2
---

4. For the **Size** (e.g. zoom), it's a bit more complicated. As the camera has been set to **Orthographic**, the zoom will be controlled by the size value. We need to find the vertical and horizontal distance from the **Center** position previously found to each target and take the highest value between the two sizes of all targets.

---
    .             Tank2   
                    |  (center to top of screen)
                    |  Size = Distance in Y axis    
                    | 
    Tank1 ------- Center   
    Size = Distance in X axis
    (center to edge of screen)      


    .             
---

5. For the vertical size, we can directly take the distance in the Y axis which is the **target.position.y - center.position.y**.

6. For the horizontal size, the distance will be equal to size multiply by the camera aspect (e.g. **distance = size * aspect**) since the camera is **Orthographic**. As we are interested in the size, the formula would be **size = distance / aspect**. The distance would be **target.position.x - center.position.x** and the aspect would be the **camera.aspect**.

7. Between the two sizes, we use the highest value found and repeat the above logic for all targets in order to find which target is the farthest so we can zoom out to ensure that all targets are in the camera.

8. To prevent some edge case scenarios, we add a screen buffer to act as a border around the camera to ensure the targets are not too close to the edge of the screen and we also check for a minimal value for the size to ensure the zoom doesn't get all zoomed in if all targets are nearby.
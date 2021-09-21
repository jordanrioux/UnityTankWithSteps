# Introduction

The `master` branch only contains the basic project template including all the required resources (AudioClips, Fonts, Materials, Models and Sprites) for the project and default settings for the scene.

A simple terrain and tank have also been added to the main scene but no components (Rigidbody, Collider, etc.) have been added to them yet.

The Assets and everything are coming from the Unity Unity tank tutorial. 

# Steps that have already been done

1. Create a new empty scene named **Main** in the **Scenes** folder
2. Delete the default **SampleScene** scene
3. Change the **Main Camera** object:
    * Set the **Transform** component values:
        * **Position** to (-43, 42, -25)
        * **Rotation** to (40, 60, 0)
    * Set the **Camera** component values:
        * **Projection** to **Orthographic**
        * **Size** to **8**
        * **Clear Flags** to **Solid Color** (from **Skybox**)
        * **Background** color to (80, 60, 50)

4. (Optional) Change the **Directional Light** object:
    * Set the **Light** component values:
        * **Color** to (244, 226, 172)
        * **Mode** to **Realtime**
        * **Strength** to **0.85** (from **1**) under Realtime Shadows

# Explanation

1. A new scene has been created instead of starting from the default SampleScene scene.
2. We are adjusting the camera to have a default view of the terrain and changing its projection type which will be used later on for adjusting the camera using mathematics based on the desired behavior (zooming, etc.).
3. We are adjusting the default light to match the color theme of the *Desert*. It's possible to complete customize the Lighting system by going in Window > Rendering > Lighting.
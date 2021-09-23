# Introduction

The `7/game-manager` starts from the `6/audio-mixing` branch code. The steps will guide you into creating the Game Manager and the Tank Manager logic to handle the rounds and the winning condition of the game.
# Steps that have already been done

1. Create two **empty game object** and name them **SpawnPoint1** and **SpawnPoint2**
2. Set the position of the **Spawn Points** as you wish:
    * **SpawnPoint1**:
        * **Position** to (-3, 0, 30)
        * **Rotation** to (0, 180, 0)
    * **SpawnPoint1**:
        * **Position** to (13, 0, -5)
        * **Rotation** to (0, 0, 0)
3. Create the **AudioMixers** folder in the **Assets** folder 
4. Create a new **Canvas** (UI > Canvas) and name it **MessageCanvas**
5. On the **Scene** view, click on the **2D** button to enable the 2D mode
6. Select the **MessageCanvas** and **press F** while the mouse in on the **Scene** view to frame it
7. Right-click on the **MessageCanvas** and create a **Text** (UI > Text) as a child of the **MessageCanvas**
8. Select the **Text** and set:
    * On **Rect Transform**:
        * **Anchors** for (X, Y)
            * **Min** of 0.1
            * **Max** of 0.9
        * **Left, Right, Top, Bottom and Pos Z** to 0
    * On **Text** component:
        * **Text** to **TANKS!**
        * **Font** to **BowlbyOne-Regular**
        * **Alignment** to **center** and **middle**
        * Check **Best Fit**
        * **Max Size** to 60
        * **Color** to White (255, 255, 255, 255)
9. Add a **Shadow** component to the **Text** object and set:
    * **Effect Color** to Brown (114, 71, 40, 128)
    * **Effect Distance** to (-3, -3)
10. You can disable the 2D mode on the **Scene** view
11. Select the **CameraRig** and modify the **CameraControl** script fields:
    * Set **Targets** size to 0 (e.g. remove all targets)
12. Create a **GameManagerScript** script in the **Scripts/Managers** folder
13. Add the **GameManagerScript** script as component to the **GameManager** object

14. TO BE COMPLETED...
# Explanation

TO BE COMPLETED
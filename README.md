# Introduction

The `6/audio-mixing` starts from the `5/tank-shooting` branch code. The steps will guide you into creating an audio mixer in order to control all the sounds in the game correctly.
# Steps that have already been done

1. Create an **empty game object** and name it **GameManager**
2. Add an **Audio Source** component to the **GameManager** and set:
    * **Audio Clip** to **BackgroundMusic**
    * Check **Loop**
3. Create the **AudioMixers** folder in the **Assets** folder 
4. Create an **Audio Mixer** (Create > Audio Mixer) in the **AudioMixers** folder and name it **MainMix**
5. Double-click on the **MainMix** object to open the Audio Mixer window
6. Click on the **+ icon** next to the **Groups** section to add a child to the **Master** group
7. Name the group **Music**
8. Reselect the **Master** group and another group named **SFX**
9. Reselect the **Master** group and another group named **Driving**
10. Make sure they are all children of the **Master** group
11. Select the **Tank** prefab and set:
    * For the **First Audio Source**, set the **Output** to **Driving** group of MainMix
    * For the **Second Audio Source**, set the **Output** to **SFX** group of MainMix
12. Open the **Shell** prefab and select the **ShellExplosion** child object and set:
    * For the **Audio Source**, set the **Output** to **SFX** group of MainMix
14. Select the **GameManager** prefab and set:
    * For the **Audio Source**, set the **Output** to **Music** group of MainMix
15. Return to the **Audio Mixer window**   
16. Set the following group values:
    * **Music** group:
        * **Attenuation** to -12
    * **Driving** group:
        * **Attenuation** to -25
17. Select the **Music** group and click on **Add...** at the bottom to add a **Duck Volume**
18. Add 
19. Select the **SFX** group and click on **Add...** to add a **Send** effect
20. For the **Send** effect set:
    * **Receive** to **Music\Duck Volume**
    * **Send Level** to 0db
21. Reselect the **Music** group, for the **Duck Volume** set
    * **Threshold** to -46
    * **Ratio** to 250
    * **Attack Time** to 0
22. Make sure to save all the prefabs


# Explanation

1. An **Audio Mixer** is created to control all the different sound of the game. 
2. We will creating different groups for the kind of music the game is having. The **Music** group is for the background music, the **SFX** group is for all the sound effects and the **Driving** group is for the tank driving sounds.
3. Once the groups are created, we go through all the different audio sources of the game and we assign the group to each of them (e.g. Tank Audio for Driving, Tank Audio for SFX, Explosions for SFX, Background music to Music, etc.).
4. We adjust the volume attenuation of the groups to have a lower volume by default.
5. We add a **Duck Volume** which will allow us to lower the attenuation of an audio group based on the attenuation of another group. In this case, the **Duck Volume** is on the **Music** group and will be link to the **SFX** group. This will allow us to lower the background music when we are playing a sound effect.
6. We add a **Send** effect to the **SFX** group to link it with the previously created **Duck Volume**.
7. We can now set all the **Duck Volume** settings as desired such as the threshold for when we lower the volume, the ratio, the delay, etc.
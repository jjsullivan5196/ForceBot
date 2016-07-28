##To use testing workbench

1. Set GearVRService to developer mode:
	*Go to Settings->Device->Applications->Application manager->Gear VR Service->Storage->Manage storage
	*Tap VR Service Version repeatedly until you see developer options
	*Switch developer mode to on, switch developer icon as well so you can access these settings from the app drawer
2. Sideload ForceBot.apk
3. Launch ForceBot
4. Leave the game, on your phone open a browser and go to:
	* http://localhost:8080/remote/
	* **VISIT THIS EXACT URL OR IT WILL NOT WORK!**
5. You should see an HTML form with a few fields:
	* Magnitude Gate: The strength that at least one direction of acceleration must achieve before velocity can be updated
	* Scale Factor: Multiplied with input acceleration values to decrease or increase influence
	* Lock on axes: Set any combination of x,y,z to lock movement on those respective axes
	* Zero acceleration: Any input that falls below the gate threshold will be ignored
6. Set these options as desired.
7. Press the update button to change settings and go back to ForceBot, the debug display on the HMD should update shortly afterward.
8. Insert phone into headset, you can now use the application. 
	* Start from step 4 if you ever want to change the settings again.
	* If you wish to reset your position at any time, tap the touchpad or screen.


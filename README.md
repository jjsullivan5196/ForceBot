##To use testing workbench

1. Sideload ForceBot.apk
2. Connect your phone to a known network (Or setup wifi hotspot)
3. Make note of your phone's current IP address
4. Launch ForceBot
5. On another internet enabled device on the same network, open a browser and go to:
	* http://\<device-ip\>:8080/remote/
	* Replace <device-ip> with your phone's IP address.
	* *VISIT THIS EXACT URL OR IT WILL NOT WORK!*
6. You should see an HTML form with a few fields:
	* Magnitude Gate: The strength that at least one direction of acceleration must achieve before velocity can be updated
	* Scale Factor: Multiplied with input acceleration values to decrease or increase influence
	* Lock on axes: Set any combination of x,y,z to lock movement on those respective axes
	* Zero acceleration: Any input that falls below the gate threshold will be ignored
7. Set these options as desired
8. Press the update button to change settings, the debug display on the HMD should update shortly afterward.
	* If you wish to reset your position at any time, tap the touchpad.


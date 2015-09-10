Updated demo for exploration of turbine data

HOW TO BUILD:

1. Import into Unity

2. Run in editor by double clicking the desired scene (either MOF1-Scene" & "Turbine-Scene")) in the 
   Project Hierarchy panel under Assets -> Scenes.  Then click the triangle play icon in the top 
   center of the editor window. 

3. Build either or both of 2 Scenes: "MOF1-Scene" & "Turbine-Scene").  To build, click File -> 
   Build Settings.  Select scenes to build and appropriate settings.  Then click "Build."  The 
   resulting file (.app format for mac or .exe for Windows) can then be run by double clicking,
   but display settings may need to be adjusted so that the application renders to the Oculus.


4. Oculus SDK and Unity Integration have been updated several times recently.  This demo was built with the following versions:
	- Oculus SDK 0.6.0.1
	- Native Unity support for virtual reality
	- Unity 5.1.2f1 Personal 

============================================

USAGE:

The project should be opened in Unity, and
may be run from within Unity or built to run as a standalone application.

Interactions supported:

1. Standard Oculus interactions (head movements, including turning and moving towards or away from the head tracker)

2. Move forward, backward, left, or right using cross platform inputs "Horizontal" and "Vertical" 
  (by default, arrow keys on keyboard or left joystick on Xbox controller).  
  The notion of "forward" is determined by the gaze direction of the Oculus.

3. Enter targeting mode and pull up targeting crosshairs with cross platform input "Fire 1"  
  (by default, left ctrl on keyboard or button "A" on Xbox controller).

4. Exit targeting mode and release targeting crosshairs with cross platform input "Fire 3"
  (by default, left cmd on keyboard of button "X" on Xbox controller).

5. In targeting mode, bookmark a location and data object by targeting the data object with the crosshairs and:
	A) using cross platform input "Fire 2" (by default, left alt on keyboard or button "B" on Xbox controller) OR
	B) holding your gaze for 2 seconds by keeping the crosshairs focused on the data object

6. In targeting mode, bookmark a location only (if no data object is the focus of the crosshairs) using cross
  platform input "Fire 2" (by default, left alt on keyboard or button "B" on Xbox controller).  Both position and
  orientation of your avatar's current location will be saved.
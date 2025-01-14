Start Scene File: MainMenu

How to play:
Movement: wasd
Nitro boost: left shift

Where to observe technology requirements:
3D game feel: Start menu, goal to defeat all enemies, victory screen, defeat screen.
Precursors to fun gameplay: Damage not taken if colliding with front of car, balance of health with collisions.
Character/vehicle control: Vehicle slides requiring more precise control, nitro boost option (speed vs. control), smooth camera.
3D world: Walls and clear screens, spikes (temporarily disabled but visible on end screen).
Real-time NPC AI: NPC has a patrol state and chase state that are determined by vehicles in a range of the NPC.

Problem Areas:
Collisions sometimes launch the player very far upwards. May actually keep this and incorporate a feature that lets you hover and dive down. Enemies sometimes ricochet off the walls.


List Code contributions for:

iv. Manifest of which files authored by each teammate:
	1. Detail who on the team did what
	2. For each team member, list each asset implemented.
	3. Make sure to list C# script files individually so we can confirm
each team member contributed to code writing.


James
1. Floating Health Bars, Enemy AI, End game when all enemies die
2. Floating Health Bars, Enemy Cars, Player (worked on), MainScene, VictoryScene
3. FloatingHealthBar, MinionAI, CarController (worked on)


Andrew
1. PlayerCarController
2. WheelAllignment. 
3. Traps, SpikeTrapController (Map Items)
4. CarCameraControler
5. Arena Map scene


June
1. Created basic in-game Player status UI; Created Enemy/Player Vehicle status; Did a part of collision damage;Opening Scene; Collectibles; Sound effects
2.In-game Status UI
3.Basic_Status_UI.cs
  Collision_Damage.cs
  Enemy_Vehicle.cs
  Vehicle_Part.cs
  Vehicle_Status.cs
  EntryScript.cs


Ning
1. Start Menu / In-game Menu
2. Collision damage 
3. Menu.cs
   MainMenu.cs
   AttackPart.cs
   DamagePart.cs
   IVehicle.cs
   MeshMerger.cs
   VehiclePart.cs
   CarController.cs (worked on)


Aditya
1. Worked on In-Game Menu, Loss Screen, Enemy AI, Car Collider, Camera.
2. End Screen, Enemy Cars, Player Car tuning, Text Materials/Styles.
3. 	GameQuitter.cs
	GameRestarter.cs
	PlayerHealth.cs
	CarAI.cs
	PlayerCarController.cs
	MinionAI.cs
	VelocityReporter.cs

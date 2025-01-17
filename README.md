# Amulet of Many Minions Cross Mod Sample

## Overview
This repository contains a small example mod that utilizes Amulet of Many Minions' (AoMM) cross-mod
API. This API allows minions and pets added by other mods to utilize AoMM's pathfinding tools,
select enemies to attack based on AoMM's target selection tactics, and, in the case of pets, automatically
scale in damage and travel speed based on the player's combat pet level.

## Mod Structure

The sample mod's files are located in the following sub-directories. Each file demonstrates
a different portion of the cross mod API:

* `AmuletOfManyMinionsApi.cs`: Contains convenience wrappers for every `mod.Call` option in AoMM, as well
  as documentation for those calls. Documentation is also provided below in the README.

* `AoMMState.cs`: Contains an Interface that matches the value names and types returned by AoMM's state-getter call 
  (`mod.Call("GetStateDirect", projectile, output)`), as well as documentation of those fields, and a basic implementation of that interface. 
  Documentation for the fields in the interface is also provided below in the README.

* `AoMMCrossModSample.cs`: Contains all of the `mod.Call`s used to register the sample minions and pets for cross-mod AI.

* `Pets/SampleGroundedPet/`: Contains the boilerplate code for a pet that behaves like the vanilla Turtle pet. No cross-mod specific
  code exists in this directory. The cross-mod combat pet behavior is managed entirely by `mod.Call("RegisterGroundedPet", ...)`.

* `Pets/SampleFlyingPet/`: Contains the boilerplate code for a pet that behaves like the vanilla Zephyr Fish pet. No cross-mod specific
  code exists in this directory. The cross-mod combat pet behavior is managed entirely by the `mod.Call("RegisterFlyingPet", ...)`.

* `Pets/SampleCustomPet/`: Contains the code for a non-pet projectile that is turned into a combat pet via 
  `mod.Call("RegisterFlyingPet", ...)`. Also includes a basic example of using `mod.Call("GetState",...)` to act on
  AoMM state variables retrieved as a dictionary.

* `Pets/SampleOptionalCombatPetProjectile/`: Contains the code for a pet projectile with two pet buffs, one of which is registered with 
  `mod.Call(...)` and one of which isn't. The pet will behave as a regular pet when summoned via the non-registered buff, and as a combat 
	pet when summoned via the registered buff. This can be used to achieve an effect similar to the " (AoMM Version)" of pet items in the 
	base mod.

* `Pets/SampleMeleeRangedPet/`: Contains the code for a pet projectile that alternates between melee and ranged behavior depending
  on the player's combat pet level. Uses `mod.Call("GetStateDirect", ...)` to determine the player's combat pet level each frame, then
	`mod.Call("GetParamsDirect", ...)` and `mod.Call("UpdateParamsDirect", ...)` to toggle between melee and ranged behavior.

* `Minions/SampleGroundedMinion/`: Contains the boilerplate code for a minion that behaves like the vanilla vampire frog pet. No cross-mod 
  specific code exists in this directory. The cross-mod minion behavior is managed entirely by `mod.Call("RegisterGroundedMinion", ...)`.

* `Minions/SamplePathfindingMinion/`: Contains the code for a minion that implements custom behavior based on AoMM's state variables,
  while using AoMM's default pathfinder-following code. Also includes an example of using `mod.Call("GetStateDirect", ...)` to access
  AoMM's state variables directly as an object.

* `Minions/SampleCustomMinion/`: Contains the code for a minion that implements custom behavior based on AoMM's state variables,
  while implementing custom code for following the pathfinder. Also includes an example of using `mod.Call("GetStateDirect", ...)` 
  to access AoMM's state variables directly as an object.

* `Minions/SampleEmpoweredMinion/`: Contains the code for a minion that behaves like Abigail or the Desert tiger, summoning a single minion
  that gets more powerful with each minion slot used. Uses `mod.Call("GetParamsDirect", ...)` and `mod.Call("UpdateParamsDirect", ...)` to 
	dynamically update the minion's travel speed, search range, and attack rate based on the number of copies summoned.

* `Projectiles/`: Contains clones of a few vanilla projectiles, with their `SetDefaults` updated to properly set the flags for a
	minion-shot projectile. These projectiles are assigned as minion attacks in several `mod.Calls` (see below).

## Mod.Call Documentation

AoMM provides the following mod.Calls:

### Accessing AoMM-Computed State:
* `mod.Call("GetState", ModProjectile proj) -> Dictionary<string, object>`  
  Get the entire <key, object> mapping of the projectile's cross-mod exposed state.
  * `proj`: The active instance of the projectile whose state should be retrieved

* `mod.Call("GetStateDirect", ModProjectile proj, object destination) -> void`  
	Fill the projectile's cross-mod exposed state directly into a destination object.
	The destination object should either explicitly or implicitly implement IAoMMState (see AoMMState.cs).
  * `proj`: The active instance of the projectile whose state should be retrieved
  * `destination`: An object that implements IAoMMState. Its fields will be overridden with the projectile's AoMM managed state via reflection.

### Modifying AoMM-Controlled Behavior Dynamically:

* `mod.Call("GetParams", ModProjectile proj) -> Dictionary<string, object>`  
  Get the entire <key, object> mapping of the parameters used to control the minion's cross-mod behavior.
  * `proj`: The active instance of the projectile whose behavior parameters should be retrieved

* `mod.Call("GetParamsDirect", ModProjectile proj, object destination) -> void`  
	Fill the projectile's cross-mod behavior parameters directly into a destination object.
	The destination object should either explicitly or implicitly implement IAoMMParams (see AoMMParams.cs).
  * `proj`: The active instance of the projectile whose state should be retrieved
  * `destination`: An object that implements IAoMMParams. Its fields will be overridden with the projectile's AoMM behavior parameters via reflection.

* `mod.Call("UpdateParams", ModProjectile proj, Dictionary<string, object> source) -> void`  
  Update the projectile's cross-mod behavior parameters using a <key, object> mapping of new parameters values.
  * `proj`: The active instance of the projectile whose behavior parameters should be retrieved
  * `source`: a <string, object> dictionary containing new values for the minion's cross mod behavior parameters.

* `mod.Call("UpdateParamsDirect", ModProjectile proj, object source) -> void`  
  Update the projectile's cross-mod behavior parameters directly based off the properties of a source object.
	Fill the projectile's cross-mod behavior parameters directly into a destination object.
	The destination object should either explicitly or implicitly implement IAoMMParams (see AoMMParams.cs).
  * `proj`: The active instance of the projectile whose state should be retrieved
  * `source`: An object that implements IAoMMParams. The projectile's AoMM behavior parameters will be overwritten with this object's fields via reflection.

* `mod.Call("ReleaseControl", ModProjectile proj) -> void`  
	For the following frame, do not apply AoMM's pre-calculated position and velocity changes 
	to the projectile in PostAI(). Used to temporarily override behavior in fully managed minion AIs.
  * `proj`: The active instance of the projectile whose state should be retrieved

### Registering Minions for cross-mod AI:

* `mod.Call("RegisterInfoMinion", ModProjectile proj, ModBuff buff, int searchRange) -> void`  
	Register a read-only cross mod minion. AoMM will run its state calculations for this minion every frame,
	but will not perform any actions based on those state calculations. The ModProjectile may read AoMM's 
	calculated state using `mod.Call("GetState",this)`, and act on that state in any way.
  * `proj`: The singleton content instance of the ModProjectile (as retrieved via `ModContent.GetInstance`)
  * `buff`: The singleton content instance of the ModBuff (as retrieved via `ModContent.GetInstance`) that's applied when the minion is summoned. 
  * `searchRange`: The range (in pixels) over which the tactic enemy selection should search.

* `mod.Call("RegisterInfoPet", ModProjectile proj, ModBuff buff) -> void`  
	Register a read-only cross mod combat pet. AoMM will run its state calculations for this combat pet every frame,
	but will not perform any actions based on those state calculations. The ModProjectile may read AoMM's 
	calculated state using `mod.Call("GetState",this)`, and act on that state in any way.
  * `proj`: The singleton content instance of the ModProjectile (as retrieved via `ModContent.GetInstance`)
  * `buff`: The singleton content instance of the ModBuff (as retrieved via `ModContent.GetInstance`) that's applied when the minion is summoned. 

* `mod.Call("RegisterPathfindingMinion", ModProjectile proj, ModBuff buff, int searchRange, int travelSpeed, int inertia) -> void`  
	Register a basic cross mod minion. AoMM will run its state calculations for this minion every frame,
	and take over its position and velocity while the pathfinder is present.
	* `proj`: The singleton instance of the ModProjectile for this minion type
	* `buff`: The singleton instance of the ModBuff associated with the minion
	* `searchRange`: 
		The range (in pixels) over which the tactic enemy selection should search. See behavior parameters below for more details.
	* `travelSpeed`: 
		The speed at which the minion should travel. See behavior parameters below for more details.
	* `inertia`: 
		How quickly the minion should change directions. See behavior parameters below for more details.

* `mod.Call("RegisterPathfindingPet", ModProjectile proj, ModBuff buff) -> void`  
	Register a basic cross mod combat pet. AoMM will run its state calculations for this minion every frame,
	and take over its position and velocity while the pathfinding node is present.
	The pet's movement speed and search range will automatically scale with the player's combat
	pet level.
	* `proj`: The singleton instance of the ModProjectile for this minion type
	* `buff`: The singleton instance of the ModBuff associated with the minion

* `mod.Call("RegisterFlyingPet", ModProjectile proj, ModBuff buff, int? projType) -> void`  
	Register a fully managed flying cross mod combat pet. AoMM will take over this projectile's 
	AI every frame, and will cause it to behave like a basic flying minion (eg. the Raven staff).
	The pet's damage, movement speed, and search range will automatically scale with the player's combat
	pet level.
	* `proj`: The singleton instance of the ModProjectile for this minion type
	* `buff`: The singleton instance of the ModBuff associated with the minion
	* `projType`: Which projectile the minion should shoot. If null, the minion will do a melee attack

* `mod.Call("RegisterFlyingMinion", ModProjectile proj, ModBuff buff, int? projType, int searchRange, int travelSpeed, int inertia) -> void`  
	Register a fully managed flying cross mod minion. AoMM will take over this projectile's 
	AI every frame, and will cause it to behave like a basic flying minion (eg. the Raven staff).
	* `proj`: The singleton instance of the ModProjectile for this minion type
	* `buff`: The singleton instance of the ModBuff associated with the minion
	* `projType`: Which projectile the minion should shoot. If null, the minion will do a melee attack
	* `searchRange`: 
		The range (in pixels) over which the tactic enemy selection should search. See behavior parameters below for more details.
	* `travelSpeed`: 
		The speed at which the minion should travel. See behavior parameters below for more details.
	* `inertia`: 
		How quickly the minion should change directions. See behavior parameters below for more details.
	* `attackFrames`: 
		How frequently the minion should fire a projectile. See behavior parameters below for more details.

* `mod.Call("RegisterGroundedPet", ModProjectile proj, ModBuff buff, int? projType) -> void`  
	Register a fully managed grounded cross mod combat pet. AoMM will take over this projectile's 
	AI every frame, and will cause it to behave like a basic flying minion (eg. the Flinx staff).
	The pet's damage, movement speed, and search range will automatically scale with the player's combat
	pet level.
	* `proj`: The singleton instance of the ModProjectile for this minion type
	* `buff`: The singleton instance of the ModBuff associated with the minion
	* `projType`: Which projectile the minion should shoot. If null, the minion will do a melee attack

* `mod.Call("RegisterGroundedMinion", ModProjectile proj, ModBuff buff, int? projType, int searchRange, int travelSpeed, int inertia) -> void`  
	Register a fully managed grounded cross mod minion. AoMM will take over this projectile's 
	AI every frame, and will cause it to behave like a basic flying minion (eg. the Flinx staff).
	* `proj`: The singleton instance of the ModProjectile for this minion type
	* `buff`: The singleton instance of the ModBuff associated with the minion
	* `projType`: Which projectile the minion should shoot. If null, the minion will do a melee attack
	* `searchRange`: 
		The range (in pixels) over which the tactic enemy selection should search. See behavior parameters below for more details.
	* `travelSpeed`: 
		The speed at which the minion should travel. See behavior parameters below for more details.
	* `inertia`: 
		How quickly the minion should change directions. See behavior parameters below for more details.
	* `attackFrames`: 
		How frequently the minion should fire a projectile. See behavior parameters below for more details.

* `mod.Call("RegisterSlimePet", ModProjectile proj, ModBuff buff, bool idleBounce) -> void`  
	Register a fully managed slime-style cross mod combat pet. AoMM will take over this projectile's 
	AI every frame, and will cause it to behave like a slime pet (eg. the Slime Prince).
	The pet's damage, movement speed, and search range will automatically scale with the player's combat
	pet level. Note that slime pets currently only support a melee attack, and cannot fire a projectile.
	* `proj`: The singleton instance of the ModProjectile for this minion type
	* `buff`: The singleton instance of the ModBuff associated with the minion
	* `idleBounce`: Whether the minion should remain stationary while not moving, or always bounce back and forth.

## Amulet of Many Minions cross-mod Behavior Parameters Documentation

While a minion is registered to AoMM via `mod.Call("RegisterX",...)`, Several integer values are 
passed in to determine the specifics of how the minion behaves when controlled via the cross-mod AI.
For simple minions with a single behavior pattern, setting these values once in the registration `mod.Call` 
will usually be sufficient. However, for more advanced behaviors, it may be beneficial to retrieve and update
these parameters dynamically.
These values can be retrieved and updated as a dictionary via `mod.Call("GetParams", projectile)` and 
`mod.Call("UpdateParams", projectile, dict)`, or written to and read from an object directly via 
`mod.Call("GetParamsDirect", projectile, destination)` and `mod.Call("UpdateParamsDirect", projectile, source)`.
The parameter values, their types, and their effects on the minion's behavior are documented below. Note that
most of these parameters can only be updated for minions, since the values are updated automatically for
combat pets based on the player's pet level.


* `int SearchRange`: The range (in pixels) over which the tactic enemy selection should search. 
  Updated automatically for pets, set manually for minions. For minions, a
  reasonable set of values for this parameter are as follows:
  * 600 for early pre-hardmode
  * 900 for early hardmode
  * 1200 for late hardmode  

* `int MaxSpeed`: Max travel speed for the minion. Updated automatically for pets, set manually for minions.
  For minions, a reasonable set of values for this parameter are as follows:
  * 8 for early pre-hardmode
  * 12 for early hardmode
  * 16 for late hardmode  

* `int Inertia`: How quickly the minion should change directions while moving. Higher values lead to
  slower turning. Updated automatically for pets, set manually for minions.
  For minions, a reasonable set of values for this parameter are as follows:
  * 16 for early pre-hardmode
  * 12 for early hardmode
  * 8 for late hardmode  

* `int? FiredProjectileId`: Which projectile the minion should fire. set manually for both minions and pets. If null,
  the minion/pet will perform a melee attack instead.

* `int AttackFrames`: The rate at which the minion should fire its projectile, if it fires a projectile. Updated 
  automatically for pets, set manually for minions. Reasonable values for this parameter vary depending on the
	damage of the minion, but values between 15 (4 attacks per second) for a fast, low damage minion, and 60 
	(1 attack per second) for a slow, high damage minion may be reasonable.

## Amulet of Many Minions cross-mod State Documentation

While a minion is registered to AoMM via `mod.Call("RegisterX",...)`, AoMM will perform a variety of state
calculations each frame in that ModProjectile's `PreAI` hook. These values can be retrieved as a dictionary
via `mod.Call("GetState", projectile)`, or copied directly into an object that contains any subset of the
state properties via `mod.Call("GetStateDirect", projectile, destination)`. The state values that are calculated,
and their types, are documented below:

* `bool IsAttacking`: Whether AoMM has determined that an enemy is available for the minion to attack. True
when a nonzero number of enemy NPCs are found within range of the minion for its current tactic, and the minion is
not in the middle of following the pathfinder.

* `bool IsIdle`: Whether AoMM has determined the minion should idle by the player's head. True when no enemies are detected, 
and the pathfinder is not present.

* `bool IsPathfinding`: Whether AoMM has determined that the minion should be following the pathfinder. True while the pathfinder
is present, and the minion has either not completed the pathfinding route, or has completed the route but has no enemies available to attack.

* `bool IsPet`: Whether the minion is being treated as a combat pet.

* `Vector2? NextPathfindingTarget`: The position of the next bend in the pathfinding path, based on the minion's current position.
`null` if the pathfinder is not present.

* `Vector2? PathfindingDestination`: The position of the end of the pathfinding path. `null` if the pathfinder is not present.

* `int PetDamage`: The suggested originalDamage value for a combat pet based on the player's current combat pet level.

* `int PetLevel`: The current combat pet level of the player the projectile belongs to.

* `List<NPC> PossibleTargetNPCs`: All possible NPC targets, ordered by proximity to the most relevant target.

* `NPC TargetNPC`: The NPC selected as most relevant based on the minion's current tactic and search range.
`null` if none are available.

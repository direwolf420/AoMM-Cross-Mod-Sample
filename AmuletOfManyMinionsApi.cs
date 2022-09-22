﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;

namespace AoMMCrossModSample
{
    // Copy this file for your mod, change the namespace above to yours, and read the comments
    /// <summary>
    /// Collection of utility methods that wrap the mod.Calls available from AoMM.
    /// </summary>
    public class AmuletOfManyMinionsApi : ModSystem
    {
        //GENERAL INFO - PLEASE READ THIS FIRST!
        //-----------------------
        //https://github.com/westphallm1/AoMM-Cross-Mod-Sample#readme
        //
        //This file is kept up-to-date to the latest AoMM release. You are encouraged to not edit this file, and when an update happens, copy&replace this file again.
        //Nothing will happen if AoMM updates and your mod doesn't, it's your choice to update it further
        //-----------------------

        //This is the version of the calls that are used for the mod.
        //If AoMM updates, it will keep working on the outdated calls, but new features might not be available
        internal static readonly Version apiVersion = new Version(0, 16, 0, 4);

        internal static string versionString;

        private static Mod aommMod;

        internal static Mod AommMod
        {
            get
            {
                if (aommMod == null && ModLoader.TryGetMod("AmuletOfManyMinions", out var mod))
                {
                    aommMod = mod;
                }
                return aommMod;
            }
        }

        public override void Load()
        {
            versionString = apiVersion.ToString();
        }

        public override void Unload()
        {
            aommMod = null;
            versionString = null;
        }

        #region Calls
        /// <summary>
        /// Get the entire <key, object> mapping of the projectile's cross-mod exposed state, if it has one.
        /// See IAoMMState interface below for the names and types of the exposed state variables.
        /// </summary>
        /// <param name="proj">The ModProjectile to access the state for</param>
        internal static Dictionary<string, object> GetState(ModProjectile proj)
        {
            return AommMod?.Call("GetState", versionString, proj) as Dictionary<string, object>;
        }

        /// <summary>
        /// Attempt to fill the projectile's cross-mod exposed state directly into a destination object.
        /// The returned object will contain all AoMM state variables automatically cast to the correct type 
        /// (see IAoMMState interface below).
        /// </summary>
        /// <param name="proj">The ModProjectile to access the state for.</param>
        /// <param name="destination">The object to populate the projectile's cross mod state into.</param>
        /// <returns>True if AoMM is enabled and the projectile has an AoMM state attached, false otherwise.</returns>
        internal static bool TryGetStateDirect(ModProjectile proj, out IAoMMState destination)
        {
            destination = new AoMMStateImpl();
            AommMod?.Call("GetStateDirect", versionString, proj, destination);
            return destination != null;
        }

        /// <summary>
        /// Quick, non-reflective getter for the cross-mod IsActive flag. See the CrossModParams interface for more details.
        /// </summary>
        /// <param name="proj">The ModProjectile to access the state for</param>
        internal static bool IsActive(ModProjectile proj)
        {
            return (bool)(AommMod?.Call("IsActive", versionString, proj) ?? false);
        }

        /// <summary>
        /// Quick, non-reflective getter for the cross-mod IsIdle flag. See the CrossModState interface for more details.
        /// </summary>
        /// <param name="proj">The ModProjectile to access the state for</param>
        internal static bool IsIdle(ModProjectile proj)
        {
            return (bool)(AommMod?.Call("IsIdle", versionString, proj) ?? false);
        }

        /// <summary>
        /// Quick, non-reflective getter for the cross-mod IsAttacking flag. See the CrossModState interface for more details.
        /// </summary>
        /// <param name="proj">The ModProjectile to access the state for</param>
        internal static bool IsAttacking(ModProjectile proj)
        {
            return (bool)(AommMod?.Call("IsAttacking", versionString, proj) ?? false);
        }

        /// <summary>
        /// Quick, non-reflective getter for the cross-mod IsPathfinding flag. See the CrossModState interface for more details.
        /// </summary>
        /// <param name="proj">The ModProjectile to access the state for</param>
        internal static bool IsPathfinding(ModProjectile proj)
        {
            return (bool)(AommMod?.Call("IsPathfinding", versionString, proj) ?? false);
        }

        /// <summary>
        /// Get the <key, object> mapping of the parameters used to control this projectile's
        /// cross-mod behavior. See IAoMMParams interface below for the names and types of these parameters.
        /// </summary>
        /// <param name="proj">The ModProjectile to access the behavior parameters for.</param>
        internal static Dictionary<string, object> GetParams(ModProjectile proj)
        {
            return AommMod?.Call("GetParams", versionString, proj) as Dictionary<string, object>;
        }

        /// <summary>
        /// Attempt to fill the projectile's cross-mod behavior parameters directly into a destination object.
        /// The returned object will contain all AoMM parameters automatically cast to the correct type 
        /// (see IAoMMParams interface below).
        /// </summary>
        /// <param name="proj">The ModProjectile to access the behavior parameters for.</param>
        /// <param name="destination">The object to populate the projectile's behavior parameters into.</param>
        /// <returns>True if AoMM is enabled and the projectile has AoMM params attached, false otherwise.</returns>
        internal static bool TryGetParamsDirect(ModProjectile proj, out IAoMMParams destination)
        {
            destination = new AoMMParamsImpl();
            AommMod?.Call("GetParamsDirect", versionString, proj, destination);
            return destination != null;
        }

        /// <summary>
        /// Update the parameters used to control this projectile's cross mod behaviior by passing
        /// in a <key, object> mapping of new parameter values. See IAoMMParams interface below for the names and 
        /// types of these parameters.
        /// </summary>
        /// <param name="proj">The ModProjectile to update the behavior parameters for.</param>
        /// <param name="update">A dictionary containing new behavior paramter values.</param>
        internal static void UpdateParams(ModProjectile proj, Dictionary<string, object> update)
        {
            AommMod?.Call("UpdateParams", versionString, proj, update);
        }

        /// <summary>
        /// Update the parameters used to control this projectile's cross mod behaviior by passing
        /// in an object that implements the correct paramter names and types. See IAoMMParams interface below for 
        /// the names and types of these parameters.
        /// </summary>
        /// <param name="proj">The ModProjectile to update the behavior parameters for.</param>
        /// <param name="update">An object containing new behavior paramter values.</param>
        internal static void UpdateParamsDirect(ModProjectile proj, IAoMMParams update)
        {
            AommMod?.Call("UpdateParamsDirect", versionString, proj, update);
        }

        /// <summary>
        /// For the following frame, do not apply AoMM's pre-calculated position and velocity changes 
        /// to the projectile in PostAI(). Used to temporarily override behavior in fully managed minion AIs
        /// </summary>
        /// <param name="proj">The ModProjectile to release for this frame</param>
        internal static void ReleaseControl(ModProjectile proj)
        {
            AommMod?.Call("ReleaseControl", versionString, proj);
        }

        /// <summary>
        /// Register a read-only cross mod minion. AoMM will run its state calculations for this minion every frame,
        /// but will not perform any actions based on those state calculations. The ModProjectile may read AoMM's 
        /// calculated state using mod.Call("GetState",this), and act on that state as it pleases.
        /// </summary>
        /// <param name="proj">The singleton instance of the ModProjectile for this minion type</param>
        /// <param name="buff">The singleton instance of the ModBuff associated with the minion</param>
        /// <param name="searchRange">The range (in pixels) over which the tactic enemy selection should search.</param>
        /// <returns></returns>
        internal static void RegisterInfoMinion(ModProjectile proj, ModBuff buff, int searchRange)
        {
            AommMod?.Call("RegisterInfoMinion", versionString, proj, buff, searchRange);
        }

        /// <summary>
        /// Register a read-only cross mod combat pet. AoMM will run its state calculations for this combat pet every frame,
        /// but will not perform any actions based on those state calculations. The ModProjectile may read AoMM's 
        /// calculated state using mod.Call("GetState",this), and act on that state as it pleases.
        /// </summary>
        /// <param name="proj">The singleton instance of the ModProjectile for this combat pet type</param>
        /// <param name="buff">The singleton instance of the ModBuff associated with the pet</param>
        /// <returns></returns>
        internal static void RegisterInfoPet(ModProjectile proj, ModBuff buff)
        {
            AommMod?.Call("RegisterInfoPet", versionString, proj, buff);
        }

        /// <summary>
        /// Register a basic cross mod minion. AoMM will run its state calculations for this minion every frame,
        /// and take over its position and velocity while the pathfinding node is present.
        /// </summary>
        /// <param name="proj">The singleton instance of the ModProjectile for this minion type</param>
        /// <param name="buff">The singleton instance of the ModBuff associated with the minion</param>
        /// <param name="searchRange">
        /// The range (in pixels) over which the tactic enemy selection should search. AoMM will release the 
        /// minion from the pathfinding AI as soon as an enemy is detected in range.
        /// Should be ~400 for early pre-HM, ~800 for early HM, ~1200 for late HM.
        /// </param>
        /// <param name="travelSpeed">
        /// The speed at which the minion should travel while following the pathfinder
        /// Should be ~8 for early pre-HM, ~12 for early HM, ~16 for late HM.
        /// </param>
        /// <param name="inertia">
        /// How quickly the minion should change directions while following the pathfinder. Higher values lead to
        /// slower turning.
        /// Should be ~16 for early pre-HM, ~12 for early HM, ~8 for late HM.
        /// </param>
        internal static void RegisterPathfindingMinion(ModProjectile proj, ModBuff buff, int searchRange, int travelSpeed, int inertia)
        {
            AommMod?.Call("RegisterPathfindingMinion", versionString, proj, buff, searchRange, travelSpeed, inertia);
        }


        /// <summary>
        /// Register a basic cross mod combat pet. AoMM will run its state calculations for this minion every frame,
        /// and take over its position and velocity while the pathfinding node is present.
        /// The pet's movement speed and search range will automatically scale with the player's combat
        /// pet level.
        /// </summary>
        /// <param name="proj">The singleton instance of the ModProjectile for this minion type</param>
        /// <param name="buff">The singleton instance of the ModBuff associated with the minion</param>
        /// </param>
        internal static void RegisterPathfindingPet(ModProjectile proj, ModBuff buff)
        {
            AommMod?.Call("RegisterPathfindingPet", versionString, proj, buff);
        }

        /// <summary>
        /// Register a fully managed flying cross mod combat pet. AoMM will take over this projectile's 
        /// AI every frame, and will cause it to behave like a basic flying minion (eg. the Raven staff).
        /// The pet's damage, movement speed, and search range will automatically scale with the player's combat
        /// pet level.
        /// </summary>
        /// <param name="proj">The singleton instance of the ModProjectile for this minion type</param>
        /// <param name="buff">The singleton instance of the ModBuff associated with the minion</param>
        /// <param name="projType">Which projectile the minion should shoot. If null, the minion will do a melee attack</param>
        /// <param name="defaultIdle">
        /// Whether to use default pet AI while idling by the player. Set to true to maintain unique pet behaviors 
        /// while not attacking enemies.
        /// </param>
        internal static void RegisterFlyingPet(ModProjectile proj, ModBuff buff, int? projType, bool defaultIdle = true)
        {
            AommMod?.Call("RegisterFlyingPet", versionString, proj, buff, projType, defaultIdle);
        }

        /// <summary>
        /// Register a fully managed flying cross mod minion. AoMM will take over this projectile's 
        /// AI every frame, and will cause it to behave like a basic flying minion (eg. the Raven staff).
        /// </summary>
        /// <param name="proj">The singleton instance of the ModProjectile for this minion type</param>
        /// <param name="buff">The singleton instance of the ModBuff associated with the minion</param>
        /// <param name="projType">Which projectile the minion should shoot. If null, the minion will do a melee attack</param>
        /// <param name="searchRange">
        /// The range (in pixels) over which the tactic enemy selection should search.
        /// Should be ~400 for early pre-HM, ~800 for early HM, ~1200 for late HM.
        /// </param>
        /// <param name="travelSpeed">
        /// The speed at which the minion should travel.
        /// Should be ~8 for early pre-HM, ~12 for early HM, ~16 for late HM.
        /// </param>
        /// <param name="inertia">
        /// How quickly the minion should change directions while moving. Higher values lead to
        /// slower turning.
        /// Should be ~16 for early pre-HM, ~12 for early HM, ~8 for late HM.
        /// </param>
        /// <param name="attackFrames">
        /// How frequently the minion should fire a projectile, if it fires a projectile.
        /// A good frequency depends on the amount of damage done, with somewhere around 45 frames
        /// for a high damage projectile and 15 frames for a low damage projectile.
        /// </param>
        /// <param name="defaultIdle">
        /// Whether to use default pet AI while idling by the player. Set to true to maintain unique minion behaviors 
        /// while not attacking enemies.
        /// </param>
        internal static void RegisterFlyingMinion(
            ModProjectile proj, ModBuff buff, int? projType, int searchRange, int travelSpeed, int inertia, int attackFrames = 30, bool defaultIdle = true)
        {
            AommMod?.Call("RegisterFlyingMinion", versionString, proj, buff, projType, searchRange, travelSpeed, inertia, attackFrames, defaultIdle);
        }

        /// <summary>
        /// Register a fully managed grounded cross mod combat pet. AoMM will take over this projectile's 
        /// AI every frame, and will cause it to behave like a basic grounded minion (eg. the Pirate staff).
        /// The pet's damage, movement speed, and search range will automatically scale with the player's combat
        /// pet level.
        /// </summary>
        /// <param name="proj">The singleton instance of the ModProjectile for this minion type</param>
        /// <param name="buff">The singleton instance of the ModBuff associated with the minion</param>
        /// <param name="projType">Which projectile the minion should shoot. If null, the minion will do a melee attack</param>
        /// <param name="defaultIdle">
        /// Whether to use default pet AI while idling by the player. Set to true to maintain unique pet behaviors 
        /// while not attacking enemies.
        /// </param>
        internal static void RegisterGroundedPet(ModProjectile proj, ModBuff buff, int? projType, bool defaultIdle = true)
        {
            AommMod?.Call("RegisterGroundedPet", versionString, proj, buff, projType, defaultIdle);
        }

        /// <summary>
	    /// Register a fully managed slime-style cross mod combat pet. AoMM will take over this projectile's 
	    /// AI every frame, and will cause it to behave like a slime pet (eg. the Slime Prince).
	    /// The pet's damage, movement speed, and search range will automatically scale with the player's combat
	    /// pet level. 
        /// </summary>
        /// <param name="proj">The singleton instance of the ModProjectile for this minion type</param>
        /// <param name="buff">The singleton instance of the ModBuff associated with the minion</param>
        /// <param name="projType">Which projectile the minion should shoot. If null, the minion will do a melee attack.</param>
        /// <param name="defaultIdle">
        /// Whether to use default pet AI while idling by the player. Set to true to maintain unique pet behaviors 
        /// while not attacking enemies.
        /// </param>
        internal static void RegisterSlimePet(ModProjectile proj, ModBuff buff, int? projType, bool defaultIdle = true)
        {
            AommMod?.Call("RegisterSlimePet", versionString, proj, buff, projType, defaultIdle);
        }

        /// <summary>
	    /// Register a fully managed worm-style cross mod combat pet. AoMM will take over this projectile's 
	    /// AI every frame, and will cause it to behave like a worm pet (eg. the Eater of Worms).
	    /// The pet's damage, movement speed, and search range will automatically scale with the player's combat
	    /// pet level. Note that the worm AI is intended for melee attacks, and will not move smoothly if
        /// set to fire a projectile.
        /// </summary>
        /// <param name="proj">The singleton instance of the ModProjectile for this minion type</param>
        /// <param name="buff">The singleton instance of the ModBuff associated with the minion</param>
        /// <param name="projType">Which projectile the minion should shoot. If null, the minion will do a melee attack.</param>
        /// <param name="defaultIdle">
        /// Whether to use default pet AI while idling by the player. Set to true to maintain unique pet behaviors 
        /// while not attacking enemies.
        /// </param>
        internal static void RegisterWormPet(ModProjectile proj, ModBuff buff, int? projType, bool defaultIdle = true)
        {
            AommMod?.Call("RegisterWormPet", versionString, proj, buff, projType, defaultIdle);
        }

        /// <summary>
        /// Register a fully managed grounded cross mod minion. AoMM will take over this projectile's 
        /// AI every frame, and will cause it to behave like a basic grounded minion (eg. the Pirate staff).
        /// </summary>
        /// <param name="proj">The singleton instance of the ModProjectile for this minion type</param>
        /// <param name="buff">The singleton instance of the ModBuff associated with the minion</param>
        /// <param name="projType">Which projectile the minion should shoot. If null, the minion will do a melee attack.</param>
        /// <param name="searchRange">
        /// The range (in pixels) over which the tactic enemy selection should search.
        /// Should be ~400 for early pre-HM, ~800 for early HM, ~1200 for late HM.
        /// </param>
        /// <param name="travelSpeed">
        /// The speed at which the minion should travel.
        /// Should be ~8 for early pre-HM, ~12 for early HM, ~16 for late HM.
        /// </param>
        /// <param name="inertia">
        /// How quickly the minion should change directions while moving. Higher values lead to
        /// slower turning.
        /// Should be ~16 for early pre-HM, ~12 for early HM, ~8 for late HM.
        /// </param>
        /// <param name="attackFrames">
        /// How frequently the minion should fire a projectile, if it fires a projectile.
        /// A good frequency depends on the amount of damage done, with somewhere around 45 frames
        /// for a high damage projectile and 15 frames for a low damage projectile.
        /// </param>
        /// <param name="defaultIdle">
        /// Whether to use default pet AI while idling by the player. Set to true to maintain unique minion behaviors 
        /// while not attacking enemies.
        /// </param>
        internal static void RegisterGroundedMinion(
            ModProjectile proj, ModBuff buff, int? projType, int searchRange, int travelSpeed, int inertia, int attackFrames = 30, bool defaultIdle = true)
        {
            AommMod?.Call("RegisterGroundedMinion", versionString, proj, buff, projType, searchRange, travelSpeed, inertia, attackFrames, defaultIdle);
        }

        /// <summary>
        /// Get the combat pet level of a player directly. Most stats on managed combat pets
        /// scale automatically with the player's combat pet level. 
        /// </summary>
        /// <param name="player">The player whose combat pet level should be retireved</param>
        /// <returns>The combat pet level of that player, based on the strongest pet emblem in their inventory</returns>
        internal static int GetPetLevel(Player player)
        {
            return ((int?) AommMod?.Call("GetPetLevel", versionString, player)) ?? 0;
        }
        #endregion
    }

    #region Auxiliary classes and interfaces
    /// <summary>
    /// Interface containing the names and types of the variables in the AoMM state.
    /// An object that implements this interface can be populated directly with a projectile's
    /// current AoMM state using mod.Call("GetStateDirect", versionString, projectile, stateImpl).  
    /// </summary>
    public interface IAoMMState
    {
        /// <summary>
        /// How quickly the minion should change directions while moving. Higher values lead to
        /// slower turning. Updated automatically for pets, set in the mod.Call for minions.
        /// </summary>
        int Inertia { get; set; }

        /// <summary>
        /// Whether AoMM expects the minion to be attacking an enemy on the current frame.
        /// </summary>
        bool IsAttacking { get; set; }

        /// <summary>
        /// Whether AoMM expects the minion to be idling on the current frame.
        /// </summary>
        bool IsIdle { get; set; }

        /// <summary>
        /// Whether AoMM expects the minion to be following the pathfinder on the current frame.
        /// </summary>
        bool IsPathfinding { get; set; }

        /// <summary>
        /// Whether this projectile is being treated as a combat pet.
        /// </summary>
        bool IsPet { get; set; }

        /// <summary>
        /// Max travel speed for the minion. Updated automatically for pets, set in the 
        /// mod.Call for minions.
        /// </summary>
        int MaxSpeed { get; set; }

        /// <summary>
        /// The position of the next bend in the pathfinding path, based on the minion's current
        /// position.
        /// </summary>
        Vector2? NextPathfindingTaret { get; set; }

        /// <summary>
        /// The position of the end of the pathfinding path.
        /// </summary>
        Vector2? PathfindingDestination { get; set; }

        /// <summary>
        /// The suggested originalDamage value for a combat pet based on the player's current combat pet level.
        /// </summary>
        int PetDamage { get; set; }

        /// <summary>
        /// The current combat pet level of the player the projectile belongs to.
        /// </summary>
        int PetLevel { get; set; }

        /// <summary>
        /// All possible NPC targets, ordered by proximity to the most relevant target.
        /// </summary>
        List<NPC> PossibleTargetNPCs { get; set; }

        /// <summary>
        /// The range (in pixels) over which the tactic enemy selection should search. Updated
        /// automatically for pets, set in the mod.Call for minions.
        /// </summary>
        int SearchRange { get; set; }

        /// <summary>
        /// The NPC selected as most relevant based on the minion's current tactic and search range.
        /// </summary>
        NPC TargetNPC { get; set; }
    }

    /// <summary>
    /// Utility class for accessing the AoMM state directly via
    /// mod.Call("GetStateDirect", versionString, projectile, stateImpl).  
    /// </summary>
    public class AoMMStateImpl : IAoMMState
    {
        public int MaxSpeed { get; set; }
        public int Inertia { get; set; }
        public int SearchRange { get; set; }
        public Vector2? NextPathfindingTaret { get; set; }
        public Vector2? PathfindingDestination { get; set; }
        public NPC TargetNPC { get; set; }
        public List<NPC> PossibleTargetNPCs { get; set; }
        public bool IsPet { get; set; }
        public int PetLevel { get; set; }
        public int PetDamage { get; set; }
        public bool IsPathfinding { get; set; }
        public bool IsAttacking { get; set; }
        public bool IsIdle { get; set; }
        public bool IsActive { get; set; }
    }


    /// <summary>
    /// Interface containing the names and types of the parameters used to determine the 
    /// behavior of managed minions and combat pets. These parameters are initially set in
    /// the registration mod.Call("RegisterXPet",...) or mod.Call("RegisterXMinion", ...).
    /// 
    /// An object that implements this interface can be populated directly with a projectile's
    /// current AoMM parameters using mod.Call("GetParamsDirect", versionString, projectile, paramsImpl).  
    /// 
    /// The AI parameters of an active projectile can be updated to match an object that implements
    /// this interface using mod.Call("UpdateParamsDirect", versionString, projectile, paramsImpl).  
    /// 
    /// An additional interface is provided below for parameters that are only relevant to minions,
    /// as they are updated automatically for combat pets based on the player's pet level.
    /// </summary>
    public interface IAoMMCombatPetParams
    {
        /// <summary>
        /// The projectile that the minion or pet fires. If null, the minion will use a
        /// melee attack.
        /// </summary>
        int? FiredProjectileId { get; set; }

        /// <summary>
        /// Whether this projectile should currently have cross-mod AI applied.
        /// By default, this flag is managed by AoMM and is set to true under the following conditions:
        /// - For pets, as long as the associated cross-mod buff is active
        /// - For minions, as long as the associated cross-mod buff is active, and the minion was
        ///   spawned from an item that provides that buff
        /// For simple use cases (most pets, and most minions that consist of a single projectile),
        /// this flag should be left as its default value.
        /// For more complicated use cases, such as a minion that is spawned as a sub-projectile of 
        /// another minion, this flag must be set manually.
        /// Once this flag has been set manually at least once for a projectile, AoMM will stop updating 
        /// it automatically, and it will maintain its latest set value. 
        /// </summary>
        bool IsActive { get; set; }

        /// <summary>
        /// How quickly this combat pet should turn, compared to the default combat pet AI.
        /// Lower values lead to faster turning. For best results, should be in the range of
        /// 0.5f to 1.5f. Default 1f. Has no effect on regular minion AI.
        /// </summary>
        float InertiaScaleFactor { get; set; }

        /// <summary>
        /// How quickly this combat pet should move, compared to the default combat pet AI.
        /// Higher values lead to faster movement speed. For best results, should be in the 
        /// range of 0.75f to 1.25f. Default 1f. Has no effect on regular minion AI.
        /// </summary>
        float MaxSpeedScaleFactor { get; set; }

        /// <summary>
        /// How quickly this combat pet should fire projectiles, compared to the default 
        /// combat pet AI. Lower values lead to a higher rate of fire. For best results, 
        /// should be in the range of 0.5f to 1.5f. Default 1f. Has no effect on regular 
        /// minion AI.
        /// </summary>
        float AttackFramesScaleFactor { get; set; }
    }

    /// <summary>
    /// Interface containing the names and types of the parameters used to determine the 
    /// behavior of managed minions and combat pets. These parameters are initially set in
    /// the registration mod.Call("RegisterXMinion",...).
    /// 
    /// An object that implements this interface can be populated directly with a projectile's
    /// current AoMM parameters using mod.Call("GetParamsDirect", versionString, projectile, paramsImpl).  
    /// 
    /// The AI parameters of an active projectile can be updated to match an object that implements
    /// this interface using mod.Call("UpdateParamsDirect", versionString, projectile, paramsImpl).  
    ///
    /// The values in this interface can only be updated for minions, as they are updated automatically
    /// for pets.
    /// </summary>
    public interface IAoMMParams : IAoMMCombatPetParams
    {
        /// <summary>
        /// How quickly the minion should change directions while moving. Higher values lead to
        /// slower turning. 
        /// </summary>
        int Inertia { get; set; }

        /// <summary>
        /// Max travel speed for the minion.
        /// </summary>
        int MaxSpeed { get; set; }

        /// <summary>
        /// The range (in pixels) over which the tactic enemy selection should search.
        /// </summary>
        int SearchRange { get; set; }

        /// <summary>
        /// The projectile firing rate for the minion, if that minion fires a projectile. Only
        /// applies to projectile-firing minions. The attack speed of melee minions is derived
        /// from their movement speed.
        /// </summary>
        int AttackFrames { get; set; }
    }

    public class AoMMParamsImpl : IAoMMParams
    {
        public bool IsActive { get; set; }
        public int Inertia { get; set; }
        public int MaxSpeed { get; set; }
        public int SearchRange { get; set; }
        public int AttackFrames { get; set; }
        public int? FiredProjectileId { get; set; }
        public float InertiaScaleFactor { get; set; }

        public float MaxSpeedScaleFactor { get; set; }

        public float AttackFramesScaleFactor { get; set; }
    }
    #endregion
}

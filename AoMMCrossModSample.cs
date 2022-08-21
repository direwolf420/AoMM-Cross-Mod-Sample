using AoMMCrossModSample.Pets.SampleFlyingRangedPet;
using AoMMCrossModSample.Pets.SampleGroundedPet;
using AoMMCrossModSample.Pets.SampleGroundedRangedPet;
using AoMMCrossModSample.Pets.SampleFlyingPet;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using AoMMCrossModSample.Pets.SampleCustomPet;

namespace AoMMCrossModSample
{
	public class AoMMCrossModSample : Mod
	{
		public override void PostSetupContent()
		{
			RegisterPets();
		}

		private static void RegisterPets()
		{
			// Register melee, flying cross mod pet
			AmuletOfManyMinionsApi.RegisterFlyingPet(
				GetInstance<SampleFlyingPetProjectile>(), GetInstance<SampleFlyingPetBuff>(), null);

			// To add a projectile attack to the combat pet, pass in a non-null third parameter
			AmuletOfManyMinionsApi.RegisterFlyingPet(
				GetInstance<SampleFlyingRangedPetProjectile>(), GetInstance<SampleFlyingRangedPetBuff>(), ProjectileID.FrostDaggerfish);

			// Register melee, grounded cross mod pet
			AmuletOfManyMinionsApi.RegisterGroundedPet(
				GetInstance<SampleGroundedPetProjectile>(), GetInstance<SampleGroundedPetBuff>(), null);

			// Add a ranged attack
			AmuletOfManyMinionsApi.RegisterGroundedPet(
				GetInstance<SampleGroundedRangedPetProjectile>(), GetInstance<SampleGroundedRangedPetBuff>(), ProjectileID.PoisonDartBlowgun);

			// Apply combat pet AI to a projectile that is not a clone of a vanilla pet
			AmuletOfManyMinionsApi.RegisterFlyingPet(
				GetInstance<SampleCustomPetProjectile>(), GetInstance<SampleCustomPetBuff>(), null);

		}
    }
}
﻿using AoMMCrossModSample.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AoMMCrossModSample.Pets.SampleMeleeRangedPet
{
    // Code largely adapted from tModLoader Sample Mod
    internal class SampleMeleeRangedPetProjectile : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.PetLizard;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = Main.projFrames[ProjectileID.PetLizard];
            Main.projPet[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.PetLizard);
            AIType = ProjectileID.PetLizard;
            // This appears to be necessary for visual purposes
            DrawOriginOffsetY = -12;
        }

        public override bool PreAI()
        {
            // unset default buff
            Main.player[Projectile.owner].lizard = false;
            return true;
        }

        public override void AI()
        {
            if (Main.player[Projectile.owner].HasBuff(BuffType<SampleMeleeRangedPetBuff>()))
            {
                Projectile.timeLeft = 2;
            }

            // If we're AoMM-managed, set the fired projectile based on AoMM pet level 
            if(AmuletOfManyMinionsApi.TryGetStateDirect(this, out var modState) && 
                AmuletOfManyMinionsApi.TryGetParamsDirect(this, out var modParams))
            {
                modParams.FiredProjectileId = modState.PetLevel > 2 ? ProjectileType<FrostDaggerfishCloneProjectile>() : null;
                // need to explicitly write updates to the params back,
                // TODO: there might be some more advanced form of reflection that can be used to make this automatic
                AmuletOfManyMinionsApi.UpdateParamsDirect(this, modParams);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // make it red to distinguish from vanilla
            lightColor = Color.LightBlue.MultiplyRGB(lightColor * 1.5f);
            return true;
        }

    }
}

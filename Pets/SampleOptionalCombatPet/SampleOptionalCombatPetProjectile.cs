﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AoMMCrossModSample.Pets.SampleOptionalCombatPet
{
    // Code largely adapted from tModLoader Sample Mod
    internal class SampleOptionalCombatPetProjectile : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Turtle;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = Main.projFrames[ProjectileID.Turtle];
            Main.projPet[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Turtle);
            AIType = ProjectileID.Turtle;
            // This appears to be necessary for visual purposes
            DrawOriginOffsetY = -8;
        }

        public override bool PreAI()
        {
            // unset default buff
            Main.player[Projectile.owner].turtle = false;
            return true;
        }

        public override void AI()
        {
            if (Main.player[Projectile.owner].HasBuff(BuffType<SampleOptionalCombatPetBuff>()))
            {
                Projectile.timeLeft = 2;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // make it purple to distinguish from vanilla
            lightColor = Color.Purple.MultiplyRGB(lightColor * 1.5f);
            return true;
        }

    }
}

﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AoMMCrossModSample.Minions.SampleGroundedMinion
{
    // Code largely adapted from tModLoader Sample Mod
    internal class SampleGroundedMinionItem : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.VampireFrogStaff;

        public override void SetStaticDefaults()
        {
            ItemID.Sets.GamepadWholeScreenUseRange[Type] = true;
            ItemID.Sets.LockOnIgnoresCollision[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.FlinxStaff);
            Item.shoot = ProjectileType<SampleGroundedMinionProjectile>();
            Item.buffType = BuffType<SampleGroundedMinionBuff>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);
            var proj = Projectile.NewProjectileDirect(source, Main.MouseWorld, default, type, damage, knockback, Main.myPlayer);
            proj.originalDamage = Item.damage;
            return false;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            spriteBatch.Draw(TextureAssets.Item[Type].Value, position, frame, Color.SkyBlue, 0, origin, scale, 0, 0);
            return false;
        }
    }
}

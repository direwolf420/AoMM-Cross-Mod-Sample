﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AoMMCrossModSample.Pets.SampleRapidFirePet
{
    internal class SampleRapidFirePetItem : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.ZephyrFish;

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish);
            Item.shoot = ProjectileType<SampleRapidFirePetProjectile>();
            Item.buffType = BuffType<SampleRapidFirePetBuff>();
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600);
            }
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            spriteBatch.Draw(TextureAssets.Item[Type].Value, position, frame, Color.Violet, 0, origin, scale, 0, 0);
            return false;
        }
    }
}

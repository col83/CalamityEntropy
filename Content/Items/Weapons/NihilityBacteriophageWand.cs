﻿using CalamityEntropy.Content.Buffs;
using CalamityEntropy.Content.Projectiles;
using CalamityMod.Items;
using CalamityMod.Rarities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Items.Weapons
{
    public class NihilityBacteriophageWand : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 270;
            Item.DamageType = DamageClass.Summon;
            Item.width = 90;
            Item.height = 88;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.knockBack = 2;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shoot = ModContent.ProjectileType<NihilityMinion>();
            Item.shootSpeed = 2f;
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item8;
            Item.noMelee = true;
            Item.mana = 10;
            Item.buffType = ModContent.BuffType<NihilityBacteriophageBuff>();
            Item.rare = ModContent.RarityType<Turquoise>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 3);
            int projectile = Projectile.NewProjectile(source, Main.MouseWorld, velocity, type, Item.damage, knockback, player.whoAmI, 0, 1, 0);
            Main.projectile[projectile].originalDamage = Item.damage;

            return false;
        }

    }
}

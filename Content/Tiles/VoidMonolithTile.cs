﻿using CalamityEntropy.Content.Items;
using CalamityMod.Dusts;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalamityEntropy.Content.Tiles
{
    public class VoidMonolithTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ModContent.ItemType<VoidMonolith>());
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Origin = new Point16(0, 2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 18 };
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 4, 0);
            TileObjectData.addTile(Type);
            TileID.Sets.HasOutlines[Type] = true;
            AddMapEntry(new Color(63, 72, 200));
            TileID.Sets.DisableSmartCursor[Type] = false;
            DustType = (int)CalamityDusts.PurpleCosmilite;
            AnimationFrameHeight = 52;
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (Main.tile[i, j].TileFrameY < 56)
            {
                return;
            }

            Player player = Main.LocalPlayer;
            if (player is null)
            {
                return;
            }

            if (player.active)
            {
                Main.LocalPlayer.Entropy().crSky = 30;
            }
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frameCounter++;
            if (frameCounter >= 7.2)
            {
                frameCounter = 0;
                if (++frame >= 5)
                {
                    frame = 0;
                }
            }
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<VoidMonolith>();
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return true;
        }

        public override bool RightClick(int i, int j)
        {
            HitWire(i, j);
            SoundEngine.PlaySound(SoundID.Mech, new Vector2(i * 16, j * 16));
            return true;
        }

        public override void HitWire(int i, int j)
        {
            int x = i - Main.tile[i, j].TileFrameX / 18 % 2;
            int y = j - Main.tile[i, j].TileFrameY / 18 % 3;
            int tileXX18 = 56;
            for (int l = x; l < x + 2; l++)
            {
                for (int m = y; m < y + 3; m++)
                {
                    if (Main.tile[l, m].HasTile && Main.tile[l, m].TileType == Type)
                    {
                        if (Main.tile[l, m].TileFrameY < tileXX18)
                            Main.tile[l, m].TileFrameY += (short)(tileXX18);
                        else
                            Main.tile[l, m].TileFrameY -= (short)(tileXX18);
                    }
                }
            }
            if (Wiring.running)
            {
                for (int o = 0; o < 2; o++)
                {
                    for (int p = 0; p < 3; p++)
                    {
                        Wiring.SkipWire(x + 0, x + p);
                    }
                }
            }
            NetMessage.SendTileSquare(-1, x, y + 1, 3);
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Texture2D texture;
            texture = TextureAssets.Tile[Type].Value;
            Vector2 zero = new(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            int height = 18;
            int animate = 0;
            if (tile.TileFrameY >= 56)
            {
                animate = Main.tileFrame[Type] * 56;
            }
            Main.spriteBatch.Draw(texture, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY + animate, 16, height), Lighting.GetColor(i, j), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using static Terraria.ModLoader.ModContent;

namespace TrueEndless
{
    class EndlessItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool CloneNewInstances => true;
        public bool wasConsumable = false;
        public int properStack = 0;

        private bool HasInfinity() => Main.player[Main.myPlayer].GetModPlayer<EndlessPlayer>().infinity;
        private bool HasInfinity(Player player) => player.GetModPlayer<EndlessPlayer>().infinity;
        public bool IsEndlessAmmo(Item item) => (item.stack >= item.maxStack || item.stack >= 3996) && item.maxStack > 1 && item.ammo != AmmoID.None;
        public bool IsEndlessConsumable(Item item) => (item.stack >= item.maxStack || item.stack >= 3996) && item.maxStack > 1 && (item.consumable || wasConsumable) && item.createTile == -1 && item.createWall == -1;
        public bool IsEndlessSpecific(Item item)
        {
            int[] ids = new int[]
            {
                ItemID.Torch,
                ItemID.BeachBall,
            };
            return (item.stack >= item.maxStack || item.stack >= 3996) && ids.Contains(item.type);
        }

        public bool IsEndlessExempt(Item item)
        {
            int[] ids = new int[]
            {

            };
            return (ids.Contains(item.type));
        }

        // THIS IS CALLED IN THE MAIN MENU
        // WTF
        public override bool PreDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (properStack != 0)
            {
                item.stack = properStack;
                properStack = 0;
            }
            if ((Main.gameMenu || HasInfinity()) && GetInstance<EndlessConfigClient>().RainbowSprite && !IsEndlessExempt(item) && ((IsEndlessAmmo(item) && !item.notAmmo) || IsEndlessConsumable(item) || IsEndlessSpecific(item)))
            {
                Texture2D texture = Main.itemTexture[item.type];
                spriteBatch.Draw(texture, position, null, Main.DiscoColor, 0, origin, scale, SpriteEffects.None, 0f);
                return false;
            }
            return true;
        }

        private bool IsEndlessTooltip(TooltipLine tooltip) => tooltip.mod == mod.Name && tooltip.Name == "EndlessNotice";

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            tooltips.RemoveAll(IsEndlessTooltip);
            if (HasInfinity() && !IsEndlessExempt(item) && ((IsEndlessAmmo(item) && !item.notAmmo) || IsEndlessConsumable(item) || IsEndlessSpecific(item)))
            {
                string text = "Endless";
                if (GetInstance<EndlessConfigClient>().RainbowTooltip)
                {
                    text = "[c/" + Main.DiscoColor.Hex3() + ":" + text + "]";
                }
                tooltips.Add(new TooltipLine(mod, "EndlessNotice", text));
            }
        }

        public override void UpdateInventory(Item item, Player player)
        {
            if (HasInfinity(player) && !IsEndlessExempt(item) && (IsEndlessConsumable(item) || IsEndlessSpecific(item)))
            {
                wasConsumable = true;
                item.consumable = false;
            }
            else if (wasConsumable)
            {
                item.consumable = true;
            }

            if (HasInfinity(player) && !IsEndlessExempt(item) && IsEndlessConsumable(item) && item.buffType != 0)
            {
                player.AddBuff(item.buffType, 2);
            }
        }

        public override void GetHealLife(Item item, Player player, bool quickHeal, ref int healValue)
        {
            properStack = item.stack;
        }
    }
}

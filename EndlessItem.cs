using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader.Config;

namespace TrueEndless
{
    class EndlessItem : GlobalItem
    {
        private readonly EndlessConfigServer cfg = GetInstance<EndlessConfigServer>();
        public override bool InstancePerEntity => true;
        public override bool CloneNewInstances => true;
        public bool wasConsumable = false;
        public int properStack = 0;

        private bool HasInfinity() => HasInfinity(Main.player[Main.myPlayer]);
        private bool HasInfinity(Player player) => (cfg.StartEnaled && player.GetModPlayer<EndlessPlayer>().usedInfinity) || player.GetModPlayer<EndlessPlayer>().infinity;
        public bool IsEndlessAmmo(Item item) => 
            (item.stack >= item.maxStack || item.stack >= cfg.AmmoStack) &&
            !item.notAmmo &&
            item.ammo != 0 &&
            !IsntEndlessSpecific(item);
        public bool IsEndlessConsumable(Item item) =>
            (item.stack >= item.maxStack || item.stack >= cfg.ConsumableStack) &&
            (item.consumable || wasConsumable) && 
            item.createTile == -1 && 
            item.createWall == -1 &&
            !IsntEndlessSpecific(item);
        public bool IsEndlessPotion(Item item) =>
            (item.stack >= item.maxStack || item.stack >= cfg.PotionStack) && 
            item.buffType != 0 &&
            item.damage <= 0 &&
            !IsntEndlessSpecific(item);
        public bool IsInItemDefinition(List<ItemDefinition> l, Item item)
        {
            foreach (ItemDefinition i in l)
            {
                if (i.Type == item.type) { return true;  }
            }
            return false;
        }
        public (bool, int) IsInItemDefinition(Dictionary<ItemDefinition, int> d, Item item)
        {
            foreach (KeyValuePair<ItemDefinition, int> kv in d)
            {
                if (kv.Key.Type == item.type) { return (true, kv.Value); }
            }
            return (false, -1);
        }
        public bool IsEndlessSpecific(Item item)
        {
            (bool, int) idl = IsInItemDefinition(cfg.OverrideEnable, item);
            return (item.stack >= item.maxStack || item.stack >= idl.Item2) && idl.Item1;
        }

        public bool IsntEndlessSpecific(Item item)
        {
            return IsInItemDefinition(cfg.OverrideDisable, item);
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
            if ((Main.gameMenu || HasInfinity()) && GetInstance<EndlessConfigClient>().RainbowSprite && (IsEndlessAmmo(item) || IsEndlessConsumable(item) || IsEndlessPotion(item) || IsEndlessSpecific(item)))
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
            if (HasInfinity() && IsEndlessAmmo(item) || IsEndlessConsumable(item) || IsEndlessSpecific(item))
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
            if (HasInfinity(player) && (IsEndlessConsumable(item) || IsEndlessSpecific(item)))
            {
                wasConsumable = true;
                item.consumable = false;
            }
            else if (wasConsumable)
            {
                item.consumable = true;
            }

            player.GetModPlayer<EndlessPlayer>().EndlessPotion(item);
        }

        public override void GetHealLife(Item item, Player player, bool quickHeal, ref int healValue)
        {
            properStack = item.stack;
        }
    }
}

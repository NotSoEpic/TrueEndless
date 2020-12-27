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

        // returns true if the player has infinity enabled
        private bool HasInfinity() => HasInfinity(Main.player[Main.myPlayer]);
        private bool HasInfinity(Player player) => player.GetModPlayer<EndlessPlayer>().HasInfinity;
        // is ammo that can be endless
        public bool IsEndlessAmmo(Item item) => 
            (item.stack >= item.maxStack || item.stack >= cfg.AmmoStack) &&
            item.maxStack > 1 &&
            !item.notAmmo &&
            item.ammo != 0 &&
            !IsntEndlessSpecific(item);
        // is a consumable item that can be endless
        public bool IsEndlessConsumable(Item item) =>
            (item.stack >= item.maxStack || item.stack >= cfg.ConsumableStack) &&
            item.maxStack > 1 &&
            (item.consumable || wasConsumable) && 
            item.createTile == -1 && 
            item.createWall == -1 &&
            !IsntEndlessSpecific(item);
        // is a potion that can be endless
        public bool IsEndlessPotion(Item item) =>
            (item.stack >= item.maxStack || item.stack >= cfg.PotionStack) &&
            item.maxStack > 1 &&
            (item.buffType != 0 || item.healLife > 0 || item.healMana > 0) &&
            item.damage <= 0 &&
            !IsntEndlessSpecific(item);
        // returns true if the item is in a list of item definitions
        public bool IsInItemDefinition(List<ItemDefinition> l, Item item)
        {
            return l.Contains(new ItemDefinition(item.type));
        }
        // returns true and an associated int if the item is a key in the dictionary
        public (bool, int) IsInItemDefinition(Dictionary<ItemDefinition, int> d, Item item)
        {
            foreach (KeyValuePair<ItemDefinition, int> kv in d) // loops through the key-value pairs of the dictionary
            {
                if (kv.Key.Type == item.type) { return (true, kv.Value); }
            }
            return (false, -1);
        }
        // returns true if the item is part of the dictionary defined by the mod config, and its stack size is greater than or equal to the associated int
        public bool IsEndlessSpecific(Item item)
        {
            (bool, int) idl = IsInItemDefinition(cfg.OverrideEnable, item);
            return (item.stack >= item.maxStack || item.stack >= idl.Item2) && idl.Item1;
        }

        // returns true if the item is part of the list defined by the mod config
        public bool IsntEndlessSpecific(Item item)
        {
            return IsInItemDefinition(cfg.OverrideDisable, item);
        }

        /*public bool GrantsSameBuff(Item item)
        {
            foreach (ItemDefinition potion in cfg.BuffDisable)
            {
                // I want to get the buffType from an ItemDefinition
                if (item.buffType == GetModItem(potion.Type).buffType) { return true; }
            }
            return false;
        }*/

        // returns true if the item is endless ammo, consumable, potion, or specifically defined, and isnt excluded
        public bool IsEndless(Item item)
        {
            return (IsEndlessAmmo(item) || IsEndlessConsumable(item) || IsEndlessPotion(item) || IsEndlessSpecific(item)) && !IsntEndlessSpecific(item);
        }

        // runs for every item in the inventory every tick. Importantly, its run *after* a used itemstack is decreased
        public override bool PreDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            // resets the stack size back to what it was if reduced in certain cases
            if (properStack != 0)
            {
                item.stack = properStack;
                properStack = 0;
            }
            // makes the sprite rainbowy
            if ((Main.gameMenu || HasInfinity()) && GetInstance<EndlessConfigClient>().RainbowSprite && (IsEndless(item)))
            {
                Texture2D texture = Main.itemTexture[item.type];
                spriteBatch.Draw(texture, position, null, Main.DiscoColor, 0, origin, scale, SpriteEffects.None, 0f); // Main.DiscoColor is my favourite variable :)
                return false;
            }
            return true;
        }

        private bool IsEndlessTooltip(TooltipLine tooltip) => tooltip.mod == mod.Name && tooltip.Name == "EndlessNotice";

        // adds the endless tooltip
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            tooltips.RemoveAll(IsEndlessTooltip); // removes the endless tooltip
            if (HasInfinity() && IsEndless(item))
            {
                string text = "Endless";
                if (GetInstance<EndlessConfigClient>().RainbowTooltip) // changes the colour to be rainbowy if enabled
                {
                    text = "[c/" + Main.DiscoColor.Hex3() + ":" + text + "]";
                }
                tooltips.Add(new TooltipLine(mod, "EndlessNotice", text));
            }
        }

        public override void UpdateInventory(Item item, Player player)
        {
            // if an item is endless, prevents if from being consumed
            if (HasInfinity(player) && IsEndless(item) && (item.consumable || wasConsumable))
            {
                wasConsumable = true;
                item.consumable = false; // just decides whether or not the item stack is decreased
            }
            else if (wasConsumable) // if the item loses its endless state for some reason and was consumable, it becomes consumable again
            {
                item.consumable = true;
            }

            // does stuff with applying buffs over in EndlessPlayer.cs
            player.GetModPlayer<EndlessPlayer>().EndlessPotion(item);
        }

        // one of the cases where the stack size reset in PreDrawInInventory is used
        public override void GetHealLife(Item item, Player player, bool quickHeal, ref int healValue)
        {
            properStack = item.stack;
        }
    }
}

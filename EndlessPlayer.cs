using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;

namespace TrueEndless
{
    class EndlessPlayer : ModPlayer
    {
        public bool infinity = false;
        public bool usedInfinity = false;
        private readonly EndlessConfigServer cfgs = GetInstance<EndlessConfigServer>();
        private readonly EndlessItem eI = GetInstance<EndlessItem>();

        public bool HasInfinity => infinity || (cfgs.StartEnaled && !usedInfinity);

        // saves infinity and usedInfinity variables
        public override TagCompound Save()
        {
            return new TagCompound
            {
                {"infinityItems", infinity},
                {"usedInfinity", usedInfinity}
            };
        }

        // loads infinity and usedInfinity variables
        public override void Load(TagCompound tag)
        {
            infinity = tag.GetBool("infinityItems");
            usedInfinity = tag.GetBool("usedInfinity");
        }

        // applies endless buff potions in piggy bank and co. if enabled in the config
        public override void PreUpdateBuffs()
        {
            if (cfgs.PiggyBankPotions)
            {
                foreach (Item item in player.bank.item) // piggy bank
                {
                    EndlessPotion(item);
                }
                foreach (Item item in player.bank2.item) // safe
                {
                    EndlessPotion(item);
                }
                foreach (Item item in player.bank3.item) // defender forge
                {
                    EndlessPotion(item);
                }
            }
        }

        // if an item is an endless potion, applies its effect
        public void EndlessPotion(Item item)
        {
            if (HasInfinity && !eI.IsntEndlessSpecific(item) && eI.IsEndlessPotion(item))
            {
                player.AddBuff(item.buffType, 2);
            }
        }

        public override bool ConsumeAmmo(Item weapon, Item ammo)
        {
            return !(HasInfinity && !eI.IsntEndlessSpecific(ammo) && (eI.IsEndlessAmmo(ammo) || eI.IsEndlessSpecific(ammo)));
        }
    }
}

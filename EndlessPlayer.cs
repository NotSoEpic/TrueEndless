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

        public override TagCompound Save()
        {
            return new TagCompound
            {
                {"infinityItems", infinity},
                {"usedInfinity", usedInfinity}
            };
        }

        public override void Load(TagCompound tag)
        {
            infinity = tag.GetBool("infinityItems");
            usedInfinity = tag.GetBool("usedInfinity");
        }

        public override void PreUpdateBuffs()
        {
            if (cfgs.PiggyBankPotions)
            {
                foreach (Item item in player.bank.item)
                {
                    EndlessPotion(item);
                }
                foreach (Item item in player.bank2.item)
                {
                    EndlessPotion(item);
                }
                foreach (Item item in player.bank3.item)
                {
                    EndlessPotion(item);
                }
            }
        }

        public void EndlessPotion(Item item)
        {
            if (infinity && !eI.IsntEndlessSpecific(item) && eI.IsEndlessPotion(item) && item.buffType != 0)
            {
                player.AddBuff(item.buffType, 2);
            }
        }

        public override bool ConsumeAmmo(Item weapon, Item ammo)
        {
            return !((infinity || (cfgs.StartEnaled && usedInfinity)) && !eI.IsntEndlessSpecific(ammo) && (eI.IsEndlessAmmo(ammo) || eI.IsEndlessSpecific(ammo)));
        }
    }
}

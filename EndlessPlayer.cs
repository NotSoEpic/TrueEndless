using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TrueEndless
{
    class EndlessPlayer : ModPlayer
    {
        public bool infinity;
        private EndlessItem eI = new EndlessItem();

        public override TagCompound Save()
        {
            return new TagCompound
            {
                {"infinityItems", infinity},
            };
        }

        public override void Load(TagCompound tag)
        {
            infinity = tag.GetBool("infinityItems");
        }

        public override bool ConsumeAmmo(Item weapon, Item ammo)
        {
            return !(infinity && !eI.IsEndlessExempt(ammo) && (eI.IsEndlessAmmo(ammo) || eI.IsEndlessSpecific(ammo)));
        }
    }
}

using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace TrueEndless
{
    public class EndlessConfigClient : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        [DefaultValue(true)]
        public bool RainbowSprite { get; set; }

        [DefaultValue(true)]
        public bool RainbowTooltip { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.Config;

namespace TrueEndless
{
    // all of this is pretty self-explanatory
    [Label("Client Config")]
    public class EndlessConfigClient : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(true)]
        [Label("Rainbow Sprite")]
        [Tooltip("Makes infinite items get all disco-y")]
        public bool RainbowSprite { get; set; }

        [DefaultValue(true)]
        [Label("Rainbow Tooltip")]
        [Tooltip("Makes the \"Endless\" tooltip on items get all disco-y")]
        public bool RainbowTooltip { get; set; }
    }

    [Label("Server Config")]
    public class EndlessConfigServer : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [DefaultValue(true)]
        [Label("Piggy Bank Potions")]
        [Tooltip("Controls whether the auto potion effect persists when placed in a piggy bank")]
        public bool PiggyBankPotions { get; set; }

        [DefaultValue(false)]
        [Label("Start Enabled")]
        [Tooltip("The endless effect will be enabled even if you have never used the Endless Enabler. The effect will be disabled if this option is set to false before you use the item")]
        public bool StartEnaled { get; set; }

        [Label("Ammo Stack")]
        [Tooltip("Minimum stack for all ammo, can be reduced case-by-case with override enable")]
        [DefaultValue(3996)]
        public int AmmoStack;

        [Label("Consumable Stack")]
        [Tooltip("Minimum stack for all consumable items, can be reduced case-by-case with override enable")]
        [DefaultValue(3996)]
        public int ConsumableStack;

        [Label("Potion Stack")]
        [Tooltip("Minimum stack for all potions, can be reduced case-by-case with override enable")]
        [DefaultValue(30)]
        public int PotionStack;

        [Label("Override Enable")]
        [Tooltip("Define extra endless items here, or reduce the minimum stack size of already endless items")]
        public Dictionary<ItemDefinition, int> OverrideEnable { get; set; } = new Dictionary<ItemDefinition, int>()
        {
            [new ItemDefinition(ItemID.Torch)] = 396,
            [new ItemDefinition(ItemID.BeachBall)] = 1
        };

        [Label("Override Disable")]
        [Tooltip("Define items to not be endless here")]
        public List<ItemDefinition> OverrideDisable { get; set; } = new List<ItemDefinition>();

        /*[Label("Buff Disable")]
        [Tooltip("The buff granted by defined potions won't be constantly granted. You can still use the potion to receive the buff though")]
        public List<ItemDefinition> BuffDisable = new List<ItemDefinition>()
        {
            new ItemDefinition(ItemID.InvisibilityPotion)
        };*/
    }   
}

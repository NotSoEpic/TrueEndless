using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TrueEndless.Items
{
	public class EndlessEnabler : ModItem
	{
		public override void SetStaticDefaults() 
		{
			// DisplayName.SetDefault("EndlessEnabler"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("Use to toggle endless items\nItems must be a full stack");
		}

		public override void SetDefaults() 
		{
			item.width = 22;
			item.height = 22;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.value = 1;
			item.rare = 1;
            item.expert = true;
            item.UseSound = SoundID.Item29;
		}

		public override void AddRecipes() 
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.EndlessMusketPouch, 1);
            recipe.AddIngredient(ItemID.EndlessQuiver, 1);
            recipe.AddIngredient(ItemID.FlamingArrow, 100);
            recipe.AddIngredient(ItemID.UnholyArrow, 100);
            recipe.AddIngredient(ItemID.JestersArrow, 100);
            recipe.AddIngredient(ItemID.HellfireArrow, 100);
            recipe.AddIngredient(ItemID.SilverBullet, 100);
            recipe.AddIngredient(ItemID.MeteorShot, 100);
            recipe.AddIngredient(ItemID.Shuriken, 100);
            recipe.AddIngredient(ItemID.ThrowingKnife, 100);
            recipe.AddIngredient(ItemID.HealingPotion, 15);
            recipe.AddIngredient(ItemID.RegenerationPotion, 10);
            recipe.AddIngredient(ItemID.SwiftnessPotion, 10);
            recipe.AddIngredient(ItemID.IronskinPotion, 10);
            recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

        public override bool UseItem(Player player)
        {
            EndlessPlayer p = player.GetModPlayer<EndlessPlayer>();
            if (p.infinity)
            {
                if (Main.myPlayer == player.whoAmI)
                {
                    Main.NewText("Endless Items Disabled", new Color(242, 24, 24));
                }
                p.infinity = false;
            } else
            {
                if (Main.myPlayer == player.whoAmI)
                {
                    Main.NewText("Endless Items Enabled", new Color(24, 242, 24));
                }
                p.infinity = true;
            }
            return true;
        }

        // I want the rainbow name but not the expert tooltip
        private bool IsExpertTooltip(TooltipLine tooltip) => tooltip.mod == "Terraria" && tooltip.Name == "Expert";
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.RemoveAll(IsExpertTooltip);
        }
    }
}
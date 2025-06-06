using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace NpcItemFinder;

class AddShopTooltips : GlobalItem
{
    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        foreach (string key in NpcItemFinder.shops.Keys)
        {
            foreach (var entry in NpcItemFinder.shops[key].Entries)
            {
                if (entry.Item.type == item.type)
                {
                    // TODO: Make the coin display prettier
                    tooltips.Add(new TooltipLine(Mod, "whoSoldBy", $"[c/FFF014:Sold by: {key} for {item.value}] [i:71]")); // Copper coin is item id 71
                }
            }
        }
    }
}
using System;
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
                    int[] cost = Util.ConvertCopperToCoins(item.value);
                    string costStr = "";
                    if (cost[0] != 0)
                    {
                        costStr += $"{cost[0]} [i:74]";
                    }
                    if (cost[1] != 0)
                    {
                        costStr += $"{cost[1]} [i:73]";
                    }
                    if (cost[2] != 0)
                    {
                        costStr += $"{cost[2]} [i:72]";
                    }
                    if (cost[3] != 0)
                    {
                        costStr += $"{cost[3]} [i:71]";
                    }


                    tooltips.Add(new TooltipLine(Mod, "whoSoldBy", $"[c/FFF014:Sold by: {key} for (withhout factroing happiniess or other discounts/increases)] " + costStr)); // Copper coin is item id 71
                }
            }
        }
    }
}
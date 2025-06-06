using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NpcItemFinder;

class AddShopTracker : GlobalNPC
{
    public override void ModifyShop(NPCShop shop)
    {




        string NpcName = Lang.GetNPCName(shop.NpcType).ToString();
        try
        {
            NpcItemFinder.shops.Add(NpcName, shop);
            Mod.Logger.Info($"Added shop: {shop.Entries} for npc {NpcName}");
        }
        catch (ArgumentException)
        {

        }

    }
}
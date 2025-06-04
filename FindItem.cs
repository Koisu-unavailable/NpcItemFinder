using System;
using System.Collections.Generic;
using System.Linq;
using log4net.Repository.Hierarchy;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace NpcItemFinder
{
    public class FindItem : ModCommand
    {
        public override string Command => "findItem";
        public override string Usage => "Find which NPC sells an item. I.E. /findItem Copper Shortsword";
        public override CommandType Type => CommandType.Chat;
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            string itemName = string.Join("", input.Split(' ')[1..]);
            Mod.Logger.InfoFormat($"\"{caller.Player.name}\" inputted: \"{input}\", searching for \"{itemName}\"");
            if (args.Length == 0)
            {
                caller.Reply("Please put an item name. There can be spaces.");
                return;
            }
            caller.Reply($"Searching for {itemName}");
            foreach (NPC npc in Main.npc)
            {
                string NpcName = Lang.GetNPCName(npc.type).ToString();
                if (NpcItemFinder.shops.TryGetValue(NpcName, out NPCShop shop))
                {
                    foreach (var entry in shop.ActiveEntries)
                    {
                        if (entry.Item.Name.Contains(itemName.ToLower(), StringComparison.OrdinalIgnoreCase))
                        {
                            caller.Reply($"{npc.FullName} has {entry.Item.Name} [i:{entry.Item.type}]");
                        }
                    }
                }

                else
                {
                    continue;
                }
            }




        }

    }
}
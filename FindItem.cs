using System;
using System.Collections.Generic;
using System.Linq;
using Humanizer;
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
        public override string Description => "Find which npc sells I item using Levenshtein fuzzy searching.";
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
            var results = SearchItem(itemName);
            foreach (var key in results.Keys)
            {
                caller.Reply($"Npc: {key} has the item: {results[key].Name}");
            }

        }
        public static Dictionary<string, Item> SearchItem(string item)
        {
            Dictionary<string, Item> resultItems = [];
            Dictionary<string, List<Item>> currentItems = [];
            foreach (NPC npc in Main.npc)
            {
                string NpcName = Lang.GetNPCName(npc.type).ToString();
                if (NpcItemFinder.shops.TryGetValue(NpcName, out NPCShop shop))
                {
                    foreach (var entry in shop.ActiveEntries)
                    {
                        if (currentItems.Keys.Contains(NpcName))
                        {
                            currentItems[NpcName].Add(entry.Item);
                        }

                        else
                        {
                            currentItems.Add(NpcName, [entry.Item]);
                        }
                    }
                }

                else
                {
                    continue;
                }
            }
            List<string> currentItemNames = [];
            foreach (string key in currentItems.Keys)
            {
                foreach (var i in currentItems[key])
                {
                    currentItemNames.Add(i.Name);
                }
            }
            List<string> matchingItemNames = Util.FuzzySearch(item, currentItemNames.ToArray(), 0.3f, 2, 4);
            foreach (string key in currentItems.Keys)
            {
                foreach (var i in currentItems[key])
                {
                    if (matchingItemNames.Contains(i.Name))
                    {
                        resultItems.Add(key, i);
                    }
                }

            }

            return resultItems;
        }


    }
}
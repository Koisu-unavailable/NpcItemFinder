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
        public override string Usage => "Find which NPC sells an item. I.E. /findItem Copper Shortsword.";
        public override string Description => "Find which npc sells an item.";
        public override CommandType Type => CommandType.Chat;
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            string itemName = string.Join(" ", input.Split(' ')[1..]);
            Mod.Logger.InfoFormat($"\"{caller.Player.name}\" inputted: \"{input}\", searching for \"{itemName}\"");
            if (args.Length == 0)
            {
                caller.Reply("Please put an item name. There can be spaces.");
                return;
            }
            caller.Reply($"Searching for {itemName}");
            var results = SearchItem(itemName);
            if (results.Count >= ModContent.GetInstance<NpcItemFinderConfig>().ItemDisplayLimit)
            {
                caller.Reply($"Showing {ModContent.GetInstance<NpcItemFinderConfig>().ItemDisplayLimit} items of {results.Count} total items found . This can be changed in the mod's config.");
                results = TrimResults(results, itemName, ModContent.GetInstance<NpcItemFinderConfig>().ItemDisplayLimit);
            }
            if (!results.Any())
            {
                caller.Reply("No results found!");
            }

            foreach (var key in results.Keys)
            {
                foreach (var i in results[key])
                {
                    caller.Reply($"Npc: {key} has the item: {i.Name} [i:{i.type}]");
                }
            }

        }
        private static Dictionary<string, List<Item>> TrimResults(Dictionary<string, List<Item>> dict, string searchedItem, int trimTo)
        {
            Dictionary<Tuple<string, Item>, int> differences = [];
            foreach (string key in dict.Keys)
            {
                foreach (Item item in dict[key])
                {
                    int difference = Util.GetDifference(item.Name.ToLower(), searchedItem.ToLower());
                    differences[new Tuple<string, Item>(key, item)] = difference;
                }
            }
            var sorted = from entry in differences orderby entry.Value ascending select entry;
            var trimmed = sorted.Take(trimTo).ToDictionary();
            Dictionary<string, List<Item>> result = [];
            foreach (Tuple<string, Item> key in trimmed.Keys)
            {
                if (result.TryGetValue(key.Item1, out List<Item> value))
                {
                    value.Add(key.Item2);
                }
                else
                {
                    result[key.Item1] = [key.Item2];
                }
            }
            return result;
        }
        public static Dictionary<string, List<Item>> SearchItem(string item)
        {
            Dictionary<string, List<Item>> resultItems = [];
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
            Console.WriteLine(currentItemNames.Count);
            List<string> matchingItemNames = Util.FuzzySearch(item, currentItemNames.ToArray(), 3);
            matchingItemNames.ForEach(e => Console.WriteLine(e));
            foreach (string key in currentItems.Keys)
            {
                foreach (var i in currentItems[key])
                {
                    if (matchingItemNames.Contains(i.Name))
                    {
                        if (resultItems.TryGetValue(key, out var result))
                        {
                            result.Add(i);
                        }
                        else
                        {
                            resultItems.Add(key, [i]);
                        }


                    }


                }
            }
            return resultItems;
        }


    }


}

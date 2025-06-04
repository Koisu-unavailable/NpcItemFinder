using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace NpcItemFinder
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.

	public class NpcItemFinder : Mod
	{
		static public Dictionary<string, NPCShop> shops = [];
		public override void Load()
		{
			Console.WriteLine($"Detected town NPC shops: {shops}");
        }
	}
}

using System.ComponentModel;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace NpcItemFinder;


class NpcItemFinderConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;

    [Header("findItemHeader")]
    [DefaultValue(10)]
    public int ItemDisplayLimit;
}
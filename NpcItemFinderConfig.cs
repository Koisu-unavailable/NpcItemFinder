using System.ComponentModel;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace NpcItemFinder;


class NpcItemFinderConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;

    [Header("/findItem Command")]
    [DefaultValue(10)]
    [Description("The limit on how many items to display.")]
    public int ItemDisplayLimit;
}
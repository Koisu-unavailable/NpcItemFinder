using Terraria;
using Terraria.ModLoader;

namespace NpcItemFinder.Keybinds;

class OpenFindUiKeybind : ModSystem
{
    public static ModKeybind OpenFindUi { get; private set; }

    public override void Load()
    {
        OpenFindUi = KeybindLoader.RegisterKeybind(Mod, "OpenFindUi", "G");
    }

    public override void Unload()
    {
        OpenFindUi = null;
    }
}

using Terraria;
using Terraria.ModLoader;

namespace NpcItemFinder.Keybinds;

class OpenSearchKeybind : ModSystem
{
    public static ModKeybind OpenCommand { get; private set; }
    public override void Load()
    {
        OpenCommand = KeybindLoader.RegisterKeybind(Mod, "OpenCommand", "");
    }
    public override void Unload()
    {
        OpenCommand = null;
    }
}
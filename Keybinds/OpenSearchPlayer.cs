using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;


namespace NpcItemFinder.Keybinds;


class OpenSearchPlayer : ModPlayer
{
    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (OpenSearchKeybind.OpenCommand.JustPressed)
        {
            Main.OpenPlayerChat();
            Main.chatText = "/findItem ";
        }
    }
}

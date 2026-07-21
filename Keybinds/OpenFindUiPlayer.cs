using NpcItemFinder.UI;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;


namespace NpcItemFinder.Keybinds;


class OpenFindUiPlayer : ModPlayer
{
    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (OpenFindUiKeybind.OpenFindUi.JustPressed)
        {
            // Open the FindItemPanel UI when the keybind is pressed
            ModContent.GetInstance<FindItemUiSystem>().ToggleVisible();
        }
    }
    
}

using NpcItemFinder.UI;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace NpcItemFinder.UI;
public class Blinker(string oldText) : UIElement
{
    private UIText blinker;
    private string oldText = oldText;

    public override void OnInitialize()
    {
        base.OnInitialize();
        blinker = new UIText("|", 1f); // different fonts could mess this up
        Append(blinker);
    }

    public void SetInvis(bool invis)
    {
        if (invis)
        {
            blinker.SetText("", 1f, false);
        }
        else
        {
            blinker.SetText("|", 1f, false);
        }
    }

    public void OnTextChange(string newText)
    {
        var difference =
            FontAssets.MouseText.Value.MeasureString(newText).X
            - FontAssets.MouseText.Value.MeasureString(oldText).X;
        blinker.Left.Set(blinker.Left.Pixels + difference, 0); // the 1 is for extra spacing for wide characters, probably will fix that
        oldText = newText;
        Recalculate();
    }
}

using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace NpcItemFinder.UI
{
    public class FindItemState : UIState
    {
        public FindItemPanel findItemPanel;

        public override void OnInitialize()
        {
            findItemPanel = new FindItemPanel();

            Append(findItemPanel);
            findItemPanel.SetPadding(0);
            findItemPanel.Width.Set(500f, 0);
            findItemPanel.Height.Set(200f, 0);
            findItemPanel.HAlign = 0.5f;
            findItemPanel.VAlign = 0.3f;
        }
    }
}

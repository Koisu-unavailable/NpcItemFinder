using Terraria;
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
            SetRectangle(findItemPanel, left: 400f, top: 100f, width: 170f, height: 70f);
        }
        private void SetRectangle(UIElement uiElement, float left, float top, float width, float height)
        {
            uiElement.Left.Set(left, 0f);
            uiElement.Top.Set(top, 0f);
            uiElement.Width.Set(width, 0f);
            uiElement.Height.Set(height, 0f);
        }


    }
}
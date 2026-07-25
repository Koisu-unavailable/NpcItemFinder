using System;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace NpcItemFinder.UI;

public class PageSelectorUIElement : UIElement
{
    private int currentPage;
    private int maxPages;
    private UIText pageText;
    private UIButton nextButton;
    private UIButton previousButton;

    public PageSelectorUIElement(int currentPage, int maxPages)
    {
        this.currentPage = currentPage;
        this.maxPages = maxPages;
    }
    override public void OnInitialize()
    {
        base.OnInitialize();
        
        pageText = new UIText($"Page {currentPage + 1} of {maxPages}")
        {
            HAlign = 0.5f,
            VAlign = 0.5f,
        };
        nextButton = new UIButton("Next", (evt) =>
        {
            if (currentPage < maxPages - 1)
            {
                currentPage++;
                UpdatePage(currentPage);
            }
        });
        previousButton = new UIButton("Previous", (evt) =>
        {
            if (currentPage > 0)
            {
                currentPage--;
                UpdatePage(currentPage);
            }
        });
        Height.Set(nextButton.Height.Pixels, 0);

        previousButton.HAlign = 0f;
        previousButton.Left.Set(0f, 0f);
        previousButton.VAlign = 0.5f;
        pageText.Left.Set(0f, 0f);
        nextButton.HAlign = 1f;
        nextButton.Left.Set(0f, 0f);
        nextButton.VAlign = 0.5f;

        Append(pageText);
        Append(nextButton);
        Append(previousButton);

        UpdateLayout();
    }

    private void UpdateLayout()
    {
        float textWidth = FontAssets.MouseText.Value.MeasureString(pageText.Text).X;
        Width.Set(
            previousButton.Width.Pixels + nextButton.Width.Pixels + textWidth + 16f,
            0f
        );
        Recalculate();
    }

    public event Action<int, int> OnPageChanged;
    public void UpdatePage(int newCurrentPage, int newMaxPages)
    {
        currentPage = newCurrentPage;
        maxPages = newMaxPages;
        pageText.SetText($"Page {currentPage + 1} of {maxPages}");
        UpdateLayout();
        OnPageChanged?.Invoke(currentPage, maxPages);
    }
    public void UpdatePage(int newCurrentPage)
    {
        UpdatePage(newCurrentPage, maxPages);
    }
}
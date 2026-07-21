using System;
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
        
        pageText = new UIText($"Page {currentPage + 1} of {maxPages}");
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
        // Positioning the buttons and text
        previousButton.Left.Set(0, 1 / 3);
        pageText.Left.Set(0, 2 / 3);
        nextButton.Left.Set(0, 1);
        Append(pageText);
        Append(nextButton);
        Append(previousButton);

        Recalculate();
    }

    public event Action<int, int> OnPageChanged;
    public void UpdatePage(int newCurrentPage, int newMaxPages)
    {
        currentPage = newCurrentPage;
        maxPages = newMaxPages;
        pageText.SetText($"Page {currentPage + 1} of {maxPages}");
        OnPageChanged?.Invoke(currentPage, maxPages);
    }
    public void UpdatePage(int newCurrentPage)
    {
        UpdatePage(newCurrentPage, maxPages);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace NpcItemFinder.UI;

public class FindItemPanel : UIPanel
{
    private Vector2 offset;
    private bool dragging;
    private SearchBar searchBar;
    private UIText placeholderItemText;
    private const int PLACEHOLDERITEMTEXTSCALE = 2;
    private UIButton searchButton;
    private int page = 0;
    private int maxPages = 1;

    private List<int> currentlyDisplaying = [];

    // the amount of items that can be displayed per page
    private int displayAmount;
    private const int ITEMCONTAINERXPAD = 10; // eyeballed

    public override void OnInitialize()
    {
        base.OnInitialize();
        searchBar = new SearchBar();
        searchBar.Width.Set(Width.Pixels - SearchBar.xPad * 2, 0); // times 2 accounts for left/right pad
        searchBar.Left.Set(SearchBar.xPad, 0);
        var textHeight = FontAssets.MouseText.Value.MeasureString(searchBar.hint).Y; // looked into source code for want the default font was
        searchBar.Height.Set(textHeight + SearchBar.hintYpad * 2, 0); // times two accounts for bottom and top padding
        searchBar.MarginBottom = SearchBar.yPad;
        searchBar.Top.Set(SearchBar.yPad, 0);
        Append(searchBar);
        placeholderItemText = new UIText("Items will appear here", PLACEHOLDERITEMTEXTSCALE)
        {
            HAlign = 0.5f,
            VAlign = 0.6f,
            TextColor = Color.Gray
        };
        Append(placeholderItemText);
        searchButton = new UIButton("Search", Search);
        searchBar.Width.Set(
            searchBar.Width.Pixels - searchButton.Width.Pixels - SearchBar.xPad,
            searchBar.Width.Percent
        );
        searchButton.Top.Set(searchBar.Top.Pixels, searchBar.Top.Percent);
        searchButton.Left.Set(
            searchBar.Left.Pixels + searchBar.Width.Pixels + SearchBar.xPad,
            searchBar.Left.Percent
        );
        searchBar.MarginRight = SearchBar.xPad;
        Append(searchButton);
        displayAmount = (int)(
            Width.Pixels / (ItemContainer.WIDTH + ITEMCONTAINERXPAD * 2)
        );

        for (int i = 0; i < displayAmount; i++)
        {
            var container = new ItemContainer(new Item())
            {
                VAlign = 0.6f,
            };
            container.MarginLeft = container.MarginRight = ITEMCONTAINERXPAD;
            Append(container);
            currentlyDisplaying.Add(container.UniqueId);
        }
        Recalculate();
    }


    private void Search(UIMouseEvent evt)
    {
        string query = searchBar.GetText();
        RenderQuery(FindItem.SearchItem(query));
    }

    private void RenderQuery(Dictionary<string, List<Item>> queryResults)
    {
        List<Item> items = queryResults.Flatten();
        var containers = Children.OfType<ItemContainer>();
        containers.ToList().ForEach(c =>
        {
            c.Item = new Item();
            c.Left.Set(Array.IndexOf(containers.ToArray(), c) * (Width.Pixels / displayAmount), 0);
            c.MarginLeft = c.MarginRight = ITEMCONTAINERXPAD;
        }
        );

        if (items.Count == 0)
        {
            placeholderItemText.SetText("No items found");
            return;
        }
        else
        {
            placeholderItemText.SetText("");
        }
        for (int i = 0; i < displayAmount; i++)
        {
            int itemIndex = (page * displayAmount) + i;
            if (itemIndex >= items.Count)
            {
                // should attempt to reposition containers to be centered 
                break;
            }
            containers.ElementAt(i).Item = items[itemIndex];
        }
        // recenter the containers if there are less than displayAmount items
        var enumerated_containers = containers.GetEnumerator();
        if (displayAmount > items.Count)
        {
            int spaces = (int)Math.Round(Math.Pow((1 / ((float)items.Count)) / 2, -1), 1);
            int i = 0;
            while (i <= spaces)
            {
                if (i % 2 != 0)
                {
                    enumerated_containers.MoveNext();
                    // Main.NewText($"enumerated_containers.MoveNext(): {enumerated_containers.Current.Item.Name} is being drawn at position {(float)(i / (float)spaces)}");
                    enumerated_containers.Current.Left.Set(-enumerated_containers.Current.Width.Pixels / 2, (float)(i / (float)spaces));
                    enumerated_containers.Current.MarginLeft = enumerated_containers.Current.MarginRight = 0;
                    i++;
                }
                else
                {
                    i++;
                    continue;
                }
            }
        }
        Recalculate();
    }

    public override void LeftMouseDown(UIMouseEvent evt)
    {
        // When you override UIElement methods, don't forget call the base method
        // This helps to keep the basic behavior of the UIElement
        base.LeftMouseDown(evt);
        // When the mouse button is down on this element, then we start dragging
        if (evt.Target == this)
        {
            DragStart(evt);
        }
    }

    public override void LeftMouseUp(UIMouseEvent evt)
    {
        base.LeftMouseUp(evt);
        // When the mouse button is up, then we stop dragging
        if (evt.Target == this)
        {
            DragEnd(evt);
        }
    }

    private void DragStart(UIMouseEvent evt)
    {
        // The offset variable helps to remember the position of the panel relative to the mouse position
        // So no matter where you start dragging the panel, it will move smoothly
        offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
        dragging = true;
    }

    private void DragEnd(UIMouseEvent evt)
    {
        Vector2 endMousePosition = evt.MousePosition;
        dragging = false;

        Left.Set(endMousePosition.X - offset.X, 0f);
        Top.Set(endMousePosition.Y - offset.Y, 0f);

        Recalculate();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        // Checking ContainsPoint and then setting mouseInterface to true is very common
        // This causes clicks on this UIElement to not cause the player to use current items
        if (ContainsPoint(Main.MouseScreen))
        {
            Main.LocalPlayer.mouseInterface = true;
        }

        if (dragging)
        {
            Left.Set(Main.mouseX - offset.X, 0f); // Main.MouseScreen.X and Main.mouseX are the same
            Top.Set(Main.mouseY - offset.Y, 0f);

            Recalculate();
        }

        // Here we check if the DraggableUIPanel is outside the Parent UIElement rectangle
        // (In our example, the parent would be ExampleCoinsUI, a UIState. This means that we are checking that the DraggableUIPanel is outside the whole screen)
        // By doing this and some simple math, we can snap the panel back on screen if the user resizes his window or otherwise changes resolution
        var parentSpace = Parent.GetDimensions().ToRectangle();
        if (!GetDimensions().ToRectangle().Intersects(parentSpace))
        {
            Left.Pixels = Utils.Clamp(Left.Pixels, 0, parentSpace.Right - Width.Pixels);
            Top.Pixels = Utils.Clamp(Top.Pixels, 0, parentSpace.Bottom - Height.Pixels);
            // Recalculate forces the UI system to do the positioning math again.
            Recalculate();
        }
        Recalculate();
    }
}

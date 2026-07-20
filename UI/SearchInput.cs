using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;

namespace NpcItemFinder.UI;

public class SearchBar : UIPanel
{
    private bool focused = false;
    public readonly string hint = "PLACEHOLDER"; // TODO: add localization

    private UIText textElement;
    private Blinker blinker;
    public const int xPad = 40;
    public const int yPad = 20;
    public const int hintYpad = 4;
    public event Action<string> OnTextUpdate;

    public SearchBar()
    {
        BorderColor = Color.Blue;
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        textElement = new UIText(hint)
        {
            TextColor = Color.Gray,
            HAlign = 0.05f,
            VAlign = 0.5f
        };
        Append(textElement);
        blinker = new Blinker("");
        OnTextUpdate += blinker.OnTextChange;
        Append(blinker);
        OnLeftClick += OnClick;
    }

    private void OnClick(UIMouseEvent evt, UIElement listeningElement)
    {
        Focus();
    }

    private void Focus()
    {
        focused = true;
        textElement.SetText("");
        textElement.TextColor = Color.White;
    }

    private void Unfocus(bool clear)
    {
        focused = false;
        if (clear)
        {
            textElement.SetText(hint);
            textElement.TextColor = Color.Gray;
        }
    }
    public string GetText()
    {
        return textElement.Text;
    }
    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);
        // idk why this cant go in update, ask the recipe browser dev
        if (focused)
        {
            PlayerInput.WritingText = true;
            Main.instance.HandleIME();
            string newString = Main.GetInputText(textElement.Text);
            if (!(newString == textElement.Text)) // check if they actually changed it so it doesn't flash
            {
                textElement.SetText(newString);
                OnTextUpdate?.Invoke(newString);
            }
        }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        Vector2 MousePosition = new Vector2((float)Main.mouseX, (float)Main.mouseY);
        // stole this from recipe browser
        if (!ContainsPoint(MousePosition) && (Main.mouseLeft || Main.mouseRight)) // This solution is fine, but we need a way to cleanly "unload" a UIElement
        {
            Unfocus(textElement.Text == "");
        }
    }
}

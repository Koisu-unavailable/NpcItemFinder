using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;


// generated with calude mostly because 
namespace NpcItemFinder.UI
{
	/// <summary>
	/// A general-purpose button that can show a texture, text, or both.
	/// The click callback is passed straight into the constructor rather
	/// than being wired up afterward with OnLeftClick +=.
	/// </summary>
	public class UIButton : UIPanel
	{
		private readonly Asset<Texture2D> _texture;
		private readonly string _text;
		private readonly Color _normalBackgroundColor;

		public Color HoverBackgroundColor = new Color(73, 94, 171);
		public Color TextColor = Color.White;
		public float TextScale = 1f;

		/// <summary>Text-only button.</summary>
		public UIButton(string text, Action<UIMouseEvent> onClick = null)
			: this(null, text, onClick) { }

		/// <summary>Icon-only button. Pass an already-requested texture Asset.</summary>
		public UIButton(Asset<Texture2D> texture, Action<UIMouseEvent> onClick = null)
			: this(texture, null, onClick) { }

		/// <summary>Icon + text button.</summary>
		public UIButton(Asset<Texture2D> texture, string text, Action<UIMouseEvent> onClick = null)
		{
			_texture = texture;
			_text = text;

			SetPadding(8f);
			_normalBackgroundColor = BackgroundColor;

			// Sensible defaults - override with Width/Height.Set(...) after construction
			// if you need a specific size (e.g. to match a particular icon).
			Width.Set(texture != null && string.IsNullOrEmpty(text) ? 40f : 140f, 0f);
			Height.Set(40f, 0f);

			if (onClick != null)
				OnLeftClick += (evt, listeningElement) => onClick(evt);
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			BackgroundColor = HoverBackgroundColor;
			SoundEngine.PlaySound(SoundID.MenuTick);
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			BackgroundColor = _normalBackgroundColor;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch); // panel background + border

			CalculatedStyle dimensions = GetInnerDimensions();
			Vector2 center = dimensions.Center();
			float iconRightEdge = dimensions.X;

			if (_texture != null)
			{
				Vector2 textureSize = _texture.Value.Size();

				// Icon-only: fit within both dimensions. Icon+text: match the button's height
				// and let the label flow to the right of it.
				float scale = string.IsNullOrEmpty(_text)
					? Math.Min(dimensions.Width / textureSize.X, dimensions.Height / textureSize.Y)
					: dimensions.Height / textureSize.Y;
				scale = Math.Min(scale, 1f); // don't upscale small icons

				Vector2 iconPos = string.IsNullOrEmpty(_text)
					? center
					: new Vector2(dimensions.X + textureSize.X * scale / 2f, center.Y);

				spriteBatch.Draw(_texture.Value, iconPos, null, Color.White, 0f,
					textureSize / 2f, scale, SpriteEffects.None, 0f);

				iconRightEdge = iconPos.X + textureSize.X * scale / 2f;
			}

			if (!string.IsNullOrEmpty(_text))
			{
				if (_texture != null)
					Utils.DrawBorderString(spriteBatch, _text, new Vector2(iconRightEdge + 8f, center.Y),
						TextColor, TextScale, 0f, 0.5f, -1);
				else
					Utils.DrawBorderString(spriteBatch, _text, center, TextColor, TextScale, 0.5f, 0.5f, -1);
			}
		}
	}
}

/*
Usage examples:

// Text-only
var openButton = new UIButton("Open Bag", evt => Main.NewText("Opened!"));
openButton.Left.Set(50f, 0f);
openButton.Top.Set(50f, 0f);
container.Append(openButton);

// Icon-only (request the texture once, e.g. in your UIState's OnInitialize)
Asset<Texture2D> closeIcon = ModContent.Request<Texture2D>("YourMod/UI/CloseIcon", AssetRequestMode.ImmediateLoad);
var closeButton = new UIButton(closeIcon, evt => uiState.Close());

// Icon + text
var comboButton = new UIButton(closeIcon, "Close", evt => uiState.Close());
*/
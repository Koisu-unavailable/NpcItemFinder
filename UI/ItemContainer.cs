using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.UI;

namespace NpcItemFinder.UI
{
    public class ItemContainer : UIPanel
    {
        private Item _item;
        public Item Item
        {
            get => _item;
            set
            {
                _item = value;
                if (_item != null)
                {
                    Main.instance.LoadItem(_item.type);
                }
            }
        }

        public const int WIDTH = 50;
        public const int HEIGHT = 50;

        public ItemContainer(Item item)
        {
            Item = item;
        }

        public override void OnInitialize()
        {
            base.OnInitialize();
            BorderColor = Color.Blue;
            Width.Set(WIDTH, 0);
            Height.Set(HEIGHT, 0);
            Recalculate();
            Main.instance.LoadItem(_item.type);
        }
#nullable enable
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (_item == null || _item.type == ItemID.None) return;
            CalculatedStyle dimensions = GetDimensions();
            base.DrawSelf(spriteBatch);
            ModItem? modItem = _item.ModItem;

            Texture2D texture = TextureAssets.Item[_item.type].Value;

            // Handles animated items
            Rectangle frame =
                Main.itemAnimations[_item.type] != null
                    ? Main.itemAnimations[_item.type].GetFrame(texture)
                    : texture.Frame();

            // Scale so the item fits within the panel
            float scale = Math.Min(
                Width.Pixels / ((float)frame.Width + 10),
                Height.Pixels / ((float)frame.Height + 10)
            // +10 for padding
            );

            Vector2 drawPos = dimensions.Center();
            // Pass the item's source frame to the hook and the correct origin (frame center).
            bool drawSprite = ItemLoader.PreDrawInInventory(
                _item,
                spriteBatch,
                drawPos,
                frame,
                Color.White,
                Color.White,
                frame.Size() / 2f,
                scale
            );
            if (drawSprite || modItem == null)
            {
                spriteBatch.Draw(
                    texture,
                    drawPos,
                    frame,
                    Color.White,
                    0f,
                    frame.Size() / 2f,
                    scale,
                    SpriteEffects.None,
                    0f
                );
            }
            ItemLoader.PostDrawInInventory(_item, spriteBatch, drawPos, frame, Color.White, Color.White, frame.Size() / 2, scale);

            if (ContainsPoint(Main.MouseScreen))
            {
                Main.HoverItem = _item.Clone();
                Main.hoverItemName = _item.Name;
            }
        }
    }
}

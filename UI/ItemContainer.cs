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
    public class ItemContainer(Item item) : UIPanel
    {
        private Item item = item;
        public override void OnInitialize()
        {
            base.OnInitialize();
            BorderColor = Color.Blue;
            Width.Set(50, 0);
            Height.Set(50, 0);
            Recalculate();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            ModItem? modItem = item.ModItem;
            Main.instance.LoadItem(item.type);

            Texture2D texture = TextureAssets.Item[item.type].Value;

            // Handles animated items
            Rectangle frame =
                Main.itemAnimations[item.type] != null
                    ? Main.itemAnimations[item.type].GetFrame(texture)
                    : texture.Frame();

            // Scale so the item fits within the panel
            float scale = Math.Min(
                Width.Pixels / ((float)frame.Width + 10),
                Height.Pixels / ((float)frame.Height + 10)
            // +10 for padding
            );

            Vector2 drawPos =
                dimensions.Position() + new Vector2(Width.Pixels / 2f, Height.Pixels / 2f);
            bool drawSprite = true;
            if (!(modItem == null))
            {
                drawSprite = ItemLoader.PreDrawInInventory(
                    modItem.Item,
                    spriteBatch,
                    drawPos,
                    dimensions.ToRectangle(),
                    Color.White,
                    Color.White,
                    dimensions.Center(),
                    scale
                );
            }
            if (drawSprite)
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
            if (ContainsPoint(Main.MouseScreen))
            {
                Main.HoverItem = item.Clone();
                Main.hoverItemName = item.Name;
            }
        }
    }
}

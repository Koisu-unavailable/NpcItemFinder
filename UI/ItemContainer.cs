using System;
using System.Collections.Generic;
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
        private readonly Item item = item;

        Asset<Texture2D> texture;
        private static Item[] allAvailbleItems; // static to save memory
        private bool animated;
        private DrawAnimation? drawAnimation;

        public override void OnInitialize()
        {
            base.OnInitialize();
            // // does this once and shares across all instances
            // if (allAvailbleItems.Length == 0)
            // {
            //     foreach (string key in NpcItemFinder.shops.Keys)
            //     {
            //         foreach (NPCShop.Entry entry in NpcItemFinder.shops[key].ActiveEntries)
            //         {
            //             allAvailbleItems = allAvailbleItems.Append(entry.Item).ToArray();
            //         }
            //     }
            // }
            BorderColor = Color.Blue;
            ModItem? modItem = item.ModItem;
            if (modItem == null) // vanilla
            {
                Main.instance.LoadItem(item.type);
                texture = TextureAssets.Item[item.type];
            }
            else
            {
                texture = ModContent.Request<Texture2D>(modItem.Texture);
            }
            try
            {
                drawAnimation = Main.itemAnimations[item.type];
                animated = true;
            }
            catch (IndexOutOfRangeException)
            {
                animated = false;
            }
        }

        private void DrawNotAnimated(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            CalculatedStyle dimensions = GetDimensions();
            spriteBatch.Draw(texture.Value, dimensions.Center(), Color.White);
        }

        private void DrawAnimated(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            CalculatedStyle dimensions = GetDimensions();
            ModItem? modItem = item.ModItem;
            if (modItem == null) // pretty sure all vanilla items are vertically animated
            {
                var frame = texture.Frame(
                    1,
                    drawAnimation.FrameCount,
                    0,
                    drawAnimation.FrameCounter
                );
                drawAnimation.FrameCounter++;
                spriteBatch.Draw(texture.Value, dimensions.Center(), frame, Color.White);
            }
            else
            {
                modItem.PreDrawInInventory(
                    spriteBatch,
                    dimensions.Center(),
                    dimensions.ToRectangle(),
                    Color.White,
                    Color.White,
                    new Vector2(0),
                    1
                ); // some of them might be horizobntal, dont wnatr to deal with that
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (animated)
            {
                DrawAnimated(spriteBatch);
            }
            else
            {
                DrawNotAnimated(spriteBatch);
            }
        }
    }
}

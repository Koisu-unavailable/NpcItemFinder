# Npc Item Finder
## A mod find which NPC sells what

# Usage:
/findItem [item] -> finds npcs that sell that item

# Todos:
- Add a U.I.
- Filtering by mod
- better search for FindItem
- add calling Itremloader.PostDraw nd Predraw for all items in item containere
    Better than before, but there are still a few issues:
1. You're bypassing GlobalItem hooks entirely.
Calling modItem.PreDrawInInventory(...) directly only invokes that one item's own override. It skips any GlobalItem.PreDrawInInventory hooks other mods might have added (which could apply to any item, vanilla or modded — e.g. a mod that adds a glow to all weapons). The proper way to fire both is through the loader:
csharpbool drawSprite = ItemLoader.PreDrawInInventory(
    item, spriteBatch, drawPos, frame, Color.White, Color.White, frame.Size() / 2f, scale
);
ItemLoader.PreDrawInInventory internally calls all relevant GlobalItems' hooks and the item's own ModItem hook (if any), combining the results — so it works correctly for vanilla items too (no modItem == null check needed at all).
2. The frame and origin you're passing to the hook don't match what you're drawing.
You're calling the hook with dimensions.ToRectangle() as the frame and dimensions.Center() as the origin — but you're drawing with frame (the actual animation/sprite-sheet frame) and frame.Size() / 2f as the origin. If a mod's PreDrawInInventory uses those parameters to draw something relative to the item's actual sprite (e.g. an overlay aligned to the icon), it'll be misaligned since it's working off UI-panel dimensions instead of the real texture frame. Pass the same frame and origin you use in the actual spriteBatch.Draw call.
3. You're missing PostDrawInInventory entirely.
This hook is called even if PreDrawInInventory returns false, so it needs to run unconditionally after your draw block, not be skipped just because drawSprite was false. GitHub
Here's the fixed version:
csharpprotected override void DrawSelf(SpriteBatch spriteBatch)
{
    CalculatedStyle dimensions = GetDimensions();
    Main.instance.LoadItem(item.type);

    Texture2D texture = TextureAssets.Item[item.type].Value;

    Rectangle frame =
        Main.itemAnimations[item.type] != null
            ? Main.itemAnimations[item.type].GetFrame(texture)
            : texture.Frame();

    float scale = Math.Min(
        Width.Pixels / ((float)frame.Width + 10),
        Height.Pixels / ((float)frame.Height + 10)
    );

    Vector2 drawPos = dimensions.Position() + new Vector2(Width.Pixels / 2f, Height.Pixels / 2f);
    Vector2 origin = frame.Size() / 2f;
    Color color = Color.White;

    bool drawSprite = ItemLoader.PreDrawInInventory(
        item, spriteBatch, drawPos, frame, color, color, origin, scale
    );

    if (drawSprite)
    {
        spriteBatch.Draw(texture, drawPos, frame, color, 0f, origin, scale, SpriteEffects.None, 0f);

        if (ContainsPoint(Main.MouseScreen))
        {
            Main.HoverItem = item.Clone();
            Main.hoverItemName = item.Name;
        }
    }

    ItemLoader.PostDrawInInventory(item, spriteBatch, drawPos, frame, color, color, origin, scale);
}
This removes the modItem null-check dance entirely, fires both GlobalItem and ModItem hooks correctly for any item, and keeps the parameters consistent between what's passed to the hooks and what's actually drawn.


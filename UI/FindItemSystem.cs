using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace NpcItemFinder.UI
{
    [Autoload(Side = ModSide.Client)]
    public class FindItemUiSystem : ModSystem
    {
        public static ModKeybind OpenFindUi { get; private set; }

        internal FindItemState findItemState;
        private UserInterface _findItemState;

        public override void Load()
        {
            findItemState = new FindItemState();
            findItemState.Activate();
            _findItemState = new UserInterface();
            _findItemState.SetState(findItemState);
            OpenFindUi = KeybindLoader.RegisterKeybind(Mod, "OpenFindUi", "G");
        }

        public override void UpdateUI(GameTime gameTime)
        {
            _findItemState?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer =>
                layer.Name.Equals("Vanilla: Mouse Text")
            );
            if (mouseTextIndex != -1)
            {
                layers.Insert(
                    mouseTextIndex,
                    new LegacyGameInterfaceLayer(
                        "NpcItemFinder: A mod for finding what npc sells what",
                        delegate
                        {
                            _findItemState.Draw(Main.spriteBatch, new GameTime());
                            return true;
                        },
                        InterfaceScaleType.UI
                    )
                );
            }
        }

        public override void Unload()
        {
            OpenFindUi = null;
        }
    }
}

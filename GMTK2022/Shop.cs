using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace GMTK2022
{
    public class Shop
    {
        public List<Slot> Slots;
        List<SpriteText> Text;

        public Shop()
        {
            Slots = new List<Slot>();
            Text = new List<SpriteText>();

            InitializeShop();

            Sprite.Add(new BoxButton(0, "Reroll ($10)", 545, 50, 100, 30, Color.LightBlue, Color.Blue));
        }

        private void InitializeShop()
        {
            for (int i = 0; i < 3; i++)
            {
                Slots.Add(new Slot(new Vector2(50 + i * 175, 0)));
                Text.Add(new SpriteText("", new Vector2(50 + i * 175, 80), Color.WhiteSmoke));

                SpriteText x = new SpriteText("$" + (10 + 5 * i), new Vector2(126 + i * 175, 20), Color.LightGreen);
                x.Scale = new Vector2(2, 2);
                Sprite.Add(x);

                Domino d = Domino.RandomDomino(new Vector2(0, 0));
                Slots[i].Domino = d;
                d.Slot = Slots[i];
                Sprite.Add(d);
            }

            Game1._spritesToAdd.AddRange(Slots);
            Game1._spritesToAdd.AddRange(Text);
        }

        public void RerollShop(bool clicked)
        {
            if (clicked)
            {
                if (Game1._money >= 10)
                {
                    Game1._money -= 10;

                    foreach (Slot slot in Slots)
                    {
                        if (slot.Domino != null)
                            slot.Domino.Remove();
                    }

                    Game1._spritesToRemove.AddRange(Slots);
                    Game1._spritesToRemove.AddRange(Text);

                    InitializeShop();
                }
            }
            else
            {
                foreach (Slot slot in Slots)
                {
                    if (slot.Domino != null)
                        slot.Domino.Remove();
                }

                Game1._spritesToRemove.AddRange(Slots);
                Game1._spritesToRemove.AddRange(Text);

                InitializeShop();
            }
        }

        public void Update(GameTime gt)
        {
            for (int i = 0; i < 3; i++)
            {
                if (Slots[i].Domino != null)
                {
                    Text[i].Text = Slots[i].Domino.Name.ToUpper();
                    Text[i].Position = new Vector2(50 + 32 + i * 175 - (Game1._font.MeasureString(Text[i].Text).X / 2), 75);
                }
                else
                {
                    Text[i].Text = "";
                }
            }
        }
    }
}

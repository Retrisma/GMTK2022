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
    public class Slot : Sprite
    {
        static List<Texture2D> Textures = new List<Texture2D>
        {
            Game1._spriteContent["slot0"],
            Game1._spriteContent["slot1"],
            Game1._spriteContent["slot2"],
            Game1._spriteContent["slot3"],
            Game1._spriteContent["slot4"],
            Game1._spriteContent["slot5"],
            Game1._spriteContent["slot6"],
        };

        int Value { get; set; }
        public PetriDish Dish { get; set; }
        public Domino Domino = null;
        public Rectangle Bound;

        public Slot(Vector2 position)
        {
            Init();
            Value = 0;
            Texture = Textures[0];
            Width = Texture.Width;
            Height = Texture.Height;

            Position = position + new Vector2(0, 64);
            Rotation = -(float)Math.PI / 2;

            Bound = new Rectangle((int)Position.X, (int)Position.Y - 64, (int)Width, (int)Height);

            LayerDepth = 0.12f;
        }

        public Slot(int value, bool leftOrRight, PetriDish dish)
        {
            Init();
            Value = value;
            Texture = Textures[Value];
            Dish = dish;
            Width = Texture.Width;
            Height = Texture.Height;

            int x = 0;
            int y = value * 75 + 32;

            if (value == 1)
                y += 32;

            if (!leftOrRight)
                x = 996;
            else
                SpriteEffects = SpriteEffects.FlipHorizontally;

            Position = new Vector2(x, y);

            if (value >= 2 && value <= 5)
                Bound = new Rectangle((int)Position.X, (int)Position.Y, (int)Width, (int)Height);

            LayerDepth = 0.12f;
        }

        public override void Update(GameTime gt)
        {
            if (this.Domino == null)
            {
                foreach (Sprite sprite in Game1._sprites)
                {
                    if (sprite.GetType() == typeof(Domino))
                    {
                        Domino x = (Domino)sprite;

                        if ((this.Value >= 2 && this.Value <= 5) || this.Value == 0)
                            if (x.WasHeldLastFrame && Bound.Contains(Game1._mouseState.Position) && x.Slot == null)
                            {
                                x.Slot = this;
                                this.Domino = x;
                            }
                    }
                }
            }
            else
            {
                if (Domino.Bounds.Contains(Game1._mouseState.Position) && Game1._mouseState.LeftButton == ButtonState.Pressed && Game1._previousMouseState.LeftButton == ButtonState.Released)
                {
                    Domino.Slot = null;
                    Domino.Clickable = true;
                    Domino.Holding = true;
                    this.Domino = null;
                }
            }
        }
    }
}

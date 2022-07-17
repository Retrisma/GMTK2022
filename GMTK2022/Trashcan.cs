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
    public class Trashcan : Sprite
    {
        Rectangle Bounds;

        public Trashcan()
        {
            Init();
            Texture = Game1._spriteContent["TrashcanClosed"];
            Position = new Vector2(980, 20);
            Bounds = new Rectangle((int)Position.X, (int)Position.Y, 64, 64);
            LayerDepth = 0.2f;
            Sprite.Add(this);
        }

        public override void Update(GameTime gt)
        {
            bool CanOpen = false;

            foreach (Sprite sprite in Game1._sprites)
            {
                if (sprite.GetType() == typeof(Domino))
                {
                    Domino x = (Domino)sprite;

                    if (x.Bounds.Intersects(Bounds))
                    {
                        if (x.Holding)
                        {
                            CanOpen = true;
                            x.Color = new Color(Color.Red, 0.5f);
                        }
                        else
                        {
                            x.UiRect.Remove();
                            x.Remove();
                        }
                    }
                    else
                    {
                        x.Color = Color.White;
                    }
                }
            }

            if (CanOpen)
                this.Texture = Texture = Game1._spriteContent["TrashcanOpen"];
            else
                this.Texture = Texture = Game1._spriteContent["TrashcanClosed"];
        }
    }
}

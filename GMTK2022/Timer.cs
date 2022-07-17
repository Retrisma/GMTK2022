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
    public class Timer : UiRect
    {
        double Seconds { get; set; }

        public Timer(int seconds, int pos_x, int pos_y, int width, int height, Color _fill_color, Color _border_color) 
            : base(pos_x, pos_y, width, height, _fill_color, _border_color)
        {
            Seconds = seconds;
            LayerDepth = 0.8f;
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);

            if (Seconds > 0)
            {
                double elapsed = gt.ElapsedGameTime.TotalSeconds;
                Seconds -= elapsed;
            }
            else if ((int)Seconds == 0)
            {
                foreach (Sprite sprite in Game1._sprites)
                {
                    if (sprite.GetType() == typeof(Creature))
                    {
                        Creature x = (Creature)sprite;
                        x.Kill();
                    }
                    else if (sprite.GetType() == typeof(FreezerSlot))
                    {
                        FreezerSlot x = (FreezerSlot)sprite;
                        x.Creature.Sell();
                    }
                    else if (sprite.GetType() == typeof(Slot))
                    {
                        Slot x = (Slot)sprite;
                        x.Domino = null;
                    }
                    else if (sprite.GetType() == typeof(Domino))
                    {
                        Domino x = (Domino)sprite;
                        x.Remove();
                    }
                }

                Game1._shop.RerollShop(false);

                Seconds = 2 * 60;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            string output = "";
            int time = Math.Max((int)Seconds, 0);

            output += time / 60;
            output += ":";
            if ((time % 60) - 10 < 0) output += "0";
            output += time % 60;

            SpriteText x = new SpriteText(output, Position, border_color);
            x.LayerDepth = 0.79f;
            x.Scale = Vector2.One * 4;
            x.Position = new Vector2(Position.X + rect.Width / 2 - Game1._font.MeasureString(output).X * 2 + 1, Position.Y - 3);

            x.Draw(sb);
        }
    }
}

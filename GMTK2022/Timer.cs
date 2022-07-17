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

            double elapsed = gt.ElapsedGameTime.TotalSeconds;
            Seconds -= elapsed;
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            string output = "";
            int time = (int)Seconds;

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

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace GMTK2022
{
    class SpriteText : Sprite
    {
        public string Text { get; set; }

        public SpriteText(string text, Vector2 position, Color color)
        {
            Position = position;
            Text = text;
            Color = color;
            Rotation = 0f;
            Origin = Vector2.Zero;
            Scale = Vector2.One;
            SpriteEffects = SpriteEffects.None;
            LayerDepth = 0.5f;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.DrawString(Game1._font, Text, Position, Color, Rotation, Origin, Scale, SpriteEffects, LayerDepth);
        }
    }
}

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
    public class Sprite
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Color Color { get; set; }

        public Sprite()
        {

        }

        public Sprite(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
        }

        public void Init()
        {
            Color = Color.White;
        }

        public static void Add(Sprite sprite)
        {
            Game1._spritesToAdd.Add(sprite);
        }

        public void Remove()
        {
            Game1._spritesToRemove.Add(this);
        }

        public virtual void Update(GameTime gt)
        {

        }

        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(Texture, Position, Color);
        }
    }
}

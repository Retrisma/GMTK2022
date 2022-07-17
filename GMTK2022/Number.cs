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
    public class Number : Sprite
    {
        int Value { get; set; }
        int Age = 60;

        public Number(Vector2 position, int value)
        {
            Init();

            Texture = Game1._spriteContent["numbersheet"];
            Position = position;

            Value = value;

            Width = 16f;
            Height = 16f;
            Source = new Rectangle((Value - 1) * 16, 0, (int)Width, (int)Height);
            Origin = new Vector2(8, 8);
            //Scale = new Vector2(2, 2);
            LayerDepth = 0.6f;
        }

        public override void Update(GameTime gt)
        {
            this.Position += new Vector2(0, -0.25f);

            Age--;

            if (Age == 0)
                this.Remove();
        }
    }
}

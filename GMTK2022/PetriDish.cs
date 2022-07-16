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
    public class PetriDish : Sprite
    {
        List<Creature> creatures;

        public PetriDish(Vector2 position)
        {
            Init();

            Texture = Game1._spriteContent["petridish"];
            Position = position;

            creatures = new List<Creature>();
            Populate();
        }

        public bool AcceptablePosition(Vector2 pos)
        {
            int x = (int)pos.X;
            int y = (int)pos.Y;

            int xa = (int)(x - (Position.X + 180));
            int ya = (int)(y - (Position.Y + 180));
            int r = 180;

            return ((xa * xa) - 64) + ((ya * ya) - 64) < (r * r);
        }

        public Vector2 GenerateAcceptablePosition()
        {
            while (true)
            {
                int x = Game1._rand.Next(960);
                int y = Game1._rand.Next(540);
                Vector2 z = new Vector2(x, y);

                if (AcceptablePosition(z))
                    return z;
            }
        }

        void Populate()
        {
            while (creatures.Count < 10)
            {
                Vector2 z = GenerateAcceptablePosition();

                Creature c = new Creature(z, 6, this);
                Sprite.Add(c);
                creatures.Add(c);
            }
        }
    }
}

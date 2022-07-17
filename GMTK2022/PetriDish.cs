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
        public List<Creature> Creatures;
        public List<Slot> Slots;
        public bool LeftOrRight { get; set; }
        public int Cap = 20;

        public PetriDish(Vector2 position, bool leftOrRight)
        {
            Init();

            Texture = Game1._spriteContent["petridish"];
            Position = position;
            LayerDepth = 1f;
            Scale = new Vector2(400f / 512f);
            LeftOrRight = leftOrRight;

            Slots = new List<Slot>();
            for (int i = 1; i <= 6; i++)
            {
                Slot x = new Slot(i, leftOrRight, this);
                Sprite.Add(x);
                Slots.Add(x);
            }

            Creatures = new List<Creature>();
            Populate();
        }

        public void AddCreatureToPetriDish(Creature c)
        {
            Sprite.Add(c);
            Creatures.Add(c);
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

        public void Populate()
        {
            while (Creatures.Count < 5)
            {
                Vector2 z = GenerateAcceptablePosition();

                Creature c = new Creature(z, this);
                AddCreatureToPetriDish(c);
            }
        }

        public override void Update(GameTime gt)
        {
            if (Creatures.Count == 0)
                Populate();
        }
    }
}

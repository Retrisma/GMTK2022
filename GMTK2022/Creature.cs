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
    public class Creature : Sprite
    {
        int Size { get; set; }

        Random Rand = new Random(Guid.NewGuid().GetHashCode());

        int RollCooldown = 0;
        bool Dead = false;

        Lerper Lerper;
        bool Moving = false;

        static int UpperBound = 100;
        static int RightBound = 860;

        static int XPathing = 30;
        static int YPathing = 16;

        List<SoundEffect> DeathSoundEffects = new List<SoundEffect>
        {
            Game1._SFXContent["death1"],
            Game1._SFXContent["death2"],
            Game1._SFXContent["death3"]
        };

        public Creature(Vector2 position, int size)
        {
            Init();

            RollCooldown = Rand.Next(120);

            Texture = Game1._spriteContent["dice"];
            Position = position;
            Size = size;
        }

        public Creature(int size)
        {
            Init();

            RollCooldown = Rand.Next(20);

            Texture = Game1._spriteContent["dice"];
            Position = new Vector2(Rand.Next(RightBound), Rand.Next(540 - UpperBound) + UpperBound - 32);
            Size = size;
        }

        public void Roll()
        {
            int x = Rand.Next(Size) + 1;

            switch (x)
            {
                case 1:
                    Kill();
                    break;
                case 6:
                    Baby();
                    break;
            }

            Sprite.Add(new Number(this.Position + new Vector2(16, -10), x));
        }

        public void Baby()
        {
            Sprite.Add(new Creature(this.Size));
        }

        public void Kill()
        {
            this.Dead = true;
            this.Color = Color.Red;

            DeathSoundEffects[Rand.Next(3)].Play();
        }

        public void Path()
        {
            Moving = true;
            Vector2 Destination = new Vector2(Math.Max(Math.Min(this.Position.X + Rand.Next(2 * XPathing) - XPathing, RightBound), 0), 
                Math.Max(UpperBound, Math.Min(540, this.Position.Y + Rand.Next(2 * YPathing) - YPathing)));
            Lerper = new Lerper(this.Position, Destination);
        }

        public void Move()
        {
            this.Position = Lerper.Lerp();

            if (Lerper.Age == Lerper.EndAge)
            {
                Moving = false;
                Lerper = null;
            }
        }

        public override void Update(GameTime gt)
        {
            if (RollCooldown == 0 && !this.Dead && !this.Moving)
            {
                if (Rand.Next(2) == 0)
                    Roll();
                else
                    Path();

                RollCooldown = 120;
            }
            else if (RollCooldown == -20)
                this.Remove();
            else
                RollCooldown--;

            if (this.Moving)
                Move();
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}

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
        Random Rand = new Random(Guid.NewGuid().GetHashCode());
        string Outcome = "";

        int RollCooldown = 0;
        bool Dead = false;

        int Size { get; set; }

        List<SoundEffect> DeathSoundEffects = new List<SoundEffect>
        {
            Game1._SFXContent["death1"],
            Game1._SFXContent["death2"],
            Game1._SFXContent["death3"]
        };

        public Creature(Vector2 position, int size)
        {
            Init();

            RollCooldown = Rand.Next(20);

            Texture = Game1._spriteContent["dice"];
            Position = position;
            Size = size;
        }

        public Creature(int size)
        {
            Init();

            RollCooldown = Rand.Next(20);

            Texture = Game1._spriteContent["dice"];
            Position = new Vector2(Rand.Next(600), Rand.Next(400));
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

            Outcome = x.ToString();
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

        public override void Update(GameTime gt)
        {
            if (RollCooldown == 0 && !this.Dead)
            {
                Roll();
                RollCooldown = 60;
            }
            else if (RollCooldown == -20)
                this.Remove();
            else
                RollCooldown--;
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            //sb.DrawString(Game1._font, Outcome, this.Position, Color.Black, 0, Vector2.Zero, 5, SpriteEffects.None, 0f);
        }
    }
}

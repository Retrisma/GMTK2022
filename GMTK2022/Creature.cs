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
        PetriDish Dish { get; set; }

        Random Rand = new Random(Guid.NewGuid().GetHashCode());

        int RollCooldown = 0;
        bool Dead = false;

        Lerper Lerper;
        bool Moving = false;

        static int UpperBound = 100;
        static int RightBound = 830;

        static int XPathing = 30;
        static int YPathing = 16;

        int Age = 0;
        int Lifespan = 3;

        Emotion Emotion;
        public int Anger = 0;
        public int Antisocial = 0;
        public int Social = 0;

        public int Blueness = 0;
        public int BlueGene = 0;

        List<SoundEffect> DeathSoundEffects = new List<SoundEffect>
        {
            Game1._SFXContent["death1"],
            Game1._SFXContent["death2"],
            Game1._SFXContent["death3"]
        };

        public Creature(Vector2 position, int size, PetriDish dish)
        {
            Init();

            RollCooldown = Rand.Next(120);

            Texture = Game1._spriteContent[$"{Rand.Next(6) + 1}up{Rand.Next(3) + 1}"];
            Position = position;
            Size = size;
            Dish = dish;
            LayerDepth = (float)(Rand.NextDouble() / 10) + 0.9f;

            Emotion = new Emotion(this);
            Sprite.Add(Emotion);

            Anger = Rand.Next(10);
            Antisocial = Rand.Next(10);
            Social = Rand.Next(10);
        }

        int Mid(int x, int min, int max)
        {
            return Math.Min(Math.Max(min, x), max);
        }

        public void SnapAttributes()
        {
            Anger = Mid(Anger, 0, 10);
            Antisocial = Mid(Antisocial, 0, 10);
            Social = Mid(Social, 0, 10);

            Blueness = Mid(Blueness, 0, 255);
            BlueGene = Mid(Blueness, 0, 255);
        }

        public void Emotions(int angerMod = 0, int antisocialMod = 0, int socialMod = 0)
        {
            this.Anger = this.Anger + angerMod;
            this.Antisocial = this.Antisocial + antisocialMod;
            this.Social = this.Social + socialMod;

            if (Rand.Next(2) == 0)
                this.Anger -= 1;
            if (Rand.Next(2) == 0)
                this.Antisocial -= 1;
            if (Rand.Next(2) == 0)
                this.Social -= 1;
        }

        public void Roll()
        {
            int x = Rand.Next(Size) + 1;

            switch (x)
            {
                case 1:
                    Old();
                    break;
                case 2:
                    if (this.Dish.Slots[1].Domino != null)
                        this.Dish.Slots[1].Domino.Action(this);
                    break;
                case 3:
                    if (this.Dish.Slots[2].Domino != null)
                        this.Dish.Slots[2].Domino.Action(this);
                    break;
                case 4:
                    if (this.Dish.Slots[3].Domino != null)
                        this.Dish.Slots[3].Domino.Action(this);
                    break;
                case 6:
                    Baby();
                    break;
            }

            Sprite.Add(new Number(this.Position + new Vector2(16, -10), x));
        }

        public void Baby()
        {
            if (this.Dish.Creatures.Count < this.Dish.Cap)
            {
                Creature newCreature = new Creature(Dish.GenerateAcceptablePosition(), this.Size, this.Dish);

                newCreature.Blueness = BlueGene;

                this.Dish.AddCreatureToPetriDish(newCreature);
            }
        }

        public void Old()
        {
            Age++;
            if (Age >= Lifespan)
                Kill();
        }

        public void Kill()
        {
            this.Dead = true;
            this.Color = Color.Red;
            this.Emotion.Remove();
            this.Dish.Creatures.Remove(this);

            DeathSoundEffects[Rand.Next(3)].Play();
        }

        public int NearbyCreatures(int radius, Vector2 pos)
        {
            int count = 0;

            foreach (Creature creature in Dish.Creatures)
            {
                if (creature != this)
                {
                    int xa = (int)(pos.X - creature.Position.X);
                    int ya = (int)(pos.Y - creature.Position.Y);
                    int r = radius;

                    if ((xa * xa) +(ya * ya) < (r * r))  
                        count++;
                }
            }

            return count;
        }

        public void Path()
        {
            Moving = true;
            Vector2 Destination;

            while (true)
            {
                int xr = Rand.Next(2 * XPathing);
                int yr = Rand.Next(2 * YPathing);

                
                Destination = new Vector2(Math.Max(Math.Min(this.Position.X + xr - XPathing, RightBound - 32), 0),
                    Math.Max(UpperBound, Math.Min(540, this.Position.Y + yr - YPathing)));

                if (Dish.AcceptablePosition(Destination))
                {
                    int nearby = NearbyCreatures(10, this.Position);

                    if (this.Antisocial >= 5)
                        if (nearby < NearbyCreatures(10, Destination))
                            continue;

                    if (this.Social >= 5)
                        if (nearby > NearbyCreatures(10, Destination))
                            continue;

                    break;
                }
            }
            
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
            Color = new Color(255, 255, 255 - Blueness);

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

            SnapAttributes();
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            this.Emotion.DrawFromCreature(sb);
        }
    }
}

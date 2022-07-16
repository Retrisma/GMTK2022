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
    public class Emotion : Sprite
    {
        public int Index = -1;
        public Creature Parent { get; set; }

        public int Cycle = 0;

        public List<Texture2D> TextureList = new List<Texture2D>
        {
            Game1._spriteContent["anger"],
            Game1._spriteContent["antisocial"],
            Game1._spriteContent["social"]
        };

        public Emotion(Creature creature)
        {
            Init();
            IsVisible = false;

            Parent = creature;
            Texture = TextureList[0];
            Position = Parent.Position;
            LayerDepth = 0.8f;
        }

        public override void Update(GameTime gt)
        {
            Cycle++;
            if (Cycle >= 60)
                Cycle = 0;

            IsVisible = false;

            if (Parent.Anger >= 5)
            {
                IsVisible = true;
                Index = 0;
            }

            if (Parent.Antisocial > Parent.Anger && Parent.Antisocial >= 5)
            {
                IsVisible = true;
                Index = 1;
            }

            if (Parent.Social > Parent.Antisocial && Parent.Social >= 5)
            {
                IsVisible = true;
                Index = 2;
            }

            if (Index > -1)
                Texture = TextureList[Index];

            Position = Parent.Position;

            switch (Index)
            {
                case 0:
                    Position += new Vector2((float)Math.Sin(Cycle * Math.PI / 10));
                    break;
                case 1:
                    Position += new Vector2(0, (float)(Cycle + 30) * (Cycle - 30) / 300);
                    break;
                case 2:
                    Position += new Vector2(0, (float)Math.Sin(Cycle * Math.PI / 10));
                    break;
                default:
                    break;
            }
        }

        public void DrawFromCreature(SpriteBatch sb)
        {
            if (this.IsVisible)
                sb.Draw(Texture, Position, Source, Color, 0, Origin, Scale, SpriteEffects.None, LayerDepth);
        }

        public override void Draw(SpriteBatch sb)
        {
            
        }
    }
}

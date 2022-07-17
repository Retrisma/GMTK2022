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
    public class DominoLookup
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Texture2D Texture { get; set; }

        public DominoLookup(string name, string desc, Texture2D texture)
        {
            Name = name;
            Description = desc;
            Texture = texture;
        }
    }
    public class Domino : Draggable
    {
        public static List<DominoLookup> Dominoes = new List<DominoLookup>
        {
            new DominoLookup("Alluring Cast", "This die activates a brief magnetic effect on nearby dice.", Game1._spriteContent["AlluringCast"]),
            new DominoLookup("Feeling Blue", "This die's offspring will be slightly bluer.", Game1._spriteContent["FeelingBlue"]),
            new DominoLookup("Rollin' High", "This die becomes more social.", Game1._spriteContent["RollinWithTheBestOfThem"]),
            new DominoLookup("Quantum Rolling", "This die may teleport to a different petri dish.", Game1._spriteContent["QuantumRolling"]),
            new DominoLookup("I Roll Alone", "This die becomes less social.", Game1._spriteContent["RollAlone"]),
            new DominoLookup("Mitosis", "When rolled, this die has a low chance to divide into two identical dice.", Game1._spriteContent["Mitosis"]),
            new DominoLookup("Angry Young Die", "This die's children will be slightly angrier.", Game1._spriteContent["Angry Young Die"]),
            new DominoLookup("Maverick", "This die develops a trait that is unpopular in its own dish.", Game1._spriteContent["Maverick"]),
            new DominoLookup("Crowd Chaser", "This die slightly develops a trait popular in its own dish.", Game1._spriteContent["Crowd Chaser"]),
            new DominoLookup("Close-Knit", "This die's children are more likely to be born in the same petri dish.", Game1._spriteContent["Close Knit"]),
            new DominoLookup("Quantum Progenitor", "This die's children are more likely to be born in a random different petri dish.", Game1._spriteContent["Quantum Progenitor"]),
            new DominoLookup("Terrible Stench", "This die briefly repels nearby dice.", Game1._spriteContent["TerribleStench"]),
            new DominoLookup("Take It Easy!", "This die moves and rolls itself more slowly.", Game1._spriteContent["TakeItEasy"]),
            new DominoLookup("Speed Roller", "This die moves and rolls itself more quickly.", Game1._spriteContent["SpeedRoller"]),
            new DominoLookup("Redshift", "This die's offspring will be slightly redder", Game1._spriteContent["Redshift"]),

        };

        public static Domino RandomDomino(Vector2 pos)
        {
            int x = Game1._rand.Next(Dominoes.Count);

            DominoLookup z = Dominoes[x];

            return new Domino(x, z.Texture, pos, z.Name, z.Description);
        }

        public int Value { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int ColorChangeCooldown { get; set; }

        public Slot Slot = null;

        UiRect UiRect;
        Vector2 TextPos = Vector2.Zero;
        bool DrawText = false;

        public Domino(int value, Texture2D texture, Vector2 position, string name, string description) : base(texture, position)
        {
            Value = value;
            Texture = texture;
            Position = position;
            Name = name;
            Description = description;
            LayerDepth = (float)Game1._rand.NextDouble() / 100 + 0.03f;

            UiRect = new UiRect(0, 0, 0, 0, Color.Lavender, Color.Purple);
            UiRect.IsVisible = false;
            UiRect.LayerDepth = 0.02f;
            Sprite.Add(UiRect);
        }

        public void Action(Creature creature)
        {
            switch (Value)
            {
                case 0: // Alluring Cast
                    break;
                case 1: // Feeling Blue
                    creature.BlueGene += 100;
                    break;
                case 2: // Rollin' High
                    creature.Emotions(0, 0, 5);
                    break;
                case 3: // Quantum Rolling
                    creature.ChanceToTeleport(3);
                    break;
                case 4: // I Roll Alone
                    creature.Emotions(0, 5, 0);
                    break;
                case 5: // Mitosis
                    break;
                case 6: // Angry Young Die
                    creature.AngerGene += 5;
                    break;
                case 7: // Maverick
                    break;
                case 8: // Crowd Chaser
                    break;
                case 9: // Close-Knit
                    break;
                case 10: // Quantum Progenitor
                    break;
                case 11: // Terrible Stench
                    break;
                case 12: // Take It Easy!
                    creature.Speed += 10;
                    break;
                case 13: // Speed Roller
                    creature.Speed -= 10;
                    break;
                case 14: // Redshift
                    creature.RedGene += 100;
                    break;
            }

            ColorChangeCooldown = 60;
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
            
            if (!Holding && WasHeldLastFrame)
                Game1._SFXContent["dominodrop4"].Play();

            MouseState mouseState = Game1._mouseState;
            string textOut = $"{Name}\n\n{Description}";

            if (Slot != null)
            {
                if (Slot.Dish == null)
                {
                    Position = Slot.Position - new Vector2(0, 128);
                    Bounds = new Rectangle((int)Slot.Position.X, (int)Slot.Position.Y - 64, 64, 64);
                }
                else
                {
                    if (Slot.Dish.LeftOrRight)
                    {
                        this.Rotation = (float)(Math.PI / 2);
                        Position = Slot.Position + new Vector2(64, 0);
                        Bounds = new Rectangle((int)Slot.Position.X, (int)Slot.Position.Y, 64, 64);
                    }
                    else
                    {
                        this.Rotation = (float)(Math.PI / 2);
                        Position = Slot.Position + new Vector2(128, 0);
                        Bounds = new Rectangle((int)Slot.Position.X, (int)Slot.Position.Y, 64, 64);
                    }
                }
                
                Clickable = false;

                if (Bounds.Contains(mouseState.Position))
                    this.Color = Color.Green;
                else
                    this.Color = Color.White;

                Holding = false;

                ColorChangeCooldown--;

                if (this.ColorChangeCooldown > 0)
                    this.Color = new Color(255 - ColorChangeCooldown * 3, 255, 255 - ColorChangeCooldown * 3);
                else
                    this.Color = Color.White;
            }
            else
            {
                this.Color = Color.White;
                this.Rotation = 0;
                this.Bounds = new Rectangle((int)Position.X, (int)Position.Y, (int)Width, (int)Height);
            }

            if (Bounds.Contains(mouseState.Position) && !Holding)
            {
                Vector2 stringBox = Game1._font.MeasureString(textOut);

                TextPos = new Vector2(mouseState.Position.X, mouseState.Position.Y - stringBox.Y - 2);

                if (TextPos.Y - 16 < 0)
                    TextPos = new Vector2(TextPos.X, mouseState.Position.Y + stringBox.Y + 2);

                if (stringBox.X + TextPos.X > 1060)
                    TextPos = new Vector2(TextPos.X + (1060 - (stringBox.X + TextPos.X)) - 8, TextPos.Y);

                UiRect.rect = new Rectangle((int)TextPos.X - 10, (int)TextPos.Y - 10, (int)stringBox.X + 20, (int)stringBox.Y + 20);

                DrawText = true;
                UiRect.IsVisible = true;
            }
            else
            {
                DrawText = false;
                UiRect.IsVisible = false;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            if (DrawText)
                sb.DrawString(Game1._font, $"{Name}\n\n{Description}", TextPos, Color.Black, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.01f);
        }
    }
}

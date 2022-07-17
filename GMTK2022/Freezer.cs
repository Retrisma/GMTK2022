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
    public class Freezer
    {
        public List<FreezerSlot> slots;
        private bool AddedThisFrame = false;

        public Freezer()
        {
            slots = new List<FreezerSlot>();
            for (int i = 0; i < 5; i++)
            {
                slots.Add(new FreezerSlot(i));
            }
            Game1._spritesToAdd.AddRange(slots);
        }

        public void Update(GameTime gt)
        {
            AddedThisFrame = false;

            foreach (Sprite sprite in Game1._sprites)
            {
                if (sprite.GetType() == typeof(Creature))
                {
                    Creature x = (Creature)sprite;
                    if (x.ClickLogic() && !AddedThisFrame)
                    {
                        AddedThisFrame = true;
                        LastEmptySlot().AddCreatureToThis(x);
                        x.Dish.Creatures.Remove(x);
                        x.Remove();
                    }
                }
            }
        }

        public FreezerSlot LastEmptySlot()
        {
            foreach (FreezerSlot slot in slots)
                if (slot.Creature == null)
                    return slot;
            return null;
        }
    }

    public class FreezerSlot : Sprite
    {
        public FreezerCreature Creature { get; set; }
        public Rectangle Bounds;
        public Vector2 TextPos;
        public UiRect UiRect;
        public bool DrawText;
        public string TextOut = "";

        public FreezerSlot(int i)
        {
            Init();
            Position = new Vector2(700 + i * 50, 40);
            Texture = Game1._spriteContent["FreezerSlot"];
            LayerDepth = 0.4f;
            Bounds = new Rectangle((int)Position.X, (int)Position.Y, (int)Texture.Width, (int)Texture.Height);

            UiRect = new UiRect(0, 0, 0, 0, Color.Lavender, Color.Purple);
            UiRect.IsVisible = false;
            UiRect.LayerDepth = 0.02f;
            Sprite.Add(UiRect);
        }

        public void AddCreatureToThis(Creature creature)
        {
            FreezerCreature x = new FreezerCreature(creature, this);
            this.Creature = x;
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);

            MouseState mouseState = Game1._mouseState;

            if (Bounds.Contains(mouseState.Position) && Creature != null)
            {
                TextOut = $"Anger: {Creature.Anger}\nSocial: {Creature.Social}\nAntisocial: {Creature.Antisocial}";

                Vector2 stringBox = Game1._font.MeasureString(TextOut);

                TextPos = new Vector2(mouseState.Position.X, mouseState.Position.Y - stringBox.Y - 2);

                if (TextPos.Y - 30 < 0)
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
            if (this.Creature != null)
                Creature.Draw(sb);

            if (DrawText)
                sb.DrawString(Game1._font, TextOut, TextPos, Color.Black, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.01f);
        }
    }

    public class FreezerCreature : Sprite
    {
        public FreezerSlot Slot { get; set; }
        public int Anger { get; set; }
        public int Antisocial { get; set; }
        public int Social { get; set; }
        public int Speed { get; set; }

        public FreezerCreature(Creature creature, FreezerSlot slot)
        {
            Init();
            Slot = slot;
            Position = Slot.Position;
            Texture = creature.Texture;
            Anger = creature.Anger;
            Antisocial = creature.Antisocial;
            Social = creature.Social;
            Speed = creature.Speed;
            Color = creature.Color;
            LayerDepth = 0.3f;
        }

        public void Sell()
        {
            int x = 0;
            x += Anger * 5;
            x += Antisocial * 5;
            x += Social * 5;

            Game1._money += x;

            this.Slot.Creature = null;
            this.Remove();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace GMTK2022
{
    //////////////////////////////////////////////////////////////////////////////
    public class UiRect : Sprite
    {
        public Rectangle rect { get; set; }
        private Color fill_color;
        private Color border_color;
        private int border_size = 1;

        public UiRect(int pos_x, int pos_y, int width, int height, Color _fill_color, Color _border_color)
        {
            rect = new Rectangle(pos_x, pos_y, width, height);
            fill_color = _fill_color;
            border_color = _border_color;
            LayerDepth = 0.8f;
        }

        public void Fill(Texture2D fill, SpriteBatch sb)
        {
            sb.Draw(fill, new Vector2(rect.X, rect.Y), new Rectangle(0, 0, 1, 1), border_color, 0.0f, new Vector2(0, 0), new Vector2(rect.Width, rect.Height), SpriteEffects.None, LayerDepth + 0.01f);
            sb.Draw(fill, new Vector2(rect.X + border_size, rect.Y + border_size), new Rectangle(0, 0, 1, 1), fill_color, 0.0f, new Vector2(0, 0), new Vector2(rect.Width - (border_size * 2), rect.Height - (border_size * 2)), SpriteEffects.None, LayerDepth);
        }

        public override void Draw(SpriteBatch sb)
        {
            if (IsVisible)
                Fill(Game1._spriteContent["Pixel"], sb);
        }
    }
    //////////////////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////////////////
    public class BoxButton : Sprite
    {
        private Rectangle rect;
        private string text;
        private Color fill_color;
        private Color border_color;
        private Color current_fill_color;
        private Color current_border_color;
        private int border_size = 2;
        private bool mouse_hovered = false;
        private float hover_highlight_amount = 0.3f;
        private MouseState prev_mouse_state;
        private MouseState mouse_state;

        public BoxButton(string _text, int pos_x, int pos_y, int width, int height, Color _fill_color, Color _border_color)
        {
            rect = new Rectangle(pos_x, pos_y, width, height);
            text = _text;
            fill_color = _fill_color;
            border_color = _border_color;
        }

        public override void Update(GameTime gameTime)
        {
            mouse_state = Mouse.GetState();
            Vector2 mouse_pos = new Vector2(mouse_state.X, mouse_state.Y);
            bool mouse_hovered_hor = (mouse_pos.X >= rect.X) && (mouse_pos.X <= (rect.X + rect.Width));
            bool mouse_hovered_ver = (mouse_pos.Y >= rect.Y) && (mouse_pos.Y <= (rect.Y + rect.Height));
            mouse_hovered = mouse_hovered_hor && mouse_hovered_ver;

            bool current_clicked = mouse_state.LeftButton == ButtonState.Pressed;
            bool prev_up = prev_mouse_state.LeftButton == ButtonState.Released;
            if (mouse_hovered && (current_clicked && prev_up))
            {
                System.Diagnostics.Debug.WriteLine(text);
            }

            prev_mouse_state = mouse_state;
        }

        public void Fill(Texture2D fill, SpriteBatch sb)
        {
            if (mouse_hovered)
            {
                Vector3 hover_border_color = border_color.ToVector3() + new Vector3(hover_highlight_amount, hover_highlight_amount, hover_highlight_amount);
                current_border_color = new Color(hover_border_color.X, hover_border_color.Y, hover_border_color.Z);

                Vector3 hover_fill_color = fill_color.ToVector3() + new Vector3(hover_highlight_amount, hover_highlight_amount, hover_highlight_amount);
                current_fill_color = new Color(hover_fill_color.X, hover_fill_color.Y, hover_fill_color.Z);

                //Mouse.SetCursor(MouseCursor.Hand);
            }
            else
            {
                current_border_color = border_color;
                current_fill_color = fill_color;

                //Mouse.SetCursor(MouseCursor.Arrow);
            }

            sb.Draw(fill, new Vector2(rect.X, rect.Y), new Rectangle(0, 0, 1, 1), current_border_color, 0.0f, new Vector2(0, 0), new Vector2(rect.Width, rect.Height), SpriteEffects.None, 0.22f);
            sb.Draw(fill, new Vector2(rect.X + border_size, rect.Y + border_size), new Rectangle(0, 0, 1, 1), current_fill_color, 0.0f, new Vector2(0, 0), new Vector2(rect.Width - (border_size * 2), rect.Height - (border_size * 2)), SpriteEffects.None, 0.21f);
        }

        public void DrawText(SpriteBatch sb, SpriteFont font)
        {
            Vector2 textMiddlePoint = font.MeasureString(text) / 2;
            sb.DrawString(Game1._font, text, rect.Center.ToVector2(), current_border_color, 0, textMiddlePoint, 1, SpriteEffects.None, 0.2f);
        }

        public override void Draw(SpriteBatch sb)
        {
            Fill(Game1._spriteContent["Pixel"], sb);
            DrawText(sb, Game1._font);
        }
    }
    //////////////////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////////////////
    public class Draggable : Sprite
    {
        public Rectangle Bounds;
        public bool Holding = false;
        public bool WasHeldLastFrame = false;
        public bool Clickable = true;

        public Draggable(Texture2D texture, Vector2 position)
        {
            Init();
            Texture = texture;
            Position = position;
            Width = texture.Width;
            Height = texture.Height;
            Bounds = new Rectangle((int)position.X, (int)position.Y, (int)Width, (int)Height);
            LayerDepth = 0.1f;
        }

        public override void Update(GameTime gt)
        {
            WasHeldLastFrame = false;

            if (Clickable)
            {
                MouseState mouse_state = Mouse.GetState();
                Vector2 mouse_pos = new Vector2(mouse_state.X, mouse_state.Y);

                if (mouse_state.LeftButton != ButtonState.Pressed && Holding)
                {
                    Holding = false;
                    WasHeldLastFrame = true;
                }

                if ((Bounds.Contains(mouse_pos) && mouse_state.LeftButton == ButtonState.Pressed && (!HoldingAnyOtherDraggable())) || Holding)
                {
                    Holding = true;
                    this.Position = mouse_pos - new Vector2((int)(Width / 2), (int)(Height / 2));
                    Bounds = new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)Width, (int)Height);
                }
            }
        }

        public bool HoldingAnyOtherDraggable()
        {
            foreach (Sprite sprite in Game1._sprites)
            {
                if (sprite != this && sprite.GetType() == typeof(Draggable))
                {
                    Draggable sp = (Draggable)sprite;
                    if (sp.Holding == true)
                        return true;
                }
            }

            return false;
        }
    }
}
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
    public class UiRect
    {
        private Rectangle rect;
        private Color fill_color;
        private Color border_color;
        private int border_size = 1;

        public UiRect(int pos_x, int pos_y, int width, int height, Color _fill_color, Color _border_color)
        {
            rect = new Rectangle(pos_x, pos_y, width, height);
            fill_color = _fill_color;
            border_color = _border_color;
        }

        public void Fill(Texture2D fill, SpriteBatch sb)
        {
            sb.Draw(fill, new Vector2(rect.X, rect.Y), new Rectangle(0, 0, 1, 1), border_color, 0.0f, new Vector2(0, 0), new Vector2(rect.Width, rect.Height), SpriteEffects.None, 0f);
            sb.Draw(fill, new Vector2(rect.X + border_size, rect.Y + border_size), new Rectangle(0, 0, 1, 1), fill_color, 0.0f, new Vector2(0, 0), new Vector2(rect.Width - (border_size * 2), rect.Height - (border_size * 2)), SpriteEffects.None, 0f);
        }
    }
    //////////////////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////////////////
    public class BoxButton
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

        public virtual void Update(GameTime gameTime)
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

            sb.Draw(fill, new Vector2(rect.X, rect.Y), new Rectangle(0, 0, 1, 1), current_border_color, 0.0f, new Vector2(0, 0), new Vector2(rect.Width, rect.Height), SpriteEffects.None, 0f);
            sb.Draw(fill, new Vector2(rect.X + border_size, rect.Y + border_size), new Rectangle(0, 0, 1, 1), current_fill_color, 0.0f, new Vector2(0, 0), new Vector2(rect.Width - (border_size * 2), rect.Height - (border_size * 2)), SpriteEffects.None, 0f);
        }

        public void DrawText(SpriteBatch sb, SpriteFont font)
        {
            Vector2 textMiddlePoint = font.MeasureString(text) / 2;
            sb.DrawString(Game1._font, text, rect.Center.ToVector2(), current_border_color, 0, textMiddlePoint, 1, SpriteEffects.None, 0f);
        }
    }
    //////////////////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////////////////
    public class Draggable : Sprite
    {
        private Sprite sprite;

        public Draggable(Texture2D texture, int x, int y)
        {
            sprite.Texture = texture;
            sprite.Position = new Vector2(x, y);
            Sprite.Add(sprite);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite.Texture, sprite.Position, null, Color.White, 0.0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
        }
    }
}
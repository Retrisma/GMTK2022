﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace GMTK2022
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static KeyboardState _state;
        public static KeyboardState _previousState;

        public static List<Sprite> _sprites;
        public static List<Sprite> _spritesToAdd;
        public static List<Sprite> _spritesToRemove;

        private Texture2D box_fill;
        public static List<UiRect> ui_rects;
        public static List<BoxButton> buttons;

        public static Dictionary<string, Texture2D> _spriteContent;
        public static Dictionary<string, SoundEffect> _musicContent;
        public static Dictionary<string, SoundEffect> _SFXContent;

        public static SpriteFont _font;

        public static Random _rand;

        public static bool KeyPress(Keys key)
        {
            if (!_previousState.IsKeyDown(key) && _state.IsKeyDown(key))
                return true;
            return false;
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1060;
            _graphics.PreferredBackBufferHeight = 540;
            _graphics.ApplyChanges();

            _sprites = new List<Sprite>();
            _spritesToAdd = new List<Sprite>();
            _spritesToRemove = new List<Sprite>();

            _rand = new Random(Guid.NewGuid().GetHashCode());

            // Create UI rectangles.
            ui_rects = new List<UiRect>();
            ui_rects.Add(new UiRect(0, 0, 960, 100, new Color(71, 79, 83), new Color(21, 29, 33)));
            ui_rects.Add(new UiRect(860, 0, 100, 540, new Color(125, 176, 127), new Color(75, 126, 77)));
            ui_rects.Add(new UiRect(560, 0, 300, 100, new Color(210, 226, 214), new Color(160, 176, 164)));
            ui_rects.Add(new UiRect(860, 0, 100, 100, new Color(166, 206, 191), new Color(116, 156, 141)));
            ui_rects.Add(new UiRect(0, 0, 100, 540, new Color(214, 102, 113), new Color(164, 52, 63)));

            // Create UI buttons.
            buttons = new List<BoxButton>();
            buttons.Add(new BoxButton("$50", 108, 25, 100, 50, Color.Black, Color.LightGray));
            buttons.Add(new BoxButton("$100", 220, 25, 100, 50, Color.Black, Color.LightGray));
            buttons.Add(new BoxButton("$150", 334, 25, 100, 50, Color.Black, Color.LightGray));
            buttons.Add(new BoxButton("$200", 446, 25, 100, 50, Color.Black, Color.LightGray));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _spriteContent = ContentLoader.LoadListContent<Texture2D>(Content, "Graphics");
            //_musicContent = ContentLoader.LoadListContent<SoundEffect>(Content, "Music");
            _SFXContent = ContentLoader.LoadListContent<SoundEffect>(Content, "SoundEffects");

            //for (int i = 0; i < 20; i++)
            //    Sprite.Add(new Creature(6));

            _sprites.Add(new PetriDish(new Vector2(120, 120)));
            _sprites.Add(new PetriDish(new Vector2(540, 120)));

            _font = Content.Load<SpriteFont>("font");

            box_fill = Content.Load<Texture2D>("Graphics/Pixel");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _state = Keyboard.GetState();

            foreach (Sprite sprite in _spritesToRemove)
            {
                _sprites.Remove(sprite);
            }
            _spritesToRemove.Clear();

            _sprites.AddRange(_spritesToAdd);
            _spritesToAdd.Clear();

            foreach (Sprite sprite in _sprites)
            {
                sprite.Update(gameTime);
            }

            foreach (BoxButton button in buttons)
            {
                button.Update(gameTime);
            }

            _previousState = _state;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 10f, 0);

            _spriteBatch.Begin(samplerState:SamplerState.PointClamp);

            foreach (Sprite sprite in _sprites)
            {
                sprite.Draw(_spriteBatch);
            }

            foreach (UiRect ui_rect in ui_rects)
            {
                ui_rect.Fill(box_fill, _spriteBatch);
            }

            foreach (BoxButton button in buttons)
            {
                button.Fill(box_fill, _spriteBatch);
                button.DrawText(_spriteBatch, _font);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

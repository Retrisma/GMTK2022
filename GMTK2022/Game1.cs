using System;
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

        public static List<BoxButton> buttons;
        private Texture2D box_fill;

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
            _graphics.PreferredBackBufferWidth = 960;
            _graphics.PreferredBackBufferHeight = 540;

            _sprites = new List<Sprite>();
            _spritesToAdd = new List<Sprite>();
            _spritesToRemove = new List<Sprite>();

            _rand = new Random(Guid.NewGuid().GetHashCode());

            // Create UI buttons.
            buttons = new List<BoxButton>();
            buttons.Add(new BoxButton("Test Button", 16, 16, 100, 50, Color.Black, Color.LightGray));
            buttons.Add(new BoxButton("Other Button", 132, 16, 100, 50, Color.Black, Color.LightGray));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _spriteContent = ContentLoader.LoadListContent<Texture2D>(Content, "Graphics");
            //_musicContent = ContentLoader.LoadListContent<SoundEffect>(Content, "Music");
            _SFXContent = ContentLoader.LoadListContent<SoundEffect>(Content, "SoundEffects");

            for (int i = 0; i < 4; i++)
                Sprite.Add(new Creature(6));

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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState:SamplerState.PointClamp);

            foreach (Sprite sprite in _sprites)
            {
                sprite.Draw(_spriteBatch);
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

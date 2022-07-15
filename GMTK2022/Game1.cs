using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _sprites = new List<Sprite>();
            _spritesToAdd = new List<Sprite>();
            _spritesToRemove = new List<Sprite>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            
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

            _previousState = _state;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            foreach (Sprite sprite in _sprites)
            {
                sprite.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

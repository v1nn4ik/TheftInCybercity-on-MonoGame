﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheftInCybercity
{
#nullable disable
    public class Button : Component
    {
        #region Fields

        protected MouseState _currentMouse;
        protected MouseState _previousMouse;
        protected Texture2D _texture;
        public Vector2 _position;

        #endregion

        #region Properties

        public Vector2 Position { get { return _position; } }
        public event EventHandler Click;
        public bool Clicked { get; private set; }        
        public Rectangle Rectangle { get { return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height); } }

        #endregion

        #region Methods

        public Button(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            _position = position;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) => spriteBatch.Draw(_texture, Rectangle, Color.White);

        public override void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            if (mouseRectangle.Intersects(Rectangle))
                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                    Click?.Invoke(this, new EventArgs());
        }

        #endregion
    }
}
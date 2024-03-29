﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;

namespace TheftInCybercity
{
    public class Enemy : Component
    {
        #region Fields

        public Vector2 _position;
        protected Vector2 _velocity;
        protected Vector2 _origin;
        public CollisionTypes CollisionType;

        public AnimatedSprite _enemy;

        protected bool _onGround;
        public bool _hasJumped;
        public bool _hasDead;

        #endregion

        #region Properties

        public Vector2 Velocity { get { return _velocity; } }

        public Vector2 Origin { get { return _origin; } set { _origin = value; } }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public RectangleF CollisionBox
        {
            get
            {
                return new RectangleF(Position.X - Origin.X, Position.Y - Origin.Y, 144, 120);
            }
        }

        #endregion

        #region Methods

        public Enemy(AnimatedSprite enemy, Vector2 position, CollisionTypes collisionType)
        {
            _enemy = enemy;
            _position = position;
            CollisionType = collisionType;
            Origin = new Vector2(_enemy.TextureRegion.Width / 2, _enemy.TextureRegion.Height / 2);
            enemy.Play("idle");
        }

        public override void Update(GameTime gameTime)
        {
            SetAnimations();
            _enemy.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) =>
            spriteBatch.Draw(_enemy, Position);

        public void ApplyPhysics(GameTime gameTime)
        {
            if (!_onGround)
                _velocity.Y += 0.3f;
            _onGround = false;
            Position += _velocity;
        }

        protected void SetAnimations()
        {
            var animation = "idle";

            if (_velocity.X > 0)
                animation = "runRight";
            else if (_velocity.X < 0)
                animation = "runLeft";
            else if (_velocity.X == 0 && _velocity.Y == 0)
                animation = "idle";
            else if (_velocity.Y > 0)
                animation = "fall";

            _enemy.Play(animation);
        }

        #region Collision

        public void OnCollide(Object sprite)
        {
            var onTop = this.WillIntersectTop(sprite);
            var onLeft = this.WillIntersectLeft(sprite);
            var onRight = this.WillIntersectRight(sprite);
            var onBotton = this.WillIntersectBottom(sprite);

            if (onTop)
            {
                _onGround = true;
                _velocity.Y = sprite.CollisionBox.Top - this.CollisionBox.Bottom;
            }
            else if (onLeft && _velocity.X > 0)
                _velocity.X = 0;
            else if (onRight && _velocity.X < 0)
                _velocity.X = 0;
            else if (onBotton)
                _velocity.Y = 1;
        }

        public bool WillIntersect(Object sprite)
        {
            return this.WillIntersectBottom(sprite) ||
              this.WillIntersectLeft(sprite) ||
              this.WillIntersectRight(sprite) ||
              this.WillIntersectTop(sprite);
        }

        public bool WillIntersectLeft(Object sprite)
        {
            return this.CollisionBox.Right + this._velocity.X >= sprite.CollisionBox.Left &&
              this.CollisionBox.Left + this._velocity.X < sprite.CollisionBox.Left &&
              this.CollisionBox.Top < sprite.CollisionBox.Bottom &&
              this.CollisionBox.Bottom > sprite.CollisionBox.Top;
        }

        public bool WillIntersectRight(Object sprite)
        {
            return this.CollisionBox.Left + this._velocity.X <= sprite.CollisionBox.Right &&
              this.CollisionBox.Right > sprite.CollisionBox.Right &&
              this.CollisionBox.Top < sprite.CollisionBox.Bottom &&
              this.CollisionBox.Bottom > sprite.CollisionBox.Top;
        }

        public bool WillIntersectTop(Object sprite)
        {
            return this.CollisionBox.Bottom + this._velocity.Y >= sprite.CollisionBox.Top &&
              this.CollisionBox.Top < sprite.CollisionBox.Top &&
              this.CollisionBox.Right > sprite.CollisionBox.Left &&
              this.CollisionBox.Left < sprite.CollisionBox.Right;
        }

        public bool WillIntersectBottom(Object sprite)
        {
            return this.CollisionBox.Top + this._velocity.Y <= sprite.CollisionBox.Bottom &&
              this.CollisionBox.Bottom > sprite.CollisionBox.Bottom &&
              this.CollisionBox.Right > sprite.CollisionBox.Left &&
              this.CollisionBox.Left < sprite.CollisionBox.Right;
        }

        #endregion

        #endregion
    }
}

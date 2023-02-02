using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Overhaul_Of_Apocalyptica.Entities.Projectiles;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Overhaul_Of_Apocalyptica.Entities.Weapons
{
    class Shuriken : Projectile, ICollidable
    {
        private float _speed = 4f;
        private string _direction;
        private Vector2 _movementVector2;

        public Shuriken(Vector2 posFired, string directionFired, Texture2D texture)
        {
            _direction = directionFired;
            switch (_direction)
            {
                case "left":
                    _movementVector2 = new Vector2(-_speed, 0);
                    break;
                case "right":
                    _movementVector2 = new Vector2(_speed, 0);
                    break;
                case "up":
                    _movementVector2 = new Vector2(0, -_speed);
                    break;
                case "down":
                    _movementVector2 = new Vector2(0, _speed);
                    break;
            }
            Position = posFired;
            ProjectSprite = new Sprite(texture, new List<Rectangle>() { new Rectangle(0, 0, 20, 20) }, Position);
            CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, ProjectSprite.Source.Width, ProjectSprite.Source.Height);
        }
        public override void Collided(GameTime gameTime, ICollidable collidedWith)
        {
            if (!((collidedWith.GetType().Name == "Ninja") || (collidedWith.GetType().Name == "Bullet")))
            {
                IsDestroyed = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!IsDestroyed)
            {
                //start animations U CAN DO THIS UR SO CLOSE
                ProjectSprite.Draw(spriteBatch, gameTime,);
            }
        }

        public override void Flight(GameTime gameTime)
        {
            if (!IsDestroyed)
            {
                Position = Vector2.Add(Position, _movementVector2);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsDestroyed)
            {
                Flight(gameTime);
                ProjectSprite.Update(gameTime, Position);
                CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, ProjectSprite.Source.Width, ProjectSprite.Source.Height);
            }
        }
    }
}

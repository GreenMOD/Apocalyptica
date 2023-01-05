using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;
using Overhaul_Of_Apocalyptica.Entities.Characters;
namespace Overhaul_Of_Apocalyptica.Entities.Projectiles
{
    class Bullet : Projectile
    {
        private float _bulletSpeed = 4f;
        private string _direction;
        private Vector2 _movementVector2;
        
        public Bullet(Vector2 posFired, string directionFired , Texture2D texture)
        {
            _direction = directionFired;
            switch (_direction)
            {
                case "left":
                    _movementVector2 = new Vector2(-_bulletSpeed, 0);
                    break;
                case "right":
                    _movementVector2 = new Vector2(_bulletSpeed, 0);
                    break;
                case "up":
                    _movementVector2 = new Vector2(0, -_bulletSpeed);
                    break;
                case "down":
                    _movementVector2 = new Vector2(0, _bulletSpeed);
                    break;
            }
            Position = posFired;
            _sprite = new Sprite(texture, new List<Rectangle>() {new Rectangle(0,0,12,6) }, Position);
        }
        public override void CheckCollision(GameTime gameTime)
        {
            if (true)
            {
                _target.Health -= 10;
                IsDestroyed = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _sprite.Draw(spriteBatch, gameTime);
        }

        public override void Flight(GameTime gameTime)
        {
            Position = Vector2.Add(Position, _movementVector2);
        }

        public override void Update(GameTime gameTime)
        {
            Flight(gameTime);
            _sprite.Update(gameTime, Position);
        }
    }
}

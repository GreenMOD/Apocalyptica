using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Overhaul_Of_Apocalyptica.Entities.Characters;
namespace Overhaul_Of_Apocalyptica.Entities.Projectiles
{
   public class Bullet : Projectile, ICollidable
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
            ProjectSprite = new Sprite(texture, new List<Rectangle>() {new Rectangle(0,0,12,6) }, Position);
            CollisionBox = new Rectangle((int)Position.X,(int)Position.Y, ProjectSprite.Source.Width, ProjectSprite.Source.Height);
        }
        public override void Collided(GameTime gameTime, ICollidable collidedWith)
        {
            if (!((collidedWith.GetType().Name == "Soldier") || (collidedWith.GetType().Name == "Bullet")))
            {
                Debug.WriteLine(collidedWith.GetType().Name);
                IsDestroyed = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!IsDestroyed)
            {
                ProjectSprite.Draw(spriteBatch, gameTime);
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

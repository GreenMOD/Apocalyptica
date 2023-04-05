using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
namespace Overhaul_Of_Apocalyptica.Entities.Projectiles
{
   public class Bullet : Projectile
    {
        private float _bulletSpeed = 4f;
        private Vector2 _movementVector2;
        
        
        public Bullet(Vector2 posFired, string directionFired , Texture2D texture)
        {
            switch (directionFired)
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
        /// <summary>
        /// Using the name of the object collided with this subroutine decides whether this bullet is destoryed or not
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="collidedWith"></param>
        public override void Collided(GameTime gameTime, ICollidable collidedWith)
        {
            if (!((collidedWith.GetType().Name == "Soldier") || (collidedWith.GetType().FullName.Contains("Projectile") || (collidedWith.GetType().Name == "Heavy"))))
            {
                IsDestroyed = true;
            }
        }
        /// <summary>
        /// If this is not destoryed then the sprite for this projectile is displayed
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!IsDestroyed)
            {
                ProjectSprite.Draw(spriteBatch, gameTime);
            }
        }
        /// <summary>
        /// Movement in the direction fired is calculated and added to the positon of the bullet. Flight time is then incremented acorrdingly.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Flight(GameTime gameTime)
        {
            float elasped = (float)gameTime.ElapsedGameTime.TotalSeconds;

            FlightTime += elasped;
            if (!IsDestroyed)
            {
                Position = Vector2.Add(Position, _movementVector2);
            }
          
        }
        /// <summary>
        /// If the bullet is not destroyed Flight is called and the Sprite and collision box is updated
        /// </summary>
        /// <param name="gameTime"></param>
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

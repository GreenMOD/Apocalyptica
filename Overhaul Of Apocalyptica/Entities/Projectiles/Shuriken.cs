using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Overhaul_Of_Apocalyptica.Entities.Projectiles;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Overhaul_Of_Apocalyptica.Entities.Projectiles
{
    class Shuriken : Projectile
    {
        private float _speed = 4f;
        private Vector2 _movementVector2;

        public Shuriken(Vector2 posFired, string directionFired, Texture2D texture)
        {
            switch (directionFired)
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
        /// <summary>
        /// Using the name of the object collided with this subroutine decides whether this shuriken is destoryed or not.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="collidedWith"></param>
        public override void Collided(GameTime gameTime, ICollidable collidedWith)
        {
            if (!((collidedWith.GetType().Name == "Ninja") || (collidedWith.GetType().FullName.Contains("Projectile"))))
            {
                IsDestroyed = true;
            }
        }
        /// <summary>
        /// If this is not destroyed, the sprite is drawn and then the Sprite's rotation is incremented by 90 to make the shuriken spin
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!IsDestroyed)
            {
                ProjectSprite.Draw(spriteBatch, gameTime);
                

                ProjectSprite.Rotation = ProjectSprite.Rotation + 90;
            }
        }
        /// <summary>
        /// If this shuriken is not destoryed it will move in the direction it was fired. FlightTime is also incremented.
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
        /// If this is not destroyed, Flight is called and both the sprite and collison box are updated.
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

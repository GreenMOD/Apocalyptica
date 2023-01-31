using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Overhaul_Of_Apocalyptica.Entities.Weapons
{
    class Shuriken : Projectile, IEntity
    {
        public override void Collided(GameTime gameTime, ICollidable collidedWith)
        {
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

        }

        public override void Flight(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}

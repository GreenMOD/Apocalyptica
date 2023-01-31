using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
namespace Overhaul_Of_Apocalyptica.Entities
{
    public interface ICollidable
    {
        public Rectangle CollisionBox { get; set; }

        public void Collided(GameTime gameTime, ICollidable collidedWith);

    }
}

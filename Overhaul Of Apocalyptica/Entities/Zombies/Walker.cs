using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Overhaul_Of_Apocalyptica.Entities.Characters;

namespace Overhaul_Of_Apocalyptica.Entities
{
    class Walker : Zombie
    {
        protected Rectangle frame1 = new Rectangle(0, 0, 17, 38);  // left
        protected Rectangle frame2 = new Rectangle(20, 0, 17, 38);  // right
     
        public Walker(Texture2D texture2D, EntityManager entityManager, Vector2 spawnLocation,List<Player> players )
        {
            Position = spawnLocation;
            ZombieSprite = new Sprite(texture2D, new List<Rectangle>() {frame1, frame2 }, Position);
            CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, ZombieSprite.Source.Width *2, ZombieSprite.Source.Height*2);
            Players = players;
            Health = 15;
            CanAttack = true;
            CurrentTarget = Players[0];
            MaxForce = MaxForce * 1.25f;
            MaxVelocity = MaxVelocity * 1.25f;
        }
        /// <summary>
        /// Calls Pursuit then updates
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            Pursuit(CurrentTarget);
            base.Update(gameTime);
        }



    }
}

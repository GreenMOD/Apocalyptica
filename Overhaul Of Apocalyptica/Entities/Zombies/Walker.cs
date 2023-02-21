using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Overhaul_Of_Apocalyptica.Entities;
using Overhaul_Of_Apocalyptica.Entities.Characters;
using System.Linq;

namespace Overhaul_Of_Apocalyptica.Entities
{
    class Walker : Zombie
    {
        protected Rectangle frame1 = new Rectangle(0, 0, 17, 39);  // left
        protected Rectangle frame2 = new Rectangle(19, 0, 17, 39);  // right
     
        public Walker(Texture2D texture2D, EntityManager entityManager, Vector2 spawnLocation,List<Player> players )
        {
            Position = spawnLocation;
            ZombieSprite = new Sprite(texture2D, new List<Rectangle>() {frame1, frame2 }, Position);
            CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, ZombieSprite.Source.Width *2, ZombieSprite.Source.Height*2);
            Players = players;
            Health = 100;
            CanAttack = true;
            CurrentTarget = Players[0];
        }

        public override void Update(GameTime gameTime)
        {
            Pursuit(CurrentTarget);
            base.Update(gameTime);
        }



    }
}

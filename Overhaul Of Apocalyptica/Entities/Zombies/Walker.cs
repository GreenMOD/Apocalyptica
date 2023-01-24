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
        public override Vector2 Position { get; set; }

        protected Rectangle frame1 = new Rectangle(0, 0, 17, 39);  // left
        protected Rectangle frame2 = new Rectangle(19, 0, 17, 39);  // right
       
       
        protected Rectangle altLeft = new Rectangle(94, 0, 23, 42); //alternate left
        protected Rectangle altRight = new Rectangle(94, 0, 23, 42); //alternate right
        protected Rectangle altUp = new Rectangle(94, 0, 23, 42); //alternate up
        protected Rectangle altDown = new Rectangle(94, 0, 23, 42); //alternate down

        public Walker(Texture2D texture2D, EntityManager entityManager, Vector2 spawnLocation,List<Player> players )
        {
            Position = spawnLocation;
            ZombieSprite = new Sprite(texture2D, new List<Rectangle>() {frame1, frame2 }, Position);
            CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, ZombieSprite.Source.Width, ZombieSprite.Source.Height);
            Players = players;
        }
        public override void Collided(GameTime gameTime)
        {

            ////if (CollisionBox.Intersects(CurrentTarget.CollisionBox))
            ////{
            ////    if (gameTime.TotalGameTime.Seconds - _timeOfLastAttack >= AttackCooldown)
            ////    {
            ////        CurrentTarget.Health -= 5;
            ////        _timeOfLastAttack = gameTime.TotalGameTime.TotalSeconds;
            ////    }

            ////}

            ////foreach (Zombie z in _zombiesInView)
            ////{
            ////    float distance = Vector2.Distance(Position, z.Position);

            ////    if ((distance > 0) && (distance < 20))//20 pixels
            ////    {
            ////        Separate(z.Position);
            ////    }
            ////}

        }

    }
}

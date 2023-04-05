using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using Overhaul_Of_Apocalyptica.Entities.Projectiles;

namespace Overhaul_Of_Apocalyptica.Entities.Zombies
{
    class Screamer : Zombie
    { 

        public Vector2 Destination { get; set; }
        bool reachedDestination = true;

        protected Rectangle frame1 = new Rectangle(0, 40, 20, 20);  // left
        protected Rectangle frame2 = new Rectangle(22, 40, 20, 20);  // right

        private EntityManager _entityManager;

        public Screamer(Texture2D texture2D, Vector2 spawnLocation,EntityManager entityManager)
        {
           

            ZombieSprite = new Sprite(texture2D, new List<Rectangle>() {frame1,frame2 }, Position);
            Position = spawnLocation;
            CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, ZombieSprite.Source.Width, ZombieSprite.Source.Height);
            _entityManager = entityManager; //assigned an entitymanager to allow for it to track any enitiy as its target
            Health = 5;
            
        }
        /// <summary>
        /// Determines if a swarmbomb has exploded and then decideds how to move
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            
            
            List<SwarmBomb> swarmBombs =_entityManager.GetEntities<SwarmBomb>().ToList<SwarmBomb>();

            //checks position of all swarmbombs in entitymanager. If it finds one that is destoryed it will set its desination to be there
            foreach (SwarmBomb s in swarmBombs)
            {
                if (s.IsDestroyed)
                {
                    Destination = s.Position;
                    reachedDestination = false;
                }
                    
            }
            if ((reachedDestination == true))
            {
                Random rng = new Random();
                Destination = new Vector2((float)rng.Next(0, 800), (float)rng.Next(0, 480));
                reachedDestination = false;
            }
            else 
            {
                Seek(Destination);
            }
            if (Position == Destination)
            {
                reachedDestination = true;
            }
            base.Update(gameTime);

        }

    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Overhaul_Of_Apocalyptica.Entities;
using Overhaul_Of_Apocalyptica.Entities.Characters;
using Overhaul_Of_Apocalyptica.Entities.Projectiles;

namespace Overhaul_Of_Apocalyptica.Entities.Zombies
{
    class Captain : Zombie
    {
        

        protected Rectangle frame1 = new Rectangle(0, 62, 19, 33);  // left
        protected Rectangle frame2 = new Rectangle(21, 62, 19, 33);  // right
        protected Rectangle rocketSource = new Rectangle(0,0 , 5, 3);
        protected Rectangle swarmBombSource = new Rectangle(6,0 , 5, 3);
        //List of all ISwarmable Zombies or a Swarm Manager
        private List<Projectile> Rockets;

        private Texture2D _rocketProjectile;

        private EntityManager _entityManager;
        
        private CollisionManager _collisionManager;

        public const double ROCKET_COOLDOWN = 3.5;
        public const double SWARM_BOMB_COOLDOWN = 10;
        public double _timeSinceLastRocket = 0;
        public double _timeSinceLastSwarmBomb = 0;

        public Captain(Texture2D captainTexture, Vector2 spawnLocation, EntityManager entityManager, CollisionManager collisionManager,Texture2D projectile) 
        {

            _entityManager = entityManager;

            _collisionManager= collisionManager;

            ZombieSprite = new Sprite(captainTexture, new List<Rectangle>() { frame1,frame2}, Position);

            _rocketProjectile = projectile;

            Position = spawnLocation;
            Rockets = new List<Projectile>();

            _entityManager = entityManager; //assigned an entitymanager to allow for it to track any enitiy as its target
            List<Player> players = _entityManager.GetEntities<Player>().ToList(); //creates a list of all players from a IEnumerable
            CurrentTarget = players[0];
            foreach (Player n in players)
            {
                if ((CurrentTarget.Position - Position).Length() > (n.Position - Position).Length())
                {
                    CurrentTarget = n;
                }
            }
            Health = 50;
        }

        /// <summary>
        /// Moves away from the current target
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
           

           
            Flee(CurrentTarget.Position);
            Fire(gameTime);



            List<Projectile> rocketsToRemove = new List<Projectile>();
            if (Health == 0)
            {
                foreach (Projectile r in Rockets)
                {
                    if (!r.IsDestroyed)
                    {
                        r.IsDestroyed = true;
                    }
                }
            }
            foreach (Projectile item in Rockets)
            {
                if (item.IsDestroyed)
                {
                    rocketsToRemove.Add(item);
                }
            }
            foreach (Projectile item in rocketsToRemove)
            {
                Rockets.Remove(item);
                _entityManager.RemoveEntity(item);
                _collisionManager.RemoveCollidable(item);
            }
            rocketsToRemove.Clear();
            base.Update(gameTime);


          


        }
        /// <summary>
        /// Checks if the zombie can fire then if it can adds a new instanse of a rocket/swarmbomb to the entity manager and collision manager
        /// </summary>
        /// <param name="gameTime"></param>
        public void Fire(GameTime gameTime)
        {
            if ((gameTime.TotalGameTime.TotalSeconds - _timeSinceLastSwarmBomb >= SWARM_BOMB_COOLDOWN))
            {
                _timeSinceLastSwarmBomb = gameTime.TotalGameTime.TotalSeconds;
                _timeSinceLastRocket = gameTime.TotalGameTime.TotalSeconds;
                Projectile swarmBomb = new SwarmBomb(_rocketProjectile, new List<Rectangle>() { swarmBombSource }, Position, CurrentTarget, gameTime);

                Rockets.Add(swarmBomb);

                _entityManager.AddEntity(swarmBomb);

                _collisionManager.AddCollidable(swarmBomb);

            }
            else if ((gameTime.TotalGameTime.TotalSeconds - _timeSinceLastRocket >= ROCKET_COOLDOWN))
            {
                _timeSinceLastRocket = gameTime.TotalGameTime.TotalSeconds;
                Projectile seeker = new Rocket(_rocketProjectile, new List<Rectangle>() { rocketSource }, Position, CurrentTarget, gameTime);

                Rockets.Add(seeker);

                _entityManager.AddEntity(seeker);

                _collisionManager.AddCollidable(seeker);
            }
        }
    }

}



using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Overhaul_Of_Apocalyptica.Entities;

namespace Overhaul_Of_Apocalyptica.Entities.Zombies
{
    class Captain : Zombie, ICollidable
    {
        public override Vector2 Position { get; set; }


        protected Rectangle frame1 = new Rectangle(0, 62, 19, 33);  // left
        protected Rectangle frame2 = new Rectangle(21, 62, 19, 33);  // right
        protected Rectangle rocketSource = new Rectangle(0,0 , 5, 3);
        protected Rectangle swarmBombSource = new Rectangle(6,0 , 5, 3);
        //List of all ISwarmable Zombies or a Swarm Manager
        private List<Projectile> Rockets;


        

        private int _distanceUntilEdgeX;
        private int _distanceUntilEdgeY;


        private Texture2D _rocketProjectile;



        public const double ROCKET_COOLDOWN = 2.5;
        public const double SWARM_BOMB_COOLDOWN = 10;
        public double _timeSinceLastRocket = 0;
        public double _timeSinceLastSwarmBomb = 0;

        public Captain(Texture2D texture2D, Vector2 spawnLocation, EntityManager entityManager,Texture2D projectile)
        {

            _entityManager = entityManager;

            maxForce = maxForce ;
            maxVelocity = maxVelocity ;

            _frames.Add(frame1);
            _frames.Add(frame2);
            _sprite = new Sprite(texture2D, _frames, Position);

            _entityManager = entityManager; //assigned an entitymanager to allow for it to track any enitiy as its target
            _ninja = (Ninja)_entityManager.GetEntities<Ninja>();
            _texture2D = texture2D;
            _rocketProjectile = projectile;

            

            Position = new Vector2(650,250);
            Rockets = new List<Projectile>();
            _distanceUntilEdgeX = (800 - (int)Position.X);
            _distanceUntilEdgeY = (480 - (int)Position.Y);

        }

        /// <summary>
        /// Moves away from the current target
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        { //TODO Problem 1 I cannot keep the captian zombie inside the GameWindow Bounds. The code below only stops them at the border


           
            Flee(_ninja.Position);
            if (Position.X > 775)
            {
                ApplyOffset();
                if (Position.X > 775)
                {
                    Position = new Vector2(775f, Position.Y);
                }


            }
            else if (Position.X < 25)
            {
                ApplyOffset();
                if (Position.X < 25)
                {
                    Position = new Vector2(25f, Position.Y);
                }

            }
            if (Position.Y > 440)
            {
                ApplyOffset();
                if (Position.Y > 440)
                {
                    Position = new Vector2(Position.X, 440);
                }
                
            }
            else if (Position.Y < 45)
            {
                ApplyOffset();
                if (Position.Y < 45)
                {
                    Position = new Vector2(Position.X, 45);
                }
               
            }
            else
            {
                Flee(_ninja.Position);
            }
            Speed = Vector2.Add(Speed, _acceleration); //applyies a movement froce
            if (Speed.Length() > maxVelocity) // limits the velocity to under the maximum velocity
            {
                Speed = Vector2.Normalize(Speed) * maxVelocity;

            }
            Position = Vector2.Add(Position, Speed); // applies the velocity to the position allowing for movement
            _acceleration = Vector2.Multiply(_acceleration, 0); // resets acceleration in order to not have exponential growth

            if (Position.Y < 45)
            {
                Position = new Vector2(Position.X, 45);
            }
            _sprite.Update(gameTime, Position);


            //Checks whether a rocket can be fired and then fires it

            if ((gameTime.TotalGameTime.TotalSeconds - _timeSinceLastSwarmBomb >= SWARM_BOMB_COOLDOWN) ^ (_timeSinceLastSwarmBomb == 0))
            {
                _timeSinceLastSwarmBomb = gameTime.TotalGameTime.TotalSeconds;
                _timeSinceLastRocket = gameTime.TotalGameTime.TotalSeconds;
                Fire(gameTime, new SwarmBomb(_rocketProjectile, new List<Rectangle>() { swarmBombSource }, Position, _ninja, gameTime));
            }
            else if ((gameTime.TotalGameTime.TotalSeconds - _timeSinceLastRocket >= ROCKET_COOLDOWN) ^ (_timeSinceLastRocket == 0))
            {
                _timeSinceLastRocket = gameTime.TotalGameTime.TotalSeconds;
                Fire(gameTime, new Rocket(_rocketProjectile, new List<Rectangle>() { rocketSource }, Position, _ninja, gameTime));

            }
          
            
            ///updates all rockets that are currently fire
            if (Rockets.Count != 0)
            {
                //foreach (var item in Rockets)
                //{
                //    if (item.IsDestroyed == true)
                //    {
                //        Rockets.Remove(item);
                //        _entityManager.RemoveEntity(item);
                //    }
                //    else
                //    {
                //        item.Update(gameTime);
                //    }
                   
                //}
                for (int i = 0; i < Rockets.Count; i++)
                {
                    Projectile item = Rockets[i];
                    if (item.IsDestroyed)
                    {
                        Rockets.Remove(item);
                        _entityManager.RemoveEntity(item);
                    }
                }
            }





            if (MagnitudePos(Speed.X, Speed.Y) > 0)
            {
                _zombieFacing = "left";
            }
            else
            {
                _zombieFacing = "right";
            }


        }/// <summary>
         /// Normal Seeking Algorithm using Reynolds except the vector is inverted in order to flee
         /// </summary>
         /// <param name="target"></param>

         /// <summary>
         /// Creates an new rocketSource that seeks the current target
         /// </summary>
        public void Fire(GameTime gameTime, Projectile projectile)
        {
            Rockets.Add(projectile);

            _entityManager.AddEntity(projectile);
        }
        public void ApplyOffset()
        {
            //    ///Modelling the offset vector as hyperbolic tangent will allow a steady increase of speed for attacking
            //    ///With range of -2  ------ 2 each degree it goes up the vector of attack will be increase and each degree it goes down it will become 


            _distanceUntilEdgeX = (800 - (int)Position.X); //
            _distanceUntilEdgeY = (480 - (int)Position.Y);


            //    //every 10 pixels the closer it gets the harder the force is applyed

            int forceMultiplerX = _distanceUntilEdgeX / 10;
            int forceMultiplerY = _distanceUntilEdgeY / 10;


            float offsetX = forceMultiplerX * Vector2.Negate(Speed).X; //a multiplier for the amount of force added to a specific direction is added to this as it geths closer to the wall
            float offsetY = forceMultiplerY * Vector2.Negate(Speed).Y; //a multiplier for the amount of force added to a specific direction is added to this as it geths closer to the wall
            Vector2 counterVector = new Vector2(offsetX, offsetY);
            Vector2 steer = Vector2.Subtract(Speed, counterVector);
            //ApplyForce(steer);
            Speed = Vector2.Add(Speed, _acceleration); //applyies a movement froce
            if (Speed.Length() > maxVelocity) // limits the velocity to under the maximum velocity
            {
                Speed = Vector2.Normalize(Speed) * maxVelocity;

            }
            Position = Vector2.Add(Position, Speed); // applies the velocity to the position allowing for movement
            _acceleration = Vector2.Multiply(_acceleration, 0); // resets acceleration in order to not have exponential growth

            //    ///TWo vectors Attack and flee tune each to situations more to attack if its gets closer to wall 
            //    ///hyperbolic tanch  

            //    Vector2 attackVector;
            //    Vector2 fleeVector;








            //}

            //public void MovementSelection(int hyperbolicrange)
            //{
            //    float tuningFactor = 1.25f;
            //    switch (HyperbolicRange)
            //    {
            //        case 2:
            //            Seek(_ninja.Position / tuningFactor);
            //            break;
            //        case 1:
            //            Seek(_ninja.Position / tuningFactor);
            //            break;
            //        case 0:
            //            Speed = Vector2.Zero;
            //            break;
            //        case -1:
            //            Flee(_ninja.Position * tuningFactor);
            //            break;
            //        case -2:
            //            Flee(_ninja.Position * tuningFactor);
            //            break;
            //    }
            //}




        }
        
    }
}


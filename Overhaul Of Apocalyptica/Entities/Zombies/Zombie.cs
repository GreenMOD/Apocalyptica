using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Overhaul_Of_Apocalyptica.Entities;
using Overhaul_Of_Apocalyptica.Entities.Characters;


namespace Overhaul_Of_Apocalyptica.Entities
{
    abstract class Zombie :IEntity,ICollidable
    {
        private Vector2 _acceleration;

        private float _maxVelocity = 0.75f;

        private float _maxForce = 0.5f;

        private Sprite _sprite;

        private double _attackCooldown = 2.5f;

        public abstract Vector2 Position { get; set; }
        public List<Player> Players { get; set; }
        public Player CurrentTarget { get; set; }
        public double Health { get; set; }
        public double Armour { get; set; }
        public Vector2 Speed { get; set; }
        public Rectangle CollisionBox { get; set; }
        public string Status { get; set; }
        public string ZombieFacing { get; set; }

        public Vector2 Acceleration { get { return _acceleration; } set { _acceleration = value; } }
       

        public float MaxVelocity { get { return _maxVelocity; } set { _maxVelocity = value; } }


        public float MaxForce { get { return _maxForce; } set { _maxForce = value; } }


        public Sprite ZombieSprite{ get { return _sprite; } set {_sprite = value; } }

        public double AttackCooldown { get { return _attackCooldown; }set{ _attackCooldown = value; } }

        public bool isSeparating { get; set; }

        private Random _rng = new Random();

        public virtual void Update(GameTime gameTime)
        {
          
            if (isSeparating == true)
            {
                Speed = Vector2.Add(Speed, Acceleration); //applyies a movement froce
                if (Speed.Length() > MaxVelocity) // limits the velocity to under the maximum velocity
                {
                    Speed = Vector2.Normalize(Speed) * MaxVelocity;

                }
                Position = Vector2.Add(Position, Speed); // applies the velocity to the position allowing for movement
                Acceleration = Vector2.Multiply(Acceleration, 0); // resets acceleration in order to not have exponential growth
                isSeparating = false;
            }

            _sprite.Update(gameTime, Position);
            CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, _sprite.Source.Width, _sprite.Source.Height);



            if (Speed.X >0)
            {
                ZombieFacing = "right";
            }
            else if (Speed.X < 0)
            {
                ZombieFacing = "left";
            }
        }


        public virtual void Flee(Vector2 target)
        {
            

            Vector2 desired = Vector2.Subtract(target, Position); //creates the desired path towards the enemy
            desired.Normalize();
            desired = Vector2.Multiply(desired, MaxVelocity);


            Vector2 steer = Vector2.Negate(desired);
            if (steer.Length() > MaxForce)
            {
                steer = Vector2.Normalize(steer) * MaxForce; //limits the steering force to maxforce
            }
            ApplyForce(steer);
              

        }
        public virtual void Seek(Vector2 target)
        {
            Vector2 desired = Vector2.Subtract(target, Position); //creates the desired path towards the enemy
            desired.Normalize();
            desired = Vector2.Multiply(desired, MaxVelocity);

            Vector2 steer = Vector2.Subtract(desired, Speed); //Reynold's formula for calculating steering 

            if (steer.Length() > MaxForce)
            {
                steer = Vector2.Normalize(steer) * MaxForce; //limits the steering force to maxforce
            }
            ApplyForce(steer);


            Speed = Vector2.Add(Speed, Acceleration); //applyies a movement froce 
            if (Speed.Length() > MaxVelocity) // limits the velocity to under the maximum velocity
            {
                Speed = Vector2.Normalize(Speed) * MaxVelocity;

            }
            Position = Vector2.Add(Position, Speed); // applies the velocity to the position allowing for movement
            Acceleration = Vector2.Multiply(Acceleration, 0); // resets acceleration in order to not have exponential growth

            Arrive(target);
        }


        public virtual void ApplyForce(Vector2 force)
        {
            Acceleration = Vector2.Add(Acceleration, force);
        }
        /// <summary>
        /// Calculates whether to decrease the velocity of the vehicle given by a set distance.
        /// </summary>
        /// <param name="target"></param>
        public virtual void Arrive(Vector2 target)
        {
            Vector2 desired = Vector2.Subtract(target, Position);
            float d = MagnitudePos((int)desired.X, (int)desired.Y);
            ///in order to scale speed for the zombie we have to map out a circle around it
            ///Think of this as a sort of collision box which allows the vehicle, in this case the said circle has a radius of 100
            ///If the vehicle is outside the circle its velocity is not effected
            ///If it is inside the circle, the deceleration rate scales with how far the vehicle is inside the circle
            if (d < 100)
            {
                Speed = Vector2.Multiply(Speed, (d / 100)); // to balance how much force they are being subtracted .... (d/100)*2) this will decrease the distance at which they slow and arrive
            }

            //by multiplying the velocity of the vehicle by the magnitude of the desired vector / 100 I can calculate the % at which to divide by resulting in a complete stop
        }
        /// <summary>
        /// Predicts using the current velocity of the player 
        /// </summary>
        /// <param name="target"></param>
        public virtual void Pursuit(Player target) // TODO Implement PC
        {
            float predictFactor = 10f;
            //Number of cycles it will take to intercept the vehicle
            if (target.Speed != Vector2.Zero)
            {
                Vector2 futurePosition = target.Position + target.Speed * predictFactor;

                Seek(futurePosition);
            }
            else
            {
                Seek(target.Position);
            }

        }
        public virtual float MagnitudePos(float Vector2X, float Vector2Y)
        {
            float mag = 0;
            Vector2X = Vector2X * Vector2X;
            Vector2Y = Vector2Y * Vector2Y;
            mag = Vector2X + Vector2Y;
            mag = (int)Math.Sqrt(mag);
            return mag;
        }



        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _sprite.Draw(spriteBatch, gameTime, ZombieFacing, 2f);
        }

        
        public virtual void Collided(GameTime gameTime)
        {
           
        }
        public virtual void Separate(Vector2 target)
        {
            /// AIM to avoid the other zombie
            /// HOW by placing a small force on the zombie that will only be taken off when it is out of range of the zombie
            /// Luckly Vectors can be added together to produce a resultant hence i can place many vectors into a single 
            /// IMPLEMENTATION Bool if on Separate runs and applies the force after movement.

            Vector2 desiredSeparation = (Vector2.Subtract(Position, target));
            desiredSeparation.Normalize();
            desiredSeparation = Vector2.Multiply(desiredSeparation, MaxVelocity);
            
            ApplyForce(desiredSeparation);

        }
        public virtual void Idle()
        {
            Vector2 desired = Vector2.Subtract(new Vector2((float)_rng.Next(0, 800), (float)_rng.Next(0, 480)),Position);
            desired = Vector2.Multiply(desired, MaxForce);

            Vector2 steer = Vector2.Subtract(desired, Position);
            steer.Normalize();

            if (steer.Length() > MaxForce)
            {
                steer = Vector2.Normalize(steer) * MaxForce; //limits the steering force to maxforce
            }
            ApplyForce(steer);

            Speed = Vector2.Add(Speed, Acceleration); //applyies a movement froce 
            if (Speed.Length() > MaxVelocity) // limits the velocity to under the maximum velocity
            {
                Speed = Vector2.Normalize(Speed) * MaxVelocity;

            }
            Position = Vector2.Add(Position, Speed); // applies the velocity to the position allowing for movement
            Acceleration = Vector2.Multiply(Acceleration, 0); // resets acceleration in order to not have exponential growth
        }

    }
}
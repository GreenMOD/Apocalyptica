using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Overhaul_Of_Apocalyptica.Entities.Characters;

namespace Overhaul_Of_Apocalyptica.Entities
{
    abstract class Zombie :IEntity,ICollidable
    {
        #region Declarations

        private Vector2 _position = Vector2.Zero;

        private Vector2 _acceleration = Vector2.Zero;

        private Vector2 _speed = Vector2.Zero;

        private int _health = 100;

        private float _maxVelocity = 0.75f;

        private float _maxForce = 0.5f;

        private Sprite _sprite;

        private double _attackCooldown = 1f;

        private float _timeLastAttack = -1;

        private GameTime _gameTime = new GameTime();

        private List<Player> _players= new List<Player>();

        private Player _currentTarget;

        private Rectangle _collisionBox = new Rectangle();

        private string _facing = "";

        private float _minimumX = 5f;

        private float _maximumX = 755f;

        private float _minimumY = 5f;

        private float _maximumY = 445f;

        private bool isSeparating = false;
        #endregion

        #region Properties
        public virtual Vector2 Position { get { return _position; } set { _position = new Vector2(MathHelper.Clamp(value.X, _minimumX + CollisionBox.Width/2, _maximumX - CollisionBox.Width/2), (MathHelper.Clamp(value.Y, _minimumY + CollisionBox.Height/2, _maximumY - CollisionBox.Height/2))); } }

        public Vector2 Acceleration { get { return _acceleration; } set { _acceleration = value; } }

        public virtual Vector2 Speed { get { return _speed; } set { _speed = value; } }

        public virtual int Health { get { return _health; } set { _health = value; } }

        public float MaxVelocity { get { return _maxVelocity; } set { _maxVelocity = value; } }

        public float MaxForce { get { return _maxForce; } set { _maxForce = value; } }

        public Sprite ZombieSprite { get { return _sprite; } set { _sprite = value; } }

        public virtual bool CanAttack { get { return _attackCooldown < _gameTime.TotalGameTime.TotalSeconds - _timeLastAttack; } set { _timeLastAttack = (float)_gameTime.TotalGameTime.TotalSeconds; } }

        public virtual List<Player> Players { get { return _players; } set { _players = value; } }

        public virtual Player CurrentTarget { get { return _currentTarget; } set { _currentTarget = value; } }

        public Rectangle CollisionBox { get { return _collisionBox; } set { _collisionBox = value; } }

        public string ZombieFacing { get { return _facing; } set { _facing = value; } }

        #endregion

        #region Methods
        /// <summary>
        /// Updates the current speed by adding acceleeration and calculates movement for the Zombie. 
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            _gameTime = gameTime;

            Speed = Vector2.Add(Speed, Acceleration); //applyies a movement froce

          
            if (Speed.Length() > MaxVelocity) // limits the velocity to under the maximum velocity
            {
                Speed = Vector2.Normalize(Speed) * MaxVelocity;
            }
            Position = Vector2.Add(Position, Speed); // applies the velocity to the position allowing for movement
            Acceleration = Vector2.Multiply(Acceleration, 0); // resets acceleration in order to not have exponential growth

            ZombieSprite.Update(gameTime, Position);
            CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, _sprite.Source.Width*2, _sprite.Source.Height * 2);
        }

        /// <summary>
        /// Using reynolds formula, a force vector is created that steers the vehicle away from the target position
        /// </summary>
        /// <param name="target"></param>
        public virtual void Flee(Vector2 target)
        {

            Vector2 desired = Vector2.Subtract(target, Position);
            desired.Normalize();
            desired = Vector2.Multiply(desired, MaxVelocity);


            desired = Vector2.Negate(desired);

            Vector2 steer = Vector2.Add(desired, Speed);
            if (steer.Length() > MaxForce)
            {
                steer = Vector2.Multiply(steer, MaxForce);
            }

            ApplyForce(steer);

        }
        /// <summary>
        /// Using Reynolds formula, a force vector is created which moves the vehicle towards the target position
        /// </summary>
        /// <param name="target"></param>
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

            Arrive(target);
        }

        /// <summary>
        /// Applies a force vector to the Acceleration varaible
        /// </summary>
        /// <param name="force"></param>
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
                Speed = Vector2.Multiply(Speed, (d / 100)); 
            }

            //by multiplying the velocity of the vehicle by the magnitude of the desired vector / 100 I can calculate the % at which to divide by resulting in a complete stop
        }

        /// <summary>
        /// Predicts using the current velocity of the player 
        /// </summary>
        /// <param name="target"></param>
        public virtual void Pursuit(Player target) 
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
        /// <summary>
        /// Calculates the modulus of an x and Y value
        /// </summary>
        /// <param name="Vector2X"></param>
        /// <param name="Vector2Y"></param>
        /// <returns></returns>
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
            if (Health >0)
            {
                _sprite.Draw(spriteBatch, gameTime, 2f);
            }
        }
        /// <summary>
        /// Prevents zombies colliding with each other by applying a force
        /// </summary>
        /// <param name="collisionBox"></param>
        public virtual void Separate(Rectangle collisionBox)
        {
            /// AIM to avoid the other zombie
            /// HOW by placing a small force on the zombie that will only be taken off when it is out of range of the zombie
            /// Luckly Vectors can be added together to produce a resultant hence i can place many vectors into a single 

          
            Rectangle intercept = Rectangle.Intersect(CollisionBox, collisionBox);

            Vector2 pos = new Vector2((int)Position.X, (int)Position.Y);

            if (pos == new Vector2(intercept.X, intercept.Y))
            {
                if (intercept.Width > intercept.Height)
                {
                    Position = Vector2.Add(Position, new Vector2(0, intercept.Height));
                }
                else
                {
                    Position = Vector2.Add(Position, new Vector2(intercept.Width, 0));
                }

            }
            else if (intercept.Contains(pos.X + CollisionBox.Width, 0))
            {
                Position = Vector2.Add(Position, new Vector2(-intercept.Width, 0));
            }


        }
        /// <summary>
        /// Seeks a random position
        /// </summary>
        /// <param name="randomPos"></param>
        public virtual void Idle(Vector2 randomPos)
        {
            Vector2 desired = Vector2.Subtract(randomPos,Position);
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
        /// <summary>
        /// Extracts the name of the object collided with and uses this to decide how to react
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="collidedWith"></param>
        public virtual void Collided(GameTime gameTime, ICollidable collidedWith)
        {
            switch (collidedWith.GetType().Name)
            {
                case "Bullet":
                    Health = Health - 5;
                    break;
                case "Shuriken":
                    Health = Health - 5;
                    break;
                case "Walker":
                    Separate(collidedWith.CollisionBox);
                    break;
                case "Screamer":
                    Separate(collidedWith.CollisionBox);
                    break;
                case "Captain":
                    Separate(collidedWith.CollisionBox);
                    break;
            }
        }
        #endregion
    }
}
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Overhaul_Of_Apocalyptica.Entities.Characters;

namespace Overhaul_Of_Apocalyptica.Entities.Projectiles
{
    class Rocket : Projectile
    {
        Vector2 posTarget;
        const double FLIGHT_MAXIMUM = 1.5;
        public Rocket(Texture2D texture, List<Rectangle> frames, Vector2 start, Player target, GameTime gameTime) //One direction
        {
            Position = start;
            Frames = frames;
            ProjectSprite = new Sprite(texture, Frames, Position);
            Target = target;
            CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, Frames[0].Width, Frames[0].Height);
            FlightTime = gameTime.TotalGameTime.TotalSeconds;
            MaxVelocity = 4f;
            MaxForce = 4.15f;
            IsDestroyed= false;
        }
        /// <summary>
        /// If the rocket is not destroyed Flight is called and the Sprite and collision box is updated
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            
            if (IsDestroyed == false)
            {

                Flight(gameTime);
                ProjectSprite.Update(gameTime,Position); 
                CollisionBox = new Rectangle((int)Position.X, (int)Position.Y,(int) Frames[0].Width * 2,(int)Frames[0].Height * 2); //due to the frame being smaller than desired the collsion box height and width are multipled by 2
            }
        }
        /// <summary>
        /// Rocket is drawn at 2 times its usual size due to its texture being too small
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            ProjectSprite.Draw(spriteBatch, gameTime,2f);
        }
        /// <summary>
        /// Using the name of the object collided with this subroutine decides whether this rocket is destoryed or not
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="collidedWith"></param>
        public override void Collided(GameTime gameTime , ICollidable collidedWith)
        {
           
            if(collidedWith.GetType().FullName == Target.GetType().FullName)
            {
                IsDestroyed = true;
                
            }
        }
        /// <summary>
        /// Uses the seek steering behavoiur to seek the player but will be destoryed if its FlightTime passes the FLIGHT_MAXIMUM constant
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Flight(GameTime gameTime)
        {
            posTarget = Target.Position;
            if (gameTime.TotalGameTime.TotalSeconds - FlightTime >= FLIGHT_MAXIMUM)
            {
                IsDestroyed = true;
            }
            else
            {

                Vector2 desired = Vector2.Subtract(posTarget, Position); //creates the desired path towards the enemy
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


            }
        }




    }
}

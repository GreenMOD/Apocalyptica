﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Overhaul_Of_Apocalyptica.Entities.Characters;
using System.Diagnostics;

namespace Overhaul_Of_Apocalyptica.Entities.Projectiles
{/// <summary>
/// This is a rocket that doesn't cause Zombies to swarm
/// </summary>
    class Rocket : Projectile
    {
        Vector2 posTarget;
        const double FLIGHT_MAXIMUM = 1.5;
        public Rocket(Texture2D texture, List<Rectangle> frames, Vector2 start, Player target, GameTime gameTime) //One direction
        {
            Position = start;
            Frames = frames;
            ProjectSprite = new Sprite(texture, Frames, Position);
            _target = target;
            CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, Frames[0].Width, Frames[0].Height);
            FlightTime = gameTime.TotalGameTime.TotalSeconds;
            MaxVelocity = 4f;
            MaxForce = 4.15f;
            IsDestroyed= false;
        }

        public override void Update(GameTime gameTime)
        {
            
            if (IsDestroyed == false)
            {
                
                Flight(gameTime);
                ProjectSprite.Update(gameTime,Position); 
                CollisionBox = new Rectangle((int)Position.X, (int)Position.Y,(int) Frames[0].Width * 2,(int)Frames[0].Height * 2);
            }
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            ProjectSprite.Draw(spriteBatch, gameTime,2f);
        }
        public override void Collided(GameTime gameTime , ICollidable collidedWith)
        {
           
            if(collidedWith.GetType().FullName == _target.GetType().FullName)
            {
                IsDestroyed = true;
                
            }
        }
        public override void Flight(GameTime gameTime)
        {
            posTarget = _target.Position;
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

                double toFindAngle = Position.Y / Position.X; //In order to work out the angle I have to take the arctan of the x value and the y value this then produces the angle that the rocketSource must turn
                ProjectSprite.Rotation = (float)Math.Atan(toFindAngle)/ 360;  
                
                //TODO Projectile either spins constantly or it doesn't change


            }
        }




    }
}

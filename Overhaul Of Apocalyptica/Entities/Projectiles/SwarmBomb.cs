﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;
using Overhaul_Of_Apocalyptica.Entities.Characters;

namespace Overhaul_Of_Apocalyptica.Entities
{
    class SwarmBomb : Projectile
    {
        Vector2 posTarget;
        const double FLIGHT_MAXIMUM = 2.5;
        public SwarmBomb(Texture2D texture, List<Rectangle> frames, Vector2 start, Player target,GameTime gameTime)
        {
            Position = start;
            _frames = frames;
            _sprite = new Sprite(texture, _frames, Position);
            _target = target;
            CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, _frames[0].Width, _frames[0].Height);
            _timeFire = gameTime.TotalGameTime.TotalSeconds;
        }
        public override void CheckCollision(GameTime gameTime)
        {
            if (CollisionBox.Intersects(_target.CollisionBox))
            {
                _target.Health -= 10;
                IsDestroyed = true;
                //swarm
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _sprite.Draw(spriteBatch, gameTime, 2.5f);
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
                desired = Vector2.Multiply(desired, maxVelocity);

                Vector2 steer = Vector2.Subtract(desired, Speed); //Reynold's formula for calculating steering 

                if (steer.Length() > maxForce)
                {
                    steer = Vector2.Normalize(steer) * maxForce; //limits the steering force to maxforce
                }
                ApplyForce(steer);
                Speed = Vector2.Add(Speed, _acceleration); //applyies a movement froce
                if (Speed.Length() > maxVelocity) // limits the velocity to under the maximum velocity
                {
                    Speed = Vector2.Normalize(Speed) * maxVelocity;

                }
                Position = Vector2.Add(Position, Speed); // applies the velocity to the position allowing for movement
                _acceleration = Vector2.Multiply(_acceleration, 0); // resets acceleration in order to not have exponential growth

                double toFindAngle = Position.Y / Position.X; //In order to work out the angle I have to take the arctan of the x value and the y value this then produces the angle that the rocketSource must turn
                _sprite.Rotation = (float)Math.Atan(toFindAngle) / 360;

                //TODO Projectile either spins constantly or it doesn't change


            }
        }

        public override void Update(GameTime gameTime)
        {

            if (IsDestroyed == false)
            {
                FlightTime = gameTime.TotalGameTime.TotalSeconds;
                Flight(gameTime);
                _sprite.Update(gameTime, Position);
                CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, _frames[0].Width, _frames[0].Height);
                CheckCollision(gameTime);
            }
        }
    }
}

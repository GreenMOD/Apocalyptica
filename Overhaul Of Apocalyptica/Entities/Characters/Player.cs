﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Overhaul_Of_Apocalyptica.Entities;

namespace Overhaul_Of_Apocalyptica.Entities.Characters
{
    public abstract class Player : IEntity, ICollidable
    {
        public abstract Vector2 Position { get; set; }

        public abstract Vector2 Speed { get; set; }

        public Rectangle CollisionBox { get; set; }
        
        public abstract float MovementSpeed { get; set; }

        public abstract int Health { get; set; }

        public abstract double Armour { get; set; }

        public abstract string Facing { get; set; }

        public abstract bool IsActive { get; set; }

       
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);

        public abstract void Update(GameTime gameTime);

        public void Activate()
        {
            IsActive = true;
        }
        public void Deactivate()
        {
            IsActive = false;
        }

        public void Movement(float runningSpeed)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W) && (Keyboard.GetState().IsKeyDown(Keys.A)))
            {
                Speed = Vector2.Zero;

                Facing = "left";


                Speed = new Vector2(-runningSpeed, -runningSpeed);

                Position = Vector2.Add(Speed, Position);

            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S) && (Keyboard.GetState().IsKeyDown(Keys.A)))
            {
                Speed = Vector2.Zero;
                Facing = "left";

                Speed = new Vector2(-runningSpeed, runningSpeed);

                Position = Vector2.Add(Speed, Position);

            }
            else if (Keyboard.GetState().IsKeyDown(Keys.W) && (Keyboard.GetState().IsKeyDown(Keys.D)))
            {
                Speed = Vector2.Zero;
                Facing = "right";

                Speed = new Vector2(runningSpeed, -runningSpeed);
                Position = Vector2.Add(Speed, Position);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S) && (Keyboard.GetState().IsKeyDown(Keys.D)))
            {
                Speed = Vector2.Zero;
                Facing = "right";

                Speed = new Vector2(runningSpeed, runningSpeed);
                Position = Vector2.Add(Speed, Position);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Speed = Vector2.Zero;
                Facing = "up";

                Speed = new Vector2(0, -runningSpeed);
                Position = Vector2.Add(Speed, Position);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Speed = Vector2.Zero;
                Facing = "down";

                Speed = new Vector2(0, runningSpeed);
                Position = Vector2.Add(Speed, Position);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Speed = Vector2.Zero;
                Facing = "left";

                Speed = new Vector2(-runningSpeed, 0);
                Position = Vector2.Add(Speed, Position);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Speed = Vector2.Zero;
                Facing = "right";

                Speed = new Vector2(runningSpeed, 0);
                Position = Vector2.Add(Speed, Position);
            }
            else
            {
                Speed = Vector2.Zero;
            }
          

        }
    }
}

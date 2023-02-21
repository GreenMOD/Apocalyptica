using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Overhaul_Of_Apocalyptica.Entities;
using Overhaul_Of_Apocalyptica.Sprites;
using Overhaul_Of_Apocalyptica.Entities.Weapons;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Overhaul_Of_Apocalyptica.Entities.Characters
{
    public abstract class Player : IEntity, ICollidable
    {

        #region Declarations

        private Vector2 _position = new Vector2();

        private Animation _currentAnimation;

        private List<Animation> _animations =new List<Animation>();

        #endregion
        public virtual Vector2 Position { get { return _position; } set { _position = new Vector2((float)MathHelper.Clamp(value.X, 45, 755), (float)MathHelper.Clamp(value.Y, 45, 435)); } }

        public abstract Vector2 Speed { get; set; }

        public Rectangle CollisionBox { get; set; }

        public abstract int Health { get; set; }
        public string Facing { get; set; }

        public abstract bool IsActive { get; set; }

        public abstract Gun Ranged { get; set; }

        public virtual List<Animation> Animations { get { return _animations; } set { _animations = value; } }

        public virtual Animation CurrentAnimation { get { return _currentAnimation; } set { _currentAnimation = value; } } 
        
       
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);

        public abstract void Update(GameTime gameTime);

        public abstract void PlayerInput(GameTime gameTime, KeyboardState currentStateKeys);

        public void Activate()
        {
            IsActive = true;
        }
        public void Deactivate()
        {
            IsActive = false;
            
        }

        public void Movement(float runningSpeed, KeyboardState keyboardState)
        {

            if (keyboardState.IsKeyDown(Keys.W) && (keyboardState.IsKeyDown(Keys.A)))
            {
                Speed = Vector2.Zero;

                Facing = "left";


                Speed = new Vector2(-runningSpeed, -runningSpeed);

                Position = Vector2.Add(Speed, Position);

            }
            else if (keyboardState.IsKeyDown(Keys.S) && (keyboardState.IsKeyDown(Keys.A)))
            {
                Speed = Vector2.Zero;
                Facing = "left";

                Speed = new Vector2(-runningSpeed, runningSpeed);

                Position = Vector2.Add(Speed, Position);

            }
            else if ((keyboardState.IsKeyDown(Keys.W) == true) && (keyboardState.IsKeyDown(Keys.D) == true))
            {
                Speed = Vector2.Zero;
                Facing = "right";

                Speed = new Vector2(runningSpeed, -runningSpeed);
                Position = Vector2.Add(Speed, Position);
            }
            else if (keyboardState.IsKeyDown(Keys.S) && (keyboardState.IsKeyDown(Keys.D)))
            {
                Speed = Vector2.Zero;
                Facing = "right";

                Speed = new Vector2(runningSpeed, runningSpeed);
                Position = Vector2.Add(Speed, Position);
            }
            else if (keyboardState.IsKeyDown(Keys.W))
            {
                Speed = Vector2.Zero;
                Facing = "up";

                Speed = new Vector2(0, -runningSpeed);
                Position = Vector2.Add(Speed, Position);
            }
            else if (keyboardState.IsKeyDown(Keys.S))
            {
                Speed = Vector2.Zero;
                Facing = "down";

                Speed = new Vector2(0, runningSpeed);
                Position = Vector2.Add(Speed, Position);
            }
            else if (keyboardState.IsKeyDown(Keys.A))
            {
                Speed = Vector2.Zero;
                Facing = "left";

                Speed = new Vector2(-runningSpeed, 0);
                Position = Vector2.Add(Speed, Position);
            }
            else if (keyboardState.IsKeyDown(Keys.D))
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
            Ranged.Position = Position;
            Ranged.Direction = Facing;
        }
        public void FireR(GameTime gameTime)
        {
            Ranged.Fire(gameTime);
            
        }


        public void Collided(GameTime gameTime, ICollidable collidedWith)
        {
            
            
            ///sort extraction of collidewith   
            switch (collidedWith.GetType().Name)
            {
                
                case "Walker":
                    if (collidedWith.GetType().GetProperty("CanAttack").GetValue(collidedWith).ToString() == "True")
                    {
                        Health = Health - 10;
                        collidedWith.GetType().GetProperty("CanAttack").SetValue(collidedWith, false);
                    }
                    break;
                case "Screamer":
                    if (collidedWith.GetType().GetProperty("CanAttack").GetValue(collidedWith).ToString() == "True")
                    {
                        Health = Health - 5;
                        collidedWith.GetType().GetProperty("CanAttack").SetValue(collidedWith, false);
                    }
                    break;

               
            }
        }
    }
}


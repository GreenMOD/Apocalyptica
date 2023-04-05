using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Overhaul_Of_Apocalyptica.Entities.Weapons;

namespace Overhaul_Of_Apocalyptica.Entities.Characters
{
    public abstract class Player : IEntity, ICollidable
    {

        #region Declarations

        private Vector2 _position = new Vector2(100,200);

        private List<Keys> _movementKeys = new List<Keys>
        {
             Keys.W,
             Keys.S,
             Keys.A,
             Keys.D
        };
        

        private int _health=0;

        private string _facing = ("");
        #endregion
        public virtual Vector2 Position { get { return _position; } set { _position = new Vector2((float)MathHelper.Clamp(value.X, 45, 755), (float)MathHelper.Clamp(value.Y, 45, 435)); } }

        public Vector2 Speed { get; set; }

        public Rectangle CollisionBox { get; set; }

        public int Health { get { return _health; } set { _health = value; } }
        public string Facing { get { return _facing; } set { _facing = value; } }

        public bool IsActive { get; set; }

        public Gun Ranged { get; set; }

        public abstract Sprite Sprite { get; set; }
        public abstract Heart Hearts { get; set; }

        public List<Keys> MovementKeys { get { return _movementKeys; } set { _movementKeys = value;  } }

        public Keys Up { get { return _movementKeys[0]; } }
        public Keys Down { get { return _movementKeys[1]; } }
        public Keys Left { get { return _movementKeys[2]; } }
        public Keys Right { get { return _movementKeys[3]; } }
        
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);

        public void Update(GameTime gameTime)
        {
            if (IsActive == true)
            {
                PlayerInput(gameTime, Keyboard.GetState());
                Ranged.Update(gameTime);
                Sprite.Update(gameTime);
                Hearts.Update(gameTime, Health);
                CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, Sprite.Source.Width, Sprite.Source.Height);

            }
        }

        public abstract void PlayerInput(GameTime gameTime, KeyboardState currentStateKeys);
        /// <summary>
        /// Activates and allows for updates
        /// </summary>
        public void Activate()
        {
            IsActive = true;
        }
        /// <summary>
        /// Prevents updates occuring
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
        }
        /// <summary>
        /// Calculates movement and adds it to position
        /// </summary>
        /// <param name="runningSpeed">how many pixels the character should move per cycle</param>
        /// <param name="keyboardState">Stores the keys that are being pressed</param>
        public void Movement(float runningSpeed, KeyboardState keyboardState)
        {

            if (keyboardState.IsKeyDown(Up) && (keyboardState.IsKeyDown(Left)))
            {
                Speed = Vector2.Zero;

                Facing = "left";


                Speed = new Vector2(-runningSpeed, -runningSpeed);

                Position = Vector2.Add(Speed, Position);

            }
            else if (keyboardState.IsKeyDown(Down) && (keyboardState.IsKeyDown(Left)))
            {
                Speed = Vector2.Zero;
                Facing = "left";

                Speed = new Vector2(-runningSpeed, runningSpeed);

                Position = Vector2.Add(Speed, Position);

            }
            else if ((keyboardState.IsKeyDown(Up)) && (keyboardState.IsKeyDown(Right)))
            {
                Speed = Vector2.Zero;
                Facing = "right";

                Speed = new Vector2(runningSpeed, -runningSpeed);
                Position = Vector2.Add(Speed, Position);
            }
            else if (keyboardState.IsKeyDown(Down) && (keyboardState.IsKeyDown(Right)))
            {
                Speed = Vector2.Zero;
                Facing = "right";

                Speed = new Vector2(runningSpeed, runningSpeed);
                Position = Vector2.Add(Speed, Position);
            }
            else if (keyboardState.IsKeyDown(Up))
            {
                Speed = Vector2.Zero;
                Facing = "up";

                Speed = new Vector2(0, -runningSpeed);
                Position = Vector2.Add(Speed, Position);
            }
            else if (keyboardState.IsKeyDown(Down))
            {
                Speed = Vector2.Zero;
                Facing = "down";

                Speed = new Vector2(0, runningSpeed);
                Position = Vector2.Add(Speed, Position);
            }
            else if (keyboardState.IsKeyDown(Left))
            {
                Speed = Vector2.Zero;
                Facing = "left";

                Speed = new Vector2(-runningSpeed, 0);
                Position = Vector2.Add(Speed, Position);
            }
            else if (keyboardState.IsKeyDown(Right))
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
            Sprite.EntityFacing = Facing;
            Sprite.Position = Position;

            Ranged.Position = Position;
            Ranged.Direction = Facing;
        }
        /// <summary>
        /// Fires the gun
        /// </summary>
        /// <param name="gameTime"></param>
        public void FireR(GameTime gameTime)
        {
            Ranged.Fire(gameTime);
            
        }

        /// <summary>
        /// Uses the object collided with to determine how to react to a collison
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="collidedWith">Object collided with</param>
        public void Collided(GameTime gameTime, ICollidable collidedWith)
        {
            
            
            ///extraction of collidewith   
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
                case "Rocket":
                    Health = Health - 2;
                    break;

               
            }
        }
    }
}


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Overhaul_Of_Apocalyptica.Entities;

namespace Overhaul_Of_Apocalyptica.Entities
{
   public class Ninja :  IEntity,ICollidable
    {
        public Vector2 Position { get; set; }
        public Vector2 Speed { get; set; }

        private List<Rectangle> _frames = new List<Rectangle>();
        private List<Rectangle> _framesAlt = new List<Rectangle>();//TODO MAKE THIS EFFICEINT
        private Texture2D _texture2D;
        private Sprite _sprite;
        protected Rectangle frame1 = new Rectangle(115, 0, 69, 84);  // left
        protected Rectangle frame2 = new Rectangle(0, 0, 69, 84); // right
        protected Rectangle frame3 = new Rectangle(257, 0, 38, 84);  // up
        protected Rectangle frame4 = new Rectangle(356, 0, 38, 84);  // down

        protected Rectangle altLeft = new Rectangle(69, 0, 44, 84); //ternate left
        protected Rectangle altRight = new Rectangle(70, 0, 44, 84); //alternate right
        protected Rectangle altUp = new Rectangle(345, 0, 23, 42); //alternate up
        protected Rectangle altDown = new Rectangle(552, 0, 23, 42); //alternate down

        public int Health { get; set; }
        public double Armour { get; set; }
        public Rectangle CollisionBox { get; set; }

        string _ninjaFacing = "right";


        Heart _heart;

        public Ninja(Texture2D texture2D, Texture2D heartSprite)
        {
            _texture2D = texture2D;
            _frames.Add(frame1);
            _frames.Add(frame2);
            _frames.Add(frame3);
            _frames.Add(frame4);
            _framesAlt.Add(altLeft);
            _framesAlt.Add(altRight);
            _framesAlt.Add(altUp);
            _framesAlt.Add(altDown);
            _sprite = new Sprite(texture2D, _frames,_framesAlt, Position);

            Speed = Vector2.Zero;
            Position = new Vector2(100, 200);
            CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, 34, 42);

            Health = 100;
            _heart = new Heart(heartSprite, Health, this, new List<Rectangle>() { new Rectangle(0, 0, 17, 14) });
        }



        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            _sprite.Draw(spriteBatch, gameTime,_ninjaFacing);
            _heart.Draw(spriteBatch, gameTime);
            

        }
        public void Update(GameTime gameTime)
        {
            Vector2 temp = new Vector2(); // used to calculate current velocity
            if (Keyboard.GetState().IsKeyDown(Keys.W) && (Keyboard.GetState().IsKeyDown(Keys.A)))
            {
                Speed = Vector2.Zero;

                _ninjaFacing = "left";


                Speed = Vector2.Add(Vector2.Add(Vector2.Zero, temp = new Vector2(-5, -5)), Speed);

                Position = Vector2.Add(Speed, Position);
                CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, 34, 42);

            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S) && (Keyboard.GetState().IsKeyDown(Keys.A)))
            {
                Speed = Vector2.Zero;
                _ninjaFacing = "left";
                
                Speed = Vector2.Add(Vector2.Add(Vector2.Zero, temp = new Vector2(-5, 5)), Speed);
               
                Position = Vector2.Add(Speed, Position);
                CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, 34, 42);

            }
            else if (Keyboard.GetState().IsKeyDown(Keys.W) && (Keyboard.GetState().IsKeyDown(Keys.D)))
            {
                Speed = Vector2.Zero;
                _ninjaFacing = "right";
               
                Speed = Vector2.Add(Vector2.Add(Vector2.Zero, temp = new Vector2(5, -5)), Speed);
                Position = Vector2.Add(Speed, Position);
                CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, 34, 42);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S) && (Keyboard.GetState().IsKeyDown(Keys.D)))
            {
                Speed = Vector2.Zero;
                _ninjaFacing = "right";
                Speed = Vector2.Add(Vector2.Add(Vector2.Zero, temp = new Vector2(5, 5)), Speed);
                Position = Vector2.Add(Speed, Position);
                CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, 34, 42);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Speed = Vector2.Zero;
                //Up
                _ninjaFacing = "up";
                Speed = Vector2.Add(Vector2.Add(Vector2.Zero, temp = new Vector2(0, -5)), Speed);
                Position = Vector2.Add(Speed, Position);
                CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, 19, 42);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Speed = Vector2.Zero;
                // down
                _ninjaFacing = "down";

                Speed = Vector2.Add(Vector2.Add(Vector2.Zero, temp = new Vector2(0, 5)), Speed);
                Position = Vector2.Add(Speed, Position);
                CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, 19, 42);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Speed = Vector2.Zero;
                _ninjaFacing = "left";
                Speed = Vector2.Add(Vector2.Add(Vector2.Zero, temp = new Vector2(-5, 0)), Speed);
                Position = Vector2.Add(Speed, Position);
                CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, 34, 42);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Speed = Vector2.Zero;
                _ninjaFacing = "right";
                Speed = Vector2.Add(Vector2.Add(Vector2.Zero, temp = new Vector2(5, 0)), Speed);
                Position = Vector2.Add(Speed, Position);
                CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, 34, 42);
            }
            else
            {
                Speed = Vector2.Zero;
            }
            _sprite.Update(gameTime, Position);
            _heart.Update(gameTime);

          
        }
    }
}

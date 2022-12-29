﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Overhaul_Of_Apocalyptica.Entities;
using Overhaul_Of_Apocalyptica.Entities.Characters;

namespace Overhaul_Of_Apocalyptica.Entities
{
    public class Ninja : Player
    {
        #region declarations
        private List<Rectangle> _frames = new List<Rectangle>();
        private List<Rectangle> _framesAlt = new List<Rectangle>();//TODO MAKE THIS EFFICEINT

        protected Rectangle frame1 = new Rectangle(115, 0, 69, 84);  // left
        protected Rectangle frame2 = new Rectangle(0, 0, 69, 84); // right
        protected Rectangle frame3 = new Rectangle(257, 0, 38, 84);  // up
        protected Rectangle frame4 = new Rectangle(356, 0, 38, 84);  // down

        protected Rectangle altLeft = new Rectangle(69, 0, 44, 84); //ternate left
        protected Rectangle altRight = new Rectangle(70, 0, 44, 84); //alternate right
        protected Rectangle altUp = new Rectangle(345, 0, 23, 42); //alternate up
        protected Rectangle altDown = new Rectangle(552, 0, 23, 42); //alternate down

        public override Vector2 Position { get; set; }
        public override Vector2 Speed { get; set; }
        public override float MovementSpeed { get; set; }
        public override int Health { get; set; }
        public override double Armour { get; set; }
        public override string Facing { get; set; }
        public override bool IsActive { get; set; }

        private const float RUNNING_SPEED = 5f;

        private Texture2D _texture2D;
        private Sprite _sprite;
        private Heart _heart;
        #endregion
        #region Constructor
        public Ninja(Texture2D texture, Texture2D heartSprite )
        {

            _texture2D = texture;
            _frames.Add(frame1);
            _frames.Add(frame2);
            _frames.Add(frame3);
            _frames.Add(frame4);
            _framesAlt.Add(altLeft);
            _framesAlt.Add(altRight);
            _framesAlt.Add(altUp);
            _framesAlt.Add(altDown);
            _sprite = new Sprite(texture, _frames, _framesAlt, Position);

            Speed = Vector2.Zero;
            Position = new Vector2(100, 200);
            CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, 34, 42);

            Health = 100;
            _heart = new Heart(heartSprite, Health, this, new List<Rectangle>() { new Rectangle(0, 0, 17, 14) });

        }
        #endregion
        #region Methods
        public override void Update(GameTime gameTime)
        {
            if (IsActive == true)
            {
                Movement(RUNNING_SPEED);
                _sprite.Update(gameTime, Position);
                _heart.Update(gameTime);
                CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, _sprite.Source.Width, _sprite.Source.Height);
            }

        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _sprite.Draw(spriteBatch, gameTime, Facing);
            _heart.Draw(spriteBatch, gameTime);

        }
        #endregion
    }
}

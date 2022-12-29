using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Overhaul_Of_Apocalyptica.Entities;
using Overhaul_Of_Apocalyptica.Entities.Characters;

namespace Overhaul_Of_Apocalyptica.Entities.Characters
{
    public class Soldier : Player
    {
        #region Declarations

        private List<Rectangle> _frames = new List<Rectangle>();

        protected Rectangle frame1 = new Rectangle(0, 0, 38, 43);  // left
        protected Rectangle frame2 = new Rectangle(41, 0, 38, 43); // right
        protected Rectangle frame3 = new Rectangle(0, 0, 38, 43); // left
        protected Rectangle frame4 = new Rectangle(41, 0, 38, 43); // right

        public override Vector2 Position { get; set; }
        public override Vector2 Speed { get; set; }
        public override float MovementSpeed { get; set; }
        public override int Health { get; set; }
        public override double Armour { get; set; }
        public override string Facing { get; set; }
        public override bool IsActive { get; set; }

        private const float RUNNING_SPEED = 3.5f;

        private Texture2D _texture2D;
        private Sprite _sprite;
        private Heart _heart;
        #endregion
        public Soldier(Texture2D texture, Texture2D heartSprite)
        {
            _texture2D = texture;
            _frames.Add(frame1);
            _frames.Add(frame2);
            _frames.Add(frame3);
            _frames.Add(frame4);
            _sprite = new Sprite(texture, _frames, Position);

            Speed = Vector2.Zero;
            Position = new Vector2(100, 200);
            CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, 38, 43);

            Health = 100;
            _heart = new Heart(heartSprite, Health, this, new List<Rectangle>() { new Rectangle(0, 0, 17, 14) });
        }
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


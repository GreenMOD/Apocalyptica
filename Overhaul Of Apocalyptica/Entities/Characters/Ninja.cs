using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Overhaul_Of_Apocalyptica.Entities;
using Overhaul_Of_Apocalyptica.Entities.Characters;
using Overhaul_Of_Apocalyptica.Entities.Weapons;

namespace Overhaul_Of_Apocalyptica.Entities
{
    public class Ninja : Player
    {
        #region declarations

        protected Rectangle Frame1 = new Rectangle(115, 0, 69, 84);  // left
        protected Rectangle Frame2 = new Rectangle(0, 0, 69, 84); // right
        protected Rectangle Frame3 = new Rectangle(257, 0, 38, 84);  // up
        protected Rectangle Frame4 = new Rectangle(356, 0, 38, 84);  // down

        protected Rectangle AltLeft = new Rectangle(69, 0, 44, 84); //ternate left
        protected Rectangle AltRight = new Rectangle(70, 0, 44, 84); //alternate right
        protected Rectangle AltUp = new Rectangle(345, 0, 23, 42); //alternate up
        protected Rectangle AltDown = new Rectangle(552, 0, 23, 42); //alternate down

        public override Vector2 Position { get; set; }
        public override Vector2 Speed { get; set; }
        public override int Health { get; set; }
        public override string Facing { get; set; }
        public override bool IsActive { get; set; }
        public override Gun Ranged { get; set; }

        private const float RUNNING_SPEED = 5f;

        private Sprite _sprite;
        private Heart _heart;
        #endregion
        #region Constructor
        public Ninja(Texture2D texture, Texture2D heartSprite )
        {

            _sprite = new Sprite(texture, new List<Rectangle>() {Frame1,Frame2,Frame3,Frame4 },new List<Rectangle>() {AltLeft,AltRight,AltUp,AltDown }, Position);

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
                PlayerInput(gameTime, Keyboard.GetState());
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

        public override void PlayerInput(GameTime gameTime, KeyboardState currentStateKeys)
        {
            if ((currentStateKeys.IsKeyDown(Keys.W) && (currentStateKeys.IsKeyDown(Keys.A))) | (currentStateKeys.IsKeyDown(Keys.W) && (currentStateKeys.IsKeyDown(Keys.D))) | ((currentStateKeys.IsKeyDown(Keys.S)) && (currentStateKeys.IsKeyDown(Keys.A))) | (currentStateKeys.IsKeyDown(Keys.S) && (currentStateKeys.IsKeyDown(Keys.D))) | (currentStateKeys.IsKeyDown(Keys.W)) | (currentStateKeys.IsKeyDown(Keys.A)) | (currentStateKeys.IsKeyDown(Keys.S)) ^ (currentStateKeys.IsKeyDown(Keys.D))) //TODO THIS MOVEMENT DOESN'T WORK WITH MUTPLE BUTTON PRESSES
            {
                Movement(RUNNING_SPEED, currentStateKeys);
            }
            //if (currentStateKeys.IsKeyDown(Keys.G))
            //{
            //    FireR(Ranged, gameTime);
            //}
        }
        #endregion
    }
}


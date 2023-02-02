using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Overhaul_Of_Apocalyptica.Entities;
using Overhaul_Of_Apocalyptica.Entities.Characters;
using Overhaul_Of_Apocalyptica.Entities.Weapons;

namespace Overhaul_Of_Apocalyptica.Entities.Characters
{
    public class Soldier : Player
    {
        #region Declarations

        private List<Rectangle> _frames = new List<Rectangle>();

        protected Rectangle Frame1 = new Rectangle(84, 0, 76, 86);  // left  //TODO SORT THE HITBOX ON THE SOLDIER
        protected Rectangle Frame2 = new Rectangle(0, 0, 76, 86);   // right
        protected Rectangle Frame3 = new Rectangle(167, 0, 40, 86); // up
        protected Rectangle Frame4 = new Rectangle(207, 0, 40, 86); // down

        private Vector2 _position = new Vector2();
        public override Vector2 Position { get { return _position; } set { _position = new Vector2((float)MathHelper.Clamp(value.X, 45, 755), (float)MathHelper.Clamp(value.Y, 45, 435)); } }
        public override Vector2 Speed { get; set; }
        public override int Health { get; set; }
        public override string Facing { get; set; }
        public override bool IsActive { get; set; }
        public override Gun Ranged { get; set; }

        private const float RUNNING_SPEED = 3.5f;

        private Texture2D _texture2D;
        private Sprite _sprite;
        private Heart _heart;
        private EntityManager _entityManager;
        private CollisionManager _collisionManager;
        
        #endregion
        public Soldier(Texture2D texture, Texture2D heartTexture, Texture2D bulletTexture, GameTime gameTime)
        {
            _texture2D = texture;
            _frames.Add(Frame1);
            _frames.Add(Frame2);
            _frames.Add(Frame3);
            _frames.Add(Frame4);
            _sprite = new Sprite(texture, _frames, Position);

            Speed = Vector2.Zero;
            Position = new Vector2(100, 200);

            CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, _sprite.Source.Width, _sprite.Source.Height);

            Health = 100;
            _heart = new Heart(heartTexture, Health, this, new List<Rectangle>() { new Rectangle(0, 0, 17, 14) });

            Ranged = new M4(new Vector2(Position.X + 75, Position.Y), bulletTexture, gameTime) ; //TODO FIRE FROM Gun
        }
        #region Methods
        public override void Update(GameTime gameTime)
        {
            if (IsActive == true)
            {
                PlayerInput(gameTime, Keyboard.GetState());
                Ranged.Update(gameTime);
                _sprite.Update(gameTime, Position);
                _heart.Update(gameTime);
                CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, _sprite.Source.Width, _sprite.Source.Height);
            }

        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _sprite.Draw(spriteBatch, gameTime, Facing);
            _heart.Draw(spriteBatch, gameTime);
            Ranged.Draw(spriteBatch, gameTime);

        }/// <summary>
        /// When a user inputs any key this subroutine decifers which key it is and then executes the according subroutine
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="currentStateKeys"></param>
        public override void PlayerInput(GameTime gameTime, KeyboardState currentStateKeys)
        {
            if ((currentStateKeys.IsKeyDown(Keys.W) && (currentStateKeys.IsKeyDown(Keys.A))) | (currentStateKeys.IsKeyDown(Keys.W) && (currentStateKeys.IsKeyDown(Keys.D))) | (( currentStateKeys.IsKeyDown(Keys.S)) && (currentStateKeys.IsKeyDown(Keys.A))) | (currentStateKeys.IsKeyDown(Keys.S) && (currentStateKeys.IsKeyDown(Keys.D)))| (currentStateKeys.IsKeyDown(Keys.W)) | (currentStateKeys.IsKeyDown(Keys.A)) | (currentStateKeys.IsKeyDown(Keys.S)) ^ (currentStateKeys.IsKeyDown(Keys.D)))
            {
                Movement(RUNNING_SPEED, currentStateKeys);
            }
            if (currentStateKeys.IsKeyDown(Keys.G))
            {
                FireR(gameTime);
            }
        }
        #endregion
    }
}


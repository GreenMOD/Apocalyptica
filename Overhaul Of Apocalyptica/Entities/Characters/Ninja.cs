using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Overhaul_Of_Apocalyptica.Entities.Characters;
using Overhaul_Of_Apocalyptica.Entities.Weapons;

namespace Overhaul_Of_Apocalyptica.Entities
{
    public class Ninja : Player
    {
        #region declarations
        private Sprite _sprite;
        private Heart _hearts;




        protected Rectangle Frame1 = new Rectangle(115, 0, 69, 84);  // left
        protected Rectangle Frame2 = new Rectangle(0, 0, 69, 84); // right
        protected Rectangle Frame3 = new Rectangle(257, 0, 38, 84);  // up
        protected Rectangle Frame4 = new Rectangle(356, 0, 38, 84);  // down

        protected Rectangle AltLeft = new Rectangle(190, 0, 45, 84); //ternate left
        protected Rectangle AltRight = new Rectangle(70, 0, 45, 84); //alternate right
        protected Rectangle AltUp = new Rectangle(300, 0, 38, 84); //alternate up
        protected Rectangle AltDown = new Rectangle(398, 0, 38, 84); //alternate down

        public override Sprite Sprite { get { return _sprite; } set { _sprite = value; } }

        private const float RUNNING_SPEED = 5f;
        public override Heart Hearts { get { return _hearts; } set { _hearts = value; } }


        #endregion
        #region Constructor
        public Ninja(Texture2D texture, Texture2D heartSprite, Texture2D shuriken, GameTime gameTime)
        {
            Facing = "";


            _sprite = new Sprite(texture, new List<Rectangle>() {Frame1,AltLeft,Frame2,AltRight,Frame3,AltUp,Frame4,AltDown }, Position);
            _sprite.FrameTime = 0.75f;

            Speed = Vector2.Zero;
            CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, Sprite.Source.Width,Sprite.Source.Height);

            Health = 100;
            Hearts = new Heart(heartSprite, Health, new List<Rectangle>() { new Rectangle(0, 0, 17, 14) });


            Ranged = new ShurikenJustu(new Vector2(Position.X + 75, Position.Y), shuriken, gameTime);
        }
        #endregion
        #region Methods
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Sprite.Draw(spriteBatch, gameTime);
            Hearts.Draw(spriteBatch, gameTime);
        }
        /// <summary>
        /// Manages player input through a keyboard state
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="currentStateKeys">Stores the values of all keys being pressed</param>
        public override void PlayerInput(GameTime gameTime, KeyboardState currentStateKeys)
        {
            if ((currentStateKeys.IsKeyDown(Up) && (currentStateKeys.IsKeyDown(Left))) | (currentStateKeys.IsKeyDown(Up) && (currentStateKeys.IsKeyDown(Right))) | ((currentStateKeys.IsKeyDown(Down)) && (currentStateKeys.IsKeyDown(Left))) | (currentStateKeys.IsKeyDown(Down) && (currentStateKeys.IsKeyDown(Right))) | (currentStateKeys.IsKeyDown(Up)) | (currentStateKeys.IsKeyDown(Left)) | (currentStateKeys.IsKeyDown(Down)) | (currentStateKeys.IsKeyDown(Right)))
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


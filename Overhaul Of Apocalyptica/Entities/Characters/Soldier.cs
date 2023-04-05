using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Overhaul_Of_Apocalyptica.Entities.Weapons;

namespace Overhaul_Of_Apocalyptica.Entities.Characters
{
    public class Soldier : Player
    {
        #region Declarations

        static Rectangle Frame2 = new Rectangle(4, 0, 71, 86);   // right
        static Rectangle Alt2 = new Rectangle(76, 0, 71, 86);   // right
        static Rectangle Frame1 = new Rectangle(148, 0, 71, 86);  // left
        static Rectangle Alt1 = new Rectangle(221, 0, 71, 86);  // left  
        static Rectangle Frame3 = new Rectangle(292, 0, 37, 86); // up
        static Rectangle Alt3 = new Rectangle(332, 0, 37, 86); // up
        static Rectangle Frame4 = new Rectangle(372, 0, 37, 86); // down
        static Rectangle Alt4 = new Rectangle(411, 0, 37, 86); // down

        private List<Rectangle> _frames = new List<Rectangle>() 
        {
            Frame1,Alt1,

            Frame2,Alt2,

            Frame3,Alt3,

            Frame4,Alt4
        };

        private Sprite _sprite;

        private Heart _hearts;
        public override Heart Hearts { get{ return _hearts; } set { _hearts = value; } }

        public override Sprite Sprite { get { return _sprite; } set { _sprite = value; } }

        private const float RUNNING_SPEED = 3.5f;

        #endregion
        public Soldier(Texture2D texture, Texture2D heartTexture, Texture2D bulletTexture, GameTime gameTime)
        {
            
            Facing = "";
            _frames.Add(Frame1);
            _frames.Add(Frame2);
            _frames.Add(Frame3);
            _frames.Add(Frame4);
            Sprite = new Sprite(texture, _frames, Position);
            _sprite.FrameTime = 0.75f;

            Speed = Vector2.Zero;
                
            CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, Sprite.Source.Width, Sprite.Source.Height);

            Health = 100;
            _hearts = new Heart(heartTexture, Health, new List<Rectangle>() { new Rectangle(0, 0, 17, 14) });

            Ranged = new M4(new Vector2(Position.X + 75, Position.Y), bulletTexture, gameTime) ;
        }
        #region Methods
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Sprite.Draw(spriteBatch, gameTime);
            Hearts.Draw(spriteBatch, gameTime);

        }/// <summary>
        /// When a user inputs any key this subroutine decifers which key it is and then executes the according subroutine. Also changes the position of the gun accordingly
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="currentStateKeys"></param>
        public override void PlayerInput(GameTime gameTime, KeyboardState currentStateKeys)
        {
            if ((currentStateKeys.IsKeyDown(Up) && (currentStateKeys.IsKeyDown(Left))) | (currentStateKeys.IsKeyDown(Up) && (currentStateKeys.IsKeyDown(Right))) | (( currentStateKeys.IsKeyDown(Down)) && (currentStateKeys.IsKeyDown(Left))) | (currentStateKeys.IsKeyDown(Down) && (currentStateKeys.IsKeyDown(Right)))| (currentStateKeys.IsKeyDown(Up)) | (currentStateKeys.IsKeyDown(Left)) | (currentStateKeys.IsKeyDown(Down)) | (currentStateKeys.IsKeyDown(Right)))
            {
                string previousFacing = Facing;
                Movement(RUNNING_SPEED, currentStateKeys);
                switch (Facing)
                {
                    case "left":
                        Ranged.Position = new Vector2(Position.X, Position.Y - 22);
                        break;
                    case "right":
                        Ranged.Position = new Vector2(Position.X + 75, Position.Y - 22);
                        break;
                    case "up":
                        break;
                    case "down":
                        break;
                }
            }
            if (currentStateKeys.IsKeyDown(Keys.G))
            {
                FireR(gameTime);
            }
        }
        #endregion
    }
}


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Overhaul_Of_Apocalyptica.Entities;

namespace Overhaul_Of_Apocalyptica.Controls
{
    public class Button : IEntity
    {
        
         //Inspired by MonoGame Tutorial 012 - Interface Buttons by Oyyou
        //Uses delegates instead of event


        #region Declarations
        private MouseState _currentMouse;

        private SpriteFont _textFont;

        private Texture2D _texture;


        public delegate void BeingClicked(Button button);

        public BeingClicked Click;

        public Color TextColor { get; set; }

        public Vector2 Position { get; set; }

        public Rectangle CollisionBox { get { return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);  }  }

        public string Text { get; set; }

        public bool hasBeenClicked = false;

        #endregion

        #region Methods

        public Button(Texture2D texture , SpriteFont font)
        {
            _texture = texture;

            _textFont = font;

            TextColor = Color.Black;
        }
        /// <summary>
        /// Draws the collison box but ensures that the text is centred inside it
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            
            spriteBatch.Draw(_texture, CollisionBox,new Rectangle(0,0, _texture.Width,_texture.Height), Color.White,0f,new Vector2(0,0),SpriteEffects.None,0f);

            if (!(Text == null))
            {

                spriteBatch.DrawString(_textFont, Text, new Vector2((CollisionBox.X + (CollisionBox.Width / 2) - (_textFont.MeasureString(Text).X / 2)) , (CollisionBox.Y + (CollisionBox.Height / 2) - (_textFont.MeasureString(Text).Y / 2))), TextColor); //This finds the centre of the button and then subtracts the length that the text will take up allowing for text to fit
                //MeasureString is a function which calculates the size of a string using a specifc font.
   
            }
        }
        /// <summary>
        /// Checks if it has been clicked then invokes Click if it has been
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
           
            if (CollisionBox.Contains(_currentMouse.Position))
            {

                if (_currentMouse.LeftButton == ButtonState.Pressed && Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this); //Means that if click is not null then run 
                    hasBeenClicked = true;
                }
            }
            _currentMouse = Mouse.GetState();
        }
        #endregion
    }
}

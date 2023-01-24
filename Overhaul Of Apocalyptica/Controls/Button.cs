using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Overhaul_Of_Apocalyptica.Entities;
using Overhaul_Of_Apocalyptica.Events;

namespace Overhaul_Of_Apocalyptica.Controls
{
    public class Button : IEntity
    {
        #region Declarations
        private MouseState _currentMouse;

        private SpriteFont _textFont;

        private bool _isHovering;

        private MouseState _previousMouse;

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
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var colour = Color.White;

            if (_isHovering)
            {
                colour = Color.White;
            }

            spriteBatch.Draw(_texture, CollisionBox, colour);

            if (!string.IsNullOrEmpty(Text))
            {
                var x = (CollisionBox.X + (CollisionBox.Width / 2) - (_textFont.MeasureString(Text).X / 2));  //This finds the centre of the button and then subtracts the length that the text will take up allowing for text to fit
                var y = (CollisionBox.Y + (CollisionBox.Height / 2) - (_textFont.MeasureString(Text).Y / 2));  //This finds the centre of the button and then subtracts the length that the text will take up allowing for text to fit

                spriteBatch.DrawString(_textFont, Text, new Vector2(x, y), TextColor);
            }
        }
        /// <summary>
        /// Checks if it has been clicked then invokes Click if it has been
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseCollision = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            if (CollisionBox.Intersects(mouseCollision))
            {
                _isHovering = true;

                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this); //Means that if click is not null then run 
                    hasBeenClicked = true;
                }
            }
        }
        #endregion
    }
}

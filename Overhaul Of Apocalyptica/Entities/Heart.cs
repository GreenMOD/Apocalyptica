using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Overhaul_Of_Apocalyptica.Entities.Characters;
namespace Overhaul_Of_Apocalyptica.Entities
{
   public class Heart
    {
        private Sprite _sprite;
      
        private Vector2 _heartPosition;

        public int HeartsLeft { get; set; }

        private int _MAXIMUM_AMOUNT_HEARTS;

        
        public Heart(Texture2D heartSheet, int maximumHealth,List<Rectangle>frames)
        {

            _sprite = new Sprite(heartSheet, frames, _heartPosition);

            _MAXIMUM_AMOUNT_HEARTS = maximumHealth;
            HeartsLeft = maximumHealth /20;

        }


        /// <summary>
        /// Updates the hearts by the amount of health the player has
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="health">The player's health</param>
        public void Update(GameTime gameTime, int health)
        {
            HeartsLeft = health /20; 
        }
        /// <summary>
        /// Displays the correct number of hearts
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (HeartsLeft !=0)
            {
                int XBeginPoint = 800 - _sprite.Source.Width * HeartsLeft; //This calculates the offset from the side of the GameWindow allowing for the hearts to always be on screen and decrease accordingly.
                int YBeginPoint = 480 - _sprite.Source.Height;

                _heartPosition = new Vector2(XBeginPoint, YBeginPoint);

              
                for (int i = 0; i < HeartsLeft; i++)
                {
                        _sprite.Draw(spriteBatch, gameTime);
                        XBeginPoint = 800 - (_sprite.Source.Width *  (HeartsLeft - i)); //moves the position along to the next position for the heart to be drawn
                        _heartPosition = new Vector2(XBeginPoint, YBeginPoint);
                        _sprite.Update(gameTime, _heartPosition);
                }
            }
           

        }
       

    }
}

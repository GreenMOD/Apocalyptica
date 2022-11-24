using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Overhaul_Of_Apocalyptica.Entities
{
    class Heart : IEntity
    {
        private Texture2D _heartSheet;
        private Sprite _sprite;
      
        private Vector2 _heartPosition;
        private Ninja _ninja;

        public int HeartsLeft { get; set; }
        private int MAXIMUM_AMOUNT_HEARTS;

        
        public Heart(Texture2D heartSheet, int maximumHealth, Ninja ninja, List<Rectangle>frames)
        {
            _heartSheet = heartSheet;

            _sprite = new Sprite(_heartSheet, frames, _heartPosition);

            MAXIMUM_AMOUNT_HEARTS = maximumHealth;
            HeartsLeft = maximumHealth / 20;

            _ninja = ninja;
        }

        public void Update(GameTime gameTime)
        {
            HeartsLeft = _ninja.Health/20; 
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (HeartsLeft !=0)
            {
                int XBeginPoint = 800 - _sprite.Source.Width * HeartsLeft; //This calculates the offset from the side of the GameWindow allowing for the hearts to always be on screen and decrease accordingly.
                int YBeginPoint = 480 - _sprite.Source.Height;

                _heartPosition = new Vector2(XBeginPoint, YBeginPoint);

                _sprite.Update(gameTime, _heartPosition);
                for (int i = 0; i <= HeartsLeft; i++)
                {
                    _sprite.Draw(spriteBatch, gameTime);
                    XBeginPoint = 800 - (_sprite.Source.Width * (HeartsLeft - i)); //moves the position along to the next position for the heart to be drawn
                    _heartPosition = new Vector2(XBeginPoint, YBeginPoint);
                    _sprite.Update(gameTime, _heartPosition);
                }
            }
           

        }
       

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Overhaul_Of_Apocalyptica.Entities
{
    interface IEntity
    { 

        void Update(GameTime gameTime);
        

       void Draw(SpriteBatch spriteBatch, GameTime gameTime);

        
    }
}

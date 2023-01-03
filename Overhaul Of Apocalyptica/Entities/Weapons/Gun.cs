using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Overhaul_Of_Apocalyptica.Entities.Zombies;
using System.Linq;

namespace Overhaul_Of_Apocalyptica.Entities.Weapons
{
   public abstract class Gun
    {
        /// Must have:
        /// Limited ammo
        /// Is not an entity as it is part of the players sprite and will be update through the player
        /// Fire()
        /// Reload()
        /// FireRate
        /// Should be compatable with both hitscan and projectile 
        /// Is it being held right now
        public Vector2 Position { get; set; } 
        public int AmmoLeft { get; set; }

        public bool IsReloading { get; set; }
        
        public bool IsHeld { get; set; }

        public string Direction { get; set; }

        public abstract void Fire(GameTime gameTime);

        public abstract void Reload(GameTime gameTime);

        public abstract void Update(GameTime gameTime , Vector2 updatePos, string direction);

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);

        

    }
}

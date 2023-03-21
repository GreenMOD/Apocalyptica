using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Overhaul_Of_Apocalyptica.Entities.Zombies;
using Overhaul_Of_Apocalyptica.Entities.Projectiles;
using System.Linq;


namespace Overhaul_Of_Apocalyptica.Entities.Weapons
{
    class M4 : Gun
    {

        public M4(Vector2 start, Texture2D bulletTexture, GameTime gameTime)
        {
            Position = start;
            BulletTexture = bulletTexture;
            AmmoLeft = 30;
            RateOfFire = 0.25f;
            ReloadTime = 2.5f;
        }

        public override void Fire(GameTime gameTime)
        {
            if (AmmoLeft > 0)
            { 
            NextProjectile = new Bullet(Position, Direction, BulletTexture);
            base.Fire(gameTime);
            }
            else
            {
                Reload(gameTime);
            }
        }
    }
}

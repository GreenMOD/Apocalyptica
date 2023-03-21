using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Overhaul_Of_Apocalyptica.Entities.Projectiles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Overhaul_Of_Apocalyptica.Entities.Weapons
{
    class ShurikenJustu : Gun
    {
        
        public ShurikenJustu(Vector2 start, Texture2D shurikenTexture,  GameTime gameTime)
        {
            Position = start;
            BulletTexture = shurikenTexture;
            RateOfFire = 0.25f;
        }   

        public override void Fire(GameTime gameTime)
        {
            NextProjectile = new Shuriken(Position, Direction, BulletTexture);
            base.Fire(gameTime);
        }

        //public override void Reload(GameTime gameTime)
        //{
        //    return false;
        //}
    }
}

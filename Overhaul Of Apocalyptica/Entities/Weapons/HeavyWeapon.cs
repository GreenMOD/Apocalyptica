using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Overhaul_Of_Apocalyptica.Entities.Projectiles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Overhaul_Of_Apocalyptica.Entities.Weapons
{
    class HeavyWeapon : Gun
    {
        //private const float _FIRE_RATE = 0.15f;

       // private const float _RELOAD_TIME = 5f;

       // private const int _MAX_AMMO = 100;

        public HeavyWeapon(Vector2 start, Texture2D bulletTexture, GameTime gameTime)
        {
            Position = start;
            BulletTexture = bulletTexture;
            AmmoLeft = 100;
            RateOfFire = 0.15f;
            ReloadTime = 5f;
            MaxAmmo = 100;
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

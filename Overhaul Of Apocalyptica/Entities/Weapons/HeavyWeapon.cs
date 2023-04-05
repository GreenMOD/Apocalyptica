using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Overhaul_Of_Apocalyptica.Entities.Projectiles;

namespace Overhaul_Of_Apocalyptica.Entities.Weapons
{
    class HeavyWeapon : Gun
    {

        public HeavyWeapon(Vector2 start, Texture2D bulletTexture, GameTime gameTime)
        {
            Position = start;
            BulletTexture = bulletTexture;
            AmmoLeft = 100;
            RateOfFire = 0.15f;
            ReloadTime = 5f;
            MaxAmmo = 100;
        }
        /// <summary>
        /// If the magazine is not empty the next projectile is set to a bullet and Gun.Fire is called. Otherwise the gun will reload
        /// </summary>
        /// <param name="gameTime"></param>
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

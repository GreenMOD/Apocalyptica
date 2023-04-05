using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Overhaul_Of_Apocalyptica.Entities.Projectiles;


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

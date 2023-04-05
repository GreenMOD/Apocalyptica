using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Overhaul_Of_Apocalyptica.Entities.Projectiles;

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

        /// <summary>
        /// Sets the next projectile to fire to be a shuriken and then calls Gun.Fire
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Fire(GameTime gameTime)
        {
            NextProjectile = new Shuriken(Position, Direction, BulletTexture);
            base.Fire(gameTime);
        }
    }
}

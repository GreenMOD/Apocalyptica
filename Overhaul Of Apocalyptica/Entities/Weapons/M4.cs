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
        private const float _FIRE_RATE = 0.25f;

        private const float _RELOAD_TIME = 2.5f;

        private const int _MAX_AMMO = 30;

        private float _lastFired;

        private float _startReload;

        protected Texture2D BulletTexture;

        protected List<Bullet> BulletsFired = new List<Bullet>();

        public M4(Vector2 start, Texture2D bulletTexture ,string facing, GameTime gameTime)
        {
            Position = start;
            BulletTexture = bulletTexture;
            AmmoLeft = 30;
            _lastFired = -100000f;
        }
        /// <summary>
        /// Fires a bullet
        /// </summary>
        /// <param name="gameTime">Used to compare against the last time this gun was shot</param>
        public override void Fire(GameTime gameTime)
        {
            if (AmmoLeft > 0)
            {
                if (_FIRE_RATE <= gameTime.TotalGameTime.TotalSeconds - _lastFired)
                {
                    Bullet bullet = new Bullet(Position,Direction,BulletTexture);
                    _lastFired = (float)gameTime.TotalGameTime.TotalSeconds;
                    BulletsFired.Add(bullet);
                    AmmoLeft--;
                    Debug.WriteLine(AmmoLeft.ToString());
                }
            }
            else
            { 
                Reload(gameTime);
            }
        }
        /// <summary>
        /// Gun Checks if reload interval has passed
        /// </summary>
        /// <param name="gameTime"></param>
        public override bool Reload(GameTime gameTime)
        {
            if (_startReload == -1)
            {
                IsReloading = true;
                _startReload = (float)gameTime.TotalGameTime.TotalSeconds;
                Debug.WriteLine("Reload start " + gameTime.TotalGameTime.Seconds.ToString());
                return true;
            }
            else if (gameTime.TotalGameTime.TotalSeconds - _startReload >= _RELOAD_TIME)
            {
                IsReloading = false;
                AmmoLeft = _MAX_AMMO;
                _startReload = -1;
                return false;
            }
            return true;
        }

        public override void Update(GameTime gameTime, Vector2 updatePos, string direction)
        {
            Position = updatePos;
            Direction = direction;
            if (BulletsFired.Count != 0)
            {
                foreach (Bullet b in BulletsFired)
                {
                    b.Update(gameTime);
                }
            }
            
            
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (BulletsFired.Count != 0)
            {
                foreach (Bullet b in BulletsFired)
                {
                    b.Draw(spriteBatch,gameTime);
                }
            }
        }
        private void Hit_Registered(Zombie sender, EventArgs e)
        {
            sender.Health = sender.Health - 10;

        }
    }
}

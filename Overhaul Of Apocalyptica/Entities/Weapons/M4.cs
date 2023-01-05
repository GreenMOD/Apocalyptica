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

        private const float FIRE_RATE = 0.25f;

        private const float _RELOAD_TIME = 2.5f;

        private const int _MAX_AMMO = 30;

        private float lastFired;

        private float _startReload;

        protected Texture2D _bulletTexture;

        protected List<Bullet> bulletsFired = new List<Bullet>();

        public M4(Vector2 start, Texture2D bulletTexture ,string facing, GameTime gameTime)
        {
            Position = start;
            _bulletTexture = bulletTexture;
            AmmoLeft = 30;
            lastFired = -100000f;
        }

        public override void Fire(GameTime gameTime)
        {
            if (AmmoLeft > 0)
            {
                if (FIRE_RATE <= gameTime.TotalGameTime.TotalSeconds - lastFired)
                {
                    Bullet bullet = new Bullet(Position,Direction,_bulletTexture);
                    lastFired = (float)gameTime.TotalGameTime.TotalSeconds;
                    bulletsFired.Add(bullet);
                    AmmoLeft--;
                    Debug.WriteLine(AmmoLeft.ToString());
                }
            }
            else
            {
                Debug.WriteLine("Reload start " + gameTime.TotalGameTime.Seconds.ToString());
                Reload(gameTime);
            }
        }

        public override void Reload(GameTime gameTime)
        {
            IsReloading = true;
            _startReload = (float)gameTime.TotalGameTime.TotalSeconds;
        }

        public override void Update(GameTime gameTime, Vector2 updatePos, string direction)
        {
            Position = updatePos;
            Direction = direction;
            if (bulletsFired.Count != 0)
            {
                foreach (Bullet b in bulletsFired)
                {
                    b.Update(gameTime);
                }
            }
            
            if (IsReloading)
            {
                if (gameTime.TotalGameTime.TotalSeconds - _startReload >= _RELOAD_TIME)
                {
                    IsReloading = false;
                    AmmoLeft = _MAX_AMMO;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (bulletsFired.Count != 0)
            {
                foreach (Bullet b in bulletsFired)
                {
                    b.Draw(spriteBatch,gameTime);
                }
            }
        }
    }
}

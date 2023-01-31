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
        private const float _FIRE_RATE = 0.05f;

        private const float _RELOAD_TIME = 5f;

        private const int _MAX_AMMO = 100;

        private float _lastFired;

        private float _startReload;

        protected Texture2D BulletTexture;
        private List<Bullet> _bulletsFired;

        public override List<Bullet> BulletsToAdd { get; set; }

        public HeavyWeapon(Vector2 start, Texture2D bulletTexture, GameTime gameTime)
        {
            Position = start;
            BulletTexture = bulletTexture;
            AmmoLeft = 100;
            _lastFired = -100000f;
            _bulletsFired = new List<Bullet>();
            BulletsToAdd= new List<Bullet>();
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (_bulletsFired.Count != 0)
            {
                foreach (Bullet b in _bulletsFired)
                {
                    b.Draw(spriteBatch, gameTime);
                }
            }
        }

        public override void Fire(GameTime gameTime)
        {
            if (AmmoLeft > 0)
            {
                if (_FIRE_RATE <= gameTime.TotalGameTime.TotalSeconds - _lastFired)
                {
                    Bullet bullet = new Bullet(Position, Direction, BulletTexture);
                    _lastFired = (float)gameTime.TotalGameTime.TotalSeconds;
                    _bulletsFired.Add(bullet);
                    AmmoLeft--;
                    BulletsToAdd.Add(bullet);
                    Debug.WriteLine(AmmoLeft.ToString());
                }
            }
            else
            {
                Reload(gameTime);
            }
        }

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

        public override void Update(GameTime gameTime)
        {
            if (_bulletsFired.Count != 0)
            {
                foreach (Bullet b in _bulletsFired)
                {
                    b.Update(gameTime);
                }
            }
        }

    }
}

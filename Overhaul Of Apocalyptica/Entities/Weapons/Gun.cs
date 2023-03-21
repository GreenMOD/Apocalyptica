using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Overhaul_Of_Apocalyptica.Entities.Zombies;
using System.Linq;
using Overhaul_Of_Apocalyptica.Entities.Projectiles;
using System.Runtime.InteropServices.WindowsRuntime;

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
        /// Is it being held right now <summary>

        #region Declarations
        private Vector2 _position = Vector2.Zero;

        private int _ammoLeft = 0;

        private bool _isReloading = false;

        private string _direction = "";

        private float _rateOfFire = 0.25f;

        private float _reloadTime = 2.5f;

        private int _maxAmmo = 30;

        private List<Projectile> _fired = new List<Projectile>();

        private float _lastFired = -1000000f;

        private float _startReload;

        protected Texture2D BulletTexture;

        #endregion

        #region Properties
        public Vector2 Position { get { return _position; } set { _position = value; } }

        public int AmmoLeft { get { return _ammoLeft; } set { _ammoLeft = value; } }

        public bool IsReloading { get { return _isReloading; } set { _isReloading = value; } }

        public string Direction { get { return _direction; } set { _direction = value; } }

        public float RateOfFire { set { _rateOfFire = value; } }

        public float ReloadTime { set { _reloadTime = value; } }

        public int MaxAmmo { set { _maxAmmo = value; } }

        public Projectile NextProjectile;

        public List<Projectile> Fired { get { return _fired; } }
        public List<Projectile> BulletsToAdd = new List<Projectile>();
        public List<Projectile> BulletsToRemove = new List<Projectile>();
        #endregion

        /// <summary>
        /// Fires a bullet
        /// </summary>
        /// <param name="gameTime">Used to compare against the last time this gun was shot</param>
        public virtual void Fire(GameTime gameTime)
        {
            if (_rateOfFire <= gameTime.TotalGameTime.TotalSeconds - _lastFired)
            {
                _lastFired = (float)gameTime.TotalGameTime.TotalSeconds;
                Fired.Add(NextProjectile);
                AmmoLeft--;
                BulletsToAdd.Add(NextProjectile);
                NextProjectile = null;
            }
        }
        public virtual void Update(GameTime gameTime)
        {
            if (Fired.Count != 0)
            {
                foreach (Projectile p in Fired)
                {
                    if (p.IsDestroyed)
                    {
                        BulletsToRemove.Add(p);
                    }
                    else if (p.FlightTime > 8f)
                    {
                        BulletsToRemove.Add(p);
                    }

                }
                foreach (Projectile p in BulletsToRemove)
                {
                    Fired.Remove(p);
                }
            }
        }
        /// <summary>
        /// Gun Checks if reload interval has passed
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Reload(GameTime gameTime)
        {
            if (_startReload == -1)
            {
                IsReloading = true;
                _startReload = (float)gameTime.TotalGameTime.TotalSeconds;
            }
            else if (gameTime.TotalGameTime.TotalSeconds - _startReload >= _reloadTime)
            {
                IsReloading = false;
                AmmoLeft = _maxAmmo;
                _startReload = -1;
            }

        }

    }
}

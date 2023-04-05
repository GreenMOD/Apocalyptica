using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Overhaul_Of_Apocalyptica.Entities.Weapons
{
   public abstract class Gun
    {
     
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
        /// Fires a bullet if the time between the last time it was fired is greater than or equal to  the rate of fire
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
        /// <summary>
        /// Maitains the lists and removes projectiles that are expired or destroyed
        /// </summary>
        /// <param name="gameTime"></param>
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
        /// Checks if reload interval has passed then reloads the gun
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

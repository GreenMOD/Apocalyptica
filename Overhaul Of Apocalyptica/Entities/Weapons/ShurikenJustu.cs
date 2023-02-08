using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Overhaul_Of_Apocalyptica.Entities.Projectiles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Overhaul_Of_Apocalyptica.Entities.Weapons
{
    class ShurikenJustu : Gun, IEntity
    {
        private const float _FIRE_RATE = 0.25f;

        private float _lastFired;

        private Texture2D _shurikenTexture;

        private List<Shuriken> _shurikenThrown;
        public override List<Projectile> BulletsToAdd { get; set; }

        public override List<Projectile> BulletsToRemove { get; set; }
        
        public ShurikenJustu(Vector2 start, Texture2D shurikenTexture,  GameTime gameTime)
        {
            Position = start;
            _shurikenTexture = shurikenTexture;
            _lastFired = -100000f;
            _shurikenThrown = new List<Shuriken>();
            BulletsToAdd = new List<Projectile>();
            BulletsToRemove = new List<Projectile>();
            //Direction = facing;
        }   

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (_shurikenThrown.Count != 0)
            {
                foreach (Shuriken b in _shurikenThrown)
                {
                    b.Draw(spriteBatch, gameTime);
                }
            }
        }

        public override void Fire(GameTime gameTime)
        {
            if (_FIRE_RATE <= gameTime.TotalGameTime.TotalSeconds - _lastFired)
            { //fix this so that guns have a facing instead of string
                Shuriken shuriken1 = new Shuriken(Position,Direction, _shurikenTexture);
                _lastFired = (float)gameTime.TotalGameTime.TotalSeconds;
                _shurikenThrown.Add(shuriken1);
                BulletsToAdd.Add(shuriken1);
                Debug.WriteLine(AmmoLeft.ToString());
            }
        }

        public override bool Reload(GameTime gameTime)
        {
            return false;
        }


        public override void Update(GameTime gameTime)
        {

            if (_shurikenThrown.Count != 0)
            {
                foreach (Shuriken b in _shurikenThrown)
                {
                    if (b.IsDestroyed)
                    {
                        BulletsToRemove.Add(b);
                    }
                    else
                    {
                        b.Update(gameTime);
                    }

                }
                foreach (Shuriken b in BulletsToRemove)
                {
                    _shurikenThrown.Remove(b);
                }
            }

        }
    }
}

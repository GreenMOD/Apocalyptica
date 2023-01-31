using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Overhaul_Of_Apocalyptica.Entities.Projectiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Overhaul_Of_Apocalyptica.Entities.Weapons
{
    class ShurikenJustu : Gun, IEntity
    {
        public override List<Bullet> BulletsToAdd { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Fire(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override bool Reload(GameTime gameTime)
        {
            throw new NotImplementedException();
        }


        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}

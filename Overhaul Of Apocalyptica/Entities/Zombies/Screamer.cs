using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Overhaul_Of_Apocalyptica.Entities;

namespace Overhaul_Of_Apocalyptica.Entities.Zombies
{
    class Screamer : Zombie, ICollidable
    {
        public override Vector2 Position { get; set; }

        public Vector2 Destination { get; set; }

        protected Rectangle frame1 = new Rectangle(0, 40, 59, 20);  // left
        protected Rectangle frame2 = new Rectangle(22, 40, 59, 20);  // right

       

        public Screamer(Texture2D texture2D, Vector2 spawnLocation,EntityManager entityManager)
        {
            _frames.Add(frame1);
            _frames.Add(frame2);

            _sprite = new Sprite(texture2D, _frames, Position);
            _texture2D = texture2D;
            _entityManager = entityManager; //assigned an entitymanager to allow for it to track any enitiy as its target
            _ninja = (Ninja)_entityManager.GetEntities<Ninja>();
            Position = spawnLocation;
            CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, _sprite.Source.Width, _sprite.Source.Height);
            
        }
        public override void Update(GameTime gameTime)
        {
            _ninja = (Ninja)_entityManager.GetEntities<Ninja>();
            base.Update(gameTime);


            foreach (SwarmBomb s in _entityManager.GetEntities<SwarmBomb>())
            {
                if (s.IsDestroyed)
                    Destination = s.Position;
            }
            Seek(Destination);
            
        }

    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Overhaul_Of_Apocalyptica.Entities;

namespace Overhaul_Of_Apocalyptica.Entities.Zombies
{
    class Screamer : Zombie, ICollidable
    {
        public override Vector2 Position { get; set; }

        public Vector2 Destination { get; set; }
        bool reachedDestination = true;

        protected Rectangle frame1 = new Rectangle(0, 40, 59, 20);  // left
        protected Rectangle frame2 = new Rectangle(22, 40, 59, 20);  // right

       

        public Screamer(Texture2D texture2D, Vector2 spawnLocation,EntityManager entityManager)
        {
            _frames.Add(frame1);
            _frames.Add(frame2);

            _sprite = new Sprite(texture2D, _frames, Position);
            _texture2D = texture2D;
            Position = spawnLocation;
            CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, _sprite.Source.Width, _sprite.Source.Height);
            _entityManager = entityManager; //assigned an entitymanager to allow for it to track any enitiy as its target
            List<Ninja> ninjas = _entityManager.GetEntities<Ninja>().ToList(); //creates a list of all ninjas *to be changed to players* from a IEnumerable
            _ninja = ninjas[0];
            foreach (Ninja n in ninjas)
            {
                if ((_ninja.Position - Position).Length() > (n.Position - Position).Length())
                {
                    _ninja = n;
                }
            }
            
        }
        public override void Update(GameTime gameTime)
        {
            
            base.Update(gameTime);


            foreach (SwarmBomb s in _entityManager.GetEntities<SwarmBomb>())
            {
                if (s.IsDestroyed)
                    Destination = s.Position;
                reachedDestination = false;
            }
            if ((reachedDestination == true))
            {
                Idle(gameTime);
            }
            else 
            {
                Seek(Destination);
            }
            if (Position == Destination)
            {
                reachedDestination = true;
            }
            
        }
        public void Idle(GameTime gameTime)
        {

        }

    }
}

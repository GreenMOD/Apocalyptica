using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Overhaul_Of_Apocalyptica.Entities;
using System.Linq;

namespace Overhaul_Of_Apocalyptica.Entities
{
    class Walker : Zombie, ICollidable
    {
        public override Vector2 Position { get; set; }

        protected Rectangle frame1 = new Rectangle(0, 0, 17, 39);  // left
        protected Rectangle frame2 = new Rectangle(19, 0, 17, 39);  // right
       
       
        protected Rectangle altLeft = new Rectangle(94, 0, 23, 42); //alternate left
        protected Rectangle altRight = new Rectangle(94, 0, 23, 42); //alternate right
        protected Rectangle altUp = new Rectangle(94, 0, 23, 42); //alternate up
        protected Rectangle altDown = new Rectangle(94, 0, 23, 42); //alternate down

        public Walker(Texture2D texture2D, EntityManager entityManager, Vector2 spawnLocation,List<Zombie>zombiesInView )
        {
            _frames.Add(frame1);
            _frames.Add(frame2);
            _texture2D = texture2D;
            _zombiesInView = zombiesInView;
            Position = spawnLocation;
            _sprite = new Sprite(texture2D, _frames, Position);
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
        


    }
}

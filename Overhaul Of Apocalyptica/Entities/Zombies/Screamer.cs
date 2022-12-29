using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Overhaul_Of_Apocalyptica.Entities;
using Overhaul_Of_Apocalyptica.Entities.Characters;

namespace Overhaul_Of_Apocalyptica.Entities.Zombies
{
    class Screamer : Zombie, ICollidable
    {
        public override Vector2 Position { get; set; }

        public Vector2 Destination { get; set; }
        bool reachedDestination = true;

        protected Rectangle frame1 = new Rectangle(0, 40, 20, 20);  // left
        protected Rectangle frame2 = new Rectangle(22, 40, 20, 20);  // right

        

        public Screamer(Texture2D texture2D, Vector2 spawnLocation,EntityManager entityManager)
        {
            _frames.Add(frame1);
            _frames.Add(frame2);

            _sprite = new Sprite(texture2D, _frames, Position);
            _texture2D = texture2D;
            Position = spawnLocation;
            CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, _sprite.Source.Width, _sprite.Source.Height);
            _entityManager = entityManager; //assigned an entitymanager to allow for it to track any enitiy as its target
            List<Player> players = _entityManager.GetEntities<Player>().ToList(); //creates a list of all players from a IEnumerable
            _player = players[0];
            foreach (Player n in players)
            {
                if ((_player.Position - Position).Length() > (n.Position - Position).Length())
                {
                    _player = n;
                }
            }
            
        }
        public override void Update(GameTime gameTime)
        {
            
            
            List<SwarmBomb> swarmBombs =_entityManager.GetEntities<SwarmBomb>().ToList<SwarmBomb>();

            //checks position of all swarmbombs in entitymanager. If it finds one that is destoryed it will set its desination to be there
            foreach (SwarmBomb s in swarmBombs)
            {
                if (s.IsDestroyed)
                {
                    Destination = s.Position;
                    reachedDestination = false;
                }
                    
            }
            if ((reachedDestination == true))
            {
                Idle();
            }
            else 
            {
                Seek(Destination);
            }
            if (Position == Destination)
            {
                reachedDestination = true;
            }
            base.Update(gameTime);

        }
       

    }
}

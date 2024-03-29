﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Overhaul_Of_Apocalyptica.Entities
{
    class CollisionManager
    {
        public List<ICollidable> Collidables { get; set; }
        private List<ICollidable> _toAdd;
        private List<ICollidable> _toRemove;
        

        public CollisionManager(List<ICollidable> collidables)
        {
            Collidables = collidables;
            _toAdd = new List<ICollidable>();
            _toRemove = new List<ICollidable>();
        }

        public void Update(GameTime gameTime)
        {
            foreach (ICollidable c in Collidables) 
            {
                for (int i = 0; i < Collidables.Count-1; i++)
                {
                    if (c != Collidables[i])
                    {
                        if (c.CollisionBox.Intersects(Collidables[i].CollisionBox))
                        {
                            c.Collided(gameTime, Collidables[i]);
                        }
                    }
                }
                
            }
            foreach (ICollidable c in _toAdd)
            {
                Collidables.Add(c);
            }
            foreach (ICollidable c in _toRemove)
            {
                Collidables.Remove(c);
            }
            _toAdd.Clear();
            _toRemove.Clear();
        }
        public void AddCollidable(ICollidable collidable)
        {
            _toAdd.Add(collidable);
        }
        public void RemoveCollidable(ICollidable collidable)
        {
            _toRemove.Add(collidable);
        }
    }
}

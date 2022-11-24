﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Overhaul_Of_Apocalyptica.Entities.Zombies;
using System.Linq;
namespace Overhaul_Of_Apocalyptica.Entities
{
    class EntityManager
    {
        public List<IEntity> entities = new List<IEntity>();
        List<IEntity> toAdd = new List<IEntity>();
        List<IEntity> toRemove = new List<IEntity>();

        public void Update(GameTime gameTime)
        {
            
            foreach (IEntity E in entities)
            {
                E.Update(gameTime);
                
            }
            foreach (IEntity E in toAdd)
            {
                
                entities.Add(E);
                
                 
            }
            foreach (IEntity E in toRemove)
            {
                entities.Remove(E);
            }
            toAdd.Clear();
            toRemove.Clear();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (IEntity E in entities)
            {
                E.Draw(spriteBatch, gameTime);
            }
        }

        public void AddEntity(IEntity entity)
        {
            toAdd.Add(entity);
        }

        public void RemoveEntity(IEntity entity)
        {
            toRemove.Add(entity);
        }

        public IEnumerable<E> GetEntities<E>() where E : IEntity 
        {
            return entities.OfType<E>();
        }  
    }
}

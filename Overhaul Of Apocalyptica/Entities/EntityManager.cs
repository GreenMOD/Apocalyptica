using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Overhaul_Of_Apocalyptica.Entities.Zombies;
using System.Linq;
namespace Overhaul_Of_Apocalyptica.Entities
{
    class EntityManager : IEntity
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
        /// <summary>
        /// Returns a list of entites of selected E
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <returns></returns>
        public List<E> GetEntities<E>() where E : IEntity 
        {
            return entities.OfType<E>().ToList();
        }

        public void Clear()
        {
            foreach(IEntity e in entities) 
            {
                toRemove.Add(e);
            }
        }
    }
}

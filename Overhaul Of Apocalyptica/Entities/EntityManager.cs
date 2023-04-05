using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
namespace Overhaul_Of_Apocalyptica.Entities
{
    class EntityManager : IEntity
    {
        public List<IEntity> Entities = new List<IEntity>();
        private List<IEntity> _toAdd = new List<IEntity>();
        private List<IEntity> _toRemove = new List<IEntity>();

        public void Update(GameTime gameTime)
        {
            
            foreach (IEntity E in Entities)
            {
                E.Update(gameTime);
            }
            foreach (IEntity E in _toAdd)
            {
                
                Entities.Add(E);
                
                 
            }
            foreach (IEntity E in _toRemove)
            {
                Entities.Remove(E);
            }
            _toAdd.Clear();
            _toRemove.Clear();
        }

   

        public void AddEntity(IEntity entity)
        {
            _toAdd.Add(entity);
        }

        public void RemoveEntity(IEntity entity)
        {
            _toRemove.Add(entity);
        }
        /// <summary>
        /// Returns a list of entites of selected E
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <returns></returns>
        public List<E> GetEntities<E>() where E : IEntity 
        {
            return Entities.OfType<E>().ToList();
        }

        public void Clear()
        {
            foreach(IEntity e in Entities) 
            {
                _toRemove.Add(e);
            }
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (IEntity E in Entities)
            {
                E.Draw(spriteBatch, gameTime);
            }
        }
    }
}

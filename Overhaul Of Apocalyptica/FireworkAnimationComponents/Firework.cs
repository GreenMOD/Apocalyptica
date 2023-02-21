using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Overhaul_Of_Apocalyptica.Entities;

namespace Overhaul_Of_Apocalyptica.FireworkAnimationComponents
{
    public class Firework  :IEntity
    {
        public Particle _firework;

        private Vector2 _gravity;

        private bool _exploded = false;

        private Texture2D _texture;
        private Texture2D _particleTexture;
        
        private List<Particle> _particles = new List<Particle>();
        public Firework(Texture2D texture , Vector2 gravity , Texture2D partiText)
        {
            _texture = texture;           
            _particleTexture= partiText;

             _firework = new Particle(texture, true);

            _gravity = gravity;
        }

        public void Update(GameTime gameTime)
        {
            if (!_exploded)
            {
                _firework.ApplyForce(_gravity);
                _firework.Update();
                if (_firework.velocity.Y >= 0)
                {
                    _exploded = true;
                    Explode();
                }
            }
            else
            {
                foreach (Particle p in _particles)
                {
                    p.ApplyForce(_gravity);
                    p.Update();
                }
            }
            
           
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!_exploded)
            {
                _firework.Draw(spriteBatch, gameTime);
            }
            else
            {
                List<Particle> toRemove = new List<Particle>();
                foreach (Particle p in _particles)
                {
                    if (p.position.Y > 480)
                    {
                        toRemove.Add(p);
                    }
                    p.Draw(spriteBatch,gameTime);
                }
                foreach (Particle p in toRemove)
                {
                    _particles.Remove(p);
                }
                toRemove.Clear();
            }

        }

        public void Explode()
        {
            for (int i = 0; i < 100; i++)
            {
                Particle p = new Particle(_particleTexture, false);
                p.position = _firework.position;
                _particles.Add(p);
               
            }
        }

    }
}

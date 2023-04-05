using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Overhaul_Of_Apocalyptica.Entities;

namespace Overhaul_Of_Apocalyptica.FireworkAnimationComponents
{
    //Inspired by the Coding Train 
    //https://www.youtube.com/watch?v=CKeyIbT3vXI&t=819s&ab_channel=TheCodingTrain

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
        /// <summary>
        /// If the firework has not exploded it will apply the force vector gravity to the speed of the firework. If it has exploded then the list particles is maintained and updated. It will also exploded if its Y value has reached 0
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (!_exploded)
            {
                _firework.ApplyForce(_gravity);
                _firework.Update();
                if (_firework.velocity.Y >= 0) //When the speed in the Y direction is equal to 0 it is at equalibrium. Thus gravity and the velocity of the vector produce no net movement. The Firework has reached it maximum height. If continued the firework will fall, thus explode is called. 
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
        /// <summary>
        /// If the firework has not exploded, the 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
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
        /// <summary>
        /// Explodes and spawns large amounts of particles which fall down the screen
        /// </summary>
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

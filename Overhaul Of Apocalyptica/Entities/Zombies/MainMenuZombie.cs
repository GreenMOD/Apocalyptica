using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Overhaul_Of_Apocalyptica.FireworkAnimationComponents;
using System.Collections.Generic;
using Overhaul_Of_Apocalyptica.Entities;

namespace ExtensionTasks
{
    public class MainMenuZombie : IEntity
    {
        private Vector2 _position = Vector2.Zero;

        private MouseState _previousState;

        private Texture2D _texture;

        private Texture2D _particleTexture;

        private bool _dead = false;

        private List<Particle> _particles = new List<Particle>();

        public Vector2 Position { get { return _position; } set { _position = value; } } 

        public Rectangle Collision { get { return new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height); } }

        public MainMenuZombie(Vector2 position, Texture2D texture, Texture2D particleTexture)
        {
            Position = position;
            _texture = texture;
            _particleTexture = particleTexture;
        }

        public void Update(GameTime gameTime)
        {
            if (!_dead)
            {
                MouseState currentState = Mouse.GetState();

                if (Collision.Contains(currentState.Position))
                {
                    if ((currentState.LeftButton == ButtonState.Released) && (_previousState.LeftButton == ButtonState.Pressed))
                    {
                        _dead = true;
                        for (int i = 0; i < 20; i++)
                        {
                            Particle p = new Particle(_particleTexture, false);
                            p.position = Position;
                            _particles.Add(p);

                        }
                    }
                }
                _previousState = currentState;
            }
            else
            {
                foreach (var p in _particles)
                {
                    p.ApplyForce(new Vector2(0,0.5f));
                    p.Update();
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!_dead)
            {
                spriteBatch.Draw(_texture, new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height), Color.Yellow);
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
                    else
                    {
                        p.Draw(spriteBatch, gameTime);
                    }
                }
                foreach (Particle p in toRemove)
                {
                    _particles.Remove(p);
                }
                toRemove.Clear();
            }
        }
    }
}

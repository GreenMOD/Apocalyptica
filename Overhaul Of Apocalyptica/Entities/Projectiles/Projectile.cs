using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Overhaul_Of_Apocalyptica.Entities;

namespace Overhaul_Of_Apocalyptica.Entities
{ 
    abstract class Projectile: IEntity,ICollidable
    {
        public Sprite _sprite;
        public List<Rectangle> _frames;

        public Rectangle CollisionBox { get; set; }

        public double _timeFire; 
        public double FlightTime { get; set; }

        public Vector2 Position { get; set; }


        public Vector2 Speed { get; set; } 
        public Vector2 _acceleration;
        public float maxVelocity = 2.5f;
        public float maxForce = 1f;

        public bool IsDestroyed = false;

        public Ninja _ninja;

        




        public void ApplyForce(Vector2 force)
        {
            _acceleration = Vector2.Add(_acceleration, force);
        }

        public abstract void CheckCollision(GameTime gameTime);

        public abstract void Movement(GameTime gameTime);

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}

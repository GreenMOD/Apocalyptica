using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Overhaul_Of_Apocalyptica.Entities.Characters;

namespace Overhaul_Of_Apocalyptica.Entities
{ 
    public abstract class Projectile: IEntity, ICollidable
    {
        

        public Sprite ProjectSprite;
        public List<Rectangle> Frames;

        public Rectangle CollisionBox { get; set; }

        public double FlightTime { get; set; }

        public Vector2 Position { get; set; }


        public Vector2 Speed;
        public Vector2 Acceleration;
        public float MaxVelocity = 2.5f;
        public float MaxForce = 1f;

        public bool IsDestroyed = false;

        public Player Target;

        




        public void ApplyForce(Vector2 force)
        {
            Acceleration = Vector2.Add(Acceleration, force);
        }


        public abstract void Flight(GameTime gameTime);

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
        public abstract void Collided(GameTime gameTime, ICollidable collidedWith);
    }
}

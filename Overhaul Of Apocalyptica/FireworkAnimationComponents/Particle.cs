using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overhaul_Of_Apocalyptica.FireworkAnimationComponents
{
    public class Particle
    {
        #region Declarations

       public Vector2 position = Vector2.Zero;

       Vector2 _acceleration = new Vector2();
       
        Random rand  = new Random();
        public Vector2 velocity;

        Texture2D _texture;

        private Color _color = Color.White;

        #endregion

        #region Constructor


        public Particle(Texture2D texture ,bool isAFirework )
        {
            if (isAFirework)
            {
               velocity = new Vector2(0, -rand.Next(10,15));
            }
            else
            {
                velocity = new Vector2(rand.Next(-4,4),rand.Next(-4,4));
                velocity = Vector2.Multiply(velocity,(float)rand.Next(1, 6));
                
            }
            _texture = texture;

            position = new Vector2(rand.Next(0, 800), 480);

        }

        #endregion


        #region Methods

        public void Update()
        {
            velocity = Vector2.Add(velocity, _acceleration);
            position = Vector2.Add(position, velocity);
            _acceleration = Vector2.Zero;
        }

        public void Draw(SpriteBatch spriteBatch , GameTime gameTime)
        {
            spriteBatch.Draw(_texture, position, _color);
        }

        public void ApplyForce(Vector2 force)
        {
            _acceleration = Vector2.Add(force, _acceleration);
        }



        #endregion

    }
}

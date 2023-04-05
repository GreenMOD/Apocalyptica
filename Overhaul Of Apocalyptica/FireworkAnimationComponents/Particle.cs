using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Overhaul_Of_Apocalyptica.FireworkAnimationComponents
{
    public class Particle
    {
        //Inspired by the coding train
        //https://www.youtube.com/watch?v=CKeyIbT3vXI&list=PLRqwX-V7Uu6ZiZxtDDRCi6uhfTH4FilpH&index=32

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
        /// <summary>
        /// Applys the accerlation to the positon
        /// </summary>
        public void Update()
        {
            velocity = Vector2.Add(velocity, _acceleration);
            position = Vector2.Add(position, velocity);
            _acceleration = Vector2.Zero;
        }

        public void Draw(SpriteBatch spriteBatch , GameTime gameTime)
        {
            spriteBatch.Draw(_texture, new Rectangle((int)position.X,(int)position.Y,_texture.Width,_texture.Height), new Rectangle(0, 0, _texture.Width, _texture.Height), _color, 0F, Vector2.Zero, SpriteEffects.None, 1f);
        }
        /// <summary>
        /// Applies a force vector to the acceleration
        /// </summary>
        /// <param name="force"></param>
        public void ApplyForce(Vector2 force)
        {
            _acceleration = Vector2.Add(force, _acceleration);
        }



        #endregion

    }
}

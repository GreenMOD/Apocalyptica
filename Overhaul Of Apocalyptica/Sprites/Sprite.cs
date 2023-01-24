using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Overhaul_Of_Apocalyptica.Entities;

namespace Overhaul_Of_Apocalyptica
{
     class Sprite
    {
        public Texture2D Texture;

        protected List<Rectangle> _frames = new List<Rectangle>();
        protected List<Rectangle> _framesAlt = new List<Rectangle>();
        private int _frameWidth = 0;
        private int _frameHeight = 0;
        private int _currentFrame;
        private float _frameTime = 0.1f;
        private float timeForCurrentFrame = 0.0f;

        public int Frame { get { return _currentFrame; }set { _currentFrame = (int)MathHelper.Clamp(value, 0, _frames.Count - 1); } } //MathHelper prevents frame storing a value outside of frames length 
        public float FrameTime { get { return _frameTime; }set { _currentFrame = (int)MathHelper.Max( 0, value); } } //MathHelper. max makes the value stored greater then or equal to 0
        public Rectangle Source { get { return _frames[_currentFrame]; } }

        public Vector2 Position { get; set; }
        
        private Color _tintColor = Color.White;
        private float _rotation = 0.0f;

        public Color TintColor { get { return _tintColor; } set { _tintColor = value; } }
        public float Rotation { get { return _rotation; } set { _rotation = value; } }

        

        public Sprite(Texture2D texture, List<Rectangle> frames,Vector2 Position)
        {
            
            Texture = texture;


            _frames = frames;
            _frameWidth = _frames[0].Width;
            _frameHeight = _frames[0].Height;
            
            
        }
        public Sprite(Texture2D texture, List<Rectangle> frames,List<Rectangle> framesAlt, Vector2 Position)
        {
            Texture = texture;

            _frames = frames;
            _framesAlt = framesAlt;

            _frameWidth = _frames[0].Width;
            _frameHeight = _frames[0].Height;
        }




        public virtual void Draw(SpriteBatch spriteBatch,GameTime gameTime)
        {
            spriteBatch.Draw(Texture, Position, Source, _tintColor, _rotation, new Vector2(_frameWidth / 2, _frameHeight / 2),1.0f , SpriteEffects.None, 0.0f);
        }
        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
        {
            spriteBatch.Draw(Texture, Position, Source, _tintColor, _rotation, new Vector2(_frameWidth / 2, _frameHeight / 2),scale, SpriteEffects.None, 0.0f);
        }
        
        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime,string entityFacing)
        {
            if (entityFacing == "left")
            {
                _currentFrame = 0;
            } 
            if (entityFacing == "right")
            {
                _currentFrame = 1;
            } 
            if (entityFacing == "up")
            {
                _currentFrame = 2;
            } 
            if (entityFacing == "down")
            {
                _currentFrame = 3;
            }
            spriteBatch.Draw(Texture, Position, Source, _tintColor, _rotation, new Vector2(_frameWidth / 2, _frameHeight / 2), 1f, SpriteEffects.None, 0.0f);
        }
        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime,string entityFacing, float scale)
        {
            if (entityFacing == "left")
            {
                _currentFrame = 0;
            } 
            if (entityFacing == "right")
            {
                _currentFrame = 1;
            } 
            if (entityFacing == "up")
            {
                _currentFrame = 2;
            } 
            if (entityFacing == "down")
            {
                _currentFrame = 3;
            }
            spriteBatch.Draw(Texture, Position, Source, _tintColor, _rotation, new Vector2(_frameWidth / 2, _frameHeight / 2), scale, SpriteEffects.None, 0.0f);
        }

        public virtual void Update(GameTime gameTime, Vector2 newPosition)
        {
            

            Position = newPosition;
            //float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //timeForCurrentFrame += elapsed;


            //if (timeForCurrentFrame >= frameTime)
            //{
            //    currentFrame = (currentFrame + 1) % (_frames.Count);//Creates a loop so that once current frame reaches frame "5" which wont exist it will restart at the beginning
            //}

            //TDOD SET UP HOW THE ZOMBIE WILL CHANGE DIRECTION OF FACE
            _frameWidth = _frames[_currentFrame].Width;
            _frameHeight = _frames[_currentFrame].Height;
        }
    }
}

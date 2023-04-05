using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Overhaul_Of_Apocalyptica
{
    //Inspired by XNA 4.0 Game Development by Example by Kurt Jaegers 

    public class Sprite
    {

        private string _previousFacing = "";



        public Texture2D Texture;

        protected List<Rectangle> _frames = new List<Rectangle>();
        protected List<Rectangle> _framesAlt = new List<Rectangle>();
        private int _frameWidth = 0;
        private int _frameHeight = 0;
        private int _currentFrame;
        private float _frameMaxTime = 0.1f;

        private float _timeForCurrentFrame = 0;
        public float TimeForCurrentFrame { get { return _timeForCurrentFrame; } }
        public int Frame { get { return _currentFrame; }set { _currentFrame = (int)MathHelper.Clamp(value, 0, _frames.Count - 1); } } //MathHelper prevents frame storing a value outside of frames length 
        public float FrameTime { get { return _frameMaxTime; }set { _frameMaxTime = (float)value; } } 
        public Rectangle Source { get { return _frames[_currentFrame]; } }

        public Vector2 Position { get; set; }
        
        private Color _tintColor = Color.White;
        private float _rotation = 0.0f;

        public Color TintColor { get { return _tintColor; } set { _tintColor = value; } }
        public float Rotation { get { return _rotation; } set { _rotation = value; } }

        public string EntityFacing = "";

        public Sprite(Texture2D texture, List<Rectangle> frames,Vector2 position)
        {
            
            Texture = texture;


            _frames = frames;
            _frameWidth = _frames[0].Width;
            _frameHeight = _frames[0].Height;

            EntityFacing = "";
            Position = position;
            
        }
        public Sprite(Texture2D texture, List<Rectangle> frames, Vector2 position, string facing)
        {

            Texture = texture;


            _frames = frames;
            _frameWidth = _frames[0].Width;
            _frameHeight = _frames[0].Height;

            EntityFacing = facing;
            Position = position;
        }
        public Sprite(Texture2D texture, List<Rectangle> frames,List<Rectangle> framesAlt, Vector2 position)
        {
            Texture = texture;

            _frames = frames;
            _framesAlt = framesAlt;

            _frameWidth = _frames[0].Width;
            _frameHeight = _frames[0].Height;
            Position = position;
        }



        /// <summary>
        /// Displays the sprite at its default size using the passed in SpriteBatch class
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public virtual void Draw(SpriteBatch spriteBatch,GameTime gameTime)
        {
            spriteBatch.Draw(Texture, Position, Source, _tintColor, _rotation, new Vector2(_frameWidth / 2, _frameHeight / 2),1f, SpriteEffects.None, 1.0f);
        }
        /// <summary>
        /// Multiplies the size of the sprite by a parameter scale and then displays the sprite
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        /// <param name="scale"></param>
        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
        {
            spriteBatch.Draw(Texture, Position, Source, _tintColor, _rotation,new Vector2(0,0),scale, SpriteEffects.None, 0.0f);
        }
        /// <summary>
        /// Using a larger list of frames directional idle animations are created. The current frame is changed when the next animation is needed
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            if (EntityFacing != _previousFacing)
            {
                _timeForCurrentFrame = FrameTime;
            }
            else
            {
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                _timeForCurrentFrame += elapsed;
            }
            _previousFacing = EntityFacing;
            if (_frames.Count > 1)
            {
                if (_timeForCurrentFrame >= FrameTime)
                {
                    switch (EntityFacing.ToLower())
                    {
                        case "left":
                            if (_frames[_currentFrame] == _frames[0])
                            {
                                _currentFrame = 1; //alt
                                _timeForCurrentFrame = 0;
                            }
                            else
                            {
                                _currentFrame = 0; //normal
                                _timeForCurrentFrame = 0;
                            }
                            break;
                        case "right":
                            if (_frames[_currentFrame] == _frames[2])
                            {
                                _currentFrame = 3;//alt
                                _timeForCurrentFrame = 0;
                            }
                            else
                            {
                                _currentFrame = 2;//normal
                                _timeForCurrentFrame = 0;
                            }
                            break;
                        case "up":
                            if (_frames[_currentFrame] == _frames[4])
                            {
                                _currentFrame = 5; //alt
                                _timeForCurrentFrame = 0;
                            }
                            else
                            {
                                _currentFrame = 4; //normal
                                _timeForCurrentFrame = 0;
                            }
                            break;
                        case "down":
                            if (_frames[_currentFrame] == _frames[6])
                            {
                                _currentFrame = 7; //alt
                                _timeForCurrentFrame = 0;
                            }
                            else
                            {
                                _currentFrame = 6; //normal
                                _timeForCurrentFrame = 0;
                            }
                            break;
                        case "":
                            _currentFrame = _currentFrame;
                            break;
                    }
                }
                
            }    

       
            _frameWidth = _frames[_currentFrame].Width;
            _frameHeight = _frames[_currentFrame].Height;
        }
        /// <summary>
        /// Updates the position of the sprite using a parameter newPostion.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="newPosition"></param>
        public virtual void Update(GameTime gameTime, Vector2 newPosition)
        {
            Position = newPosition;
            _frameWidth = _frames[_currentFrame].Width;
            _frameHeight = _frames[_currentFrame].Height;
        }

    }
}

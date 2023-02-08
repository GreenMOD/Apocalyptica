using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Overhaul_Of_Apocalyptica.Sprites
{
    public class Animation
    {
        private Texture2D _texture;
        private int _frameWidth;
        private int _frameHeight;

        private float _frameTime= 0f;
        private float _frameDelay = 0.05f;

        private int _currentFrame;

        private bool _isLooping = true;
        private bool _hasFinished = false;

        private string _name;
        private string _nextAnimation;


        public int FrameWidth { get { return _frameWidth; } set { _frameWidth = value; } }

        public int FrameHeight { get { return _frameHeight;} set { _frameHeight = value; } }

        public Texture2D Texture { get { return _texture; } set { _texture = value; } }

        public string Name { get { return _name; } set { _name = value; } }
        
        public string NewAnimation { get { return _nextAnimation; } set { _nextAnimation = value; } }

        public bool IsLooping { get { return _isLooping; } set { _isLooping = value;  } }

        public bool HasFinished { get { return _hasFinished;} set { _hasFinished = value; } }
        
        public int FrameCount { get { return _texture.Width / _frameWidth; } }

        public float FrameLength { get { return _frameDelay;} set { _frameDelay = value; } }

        public Rectangle Frame { get { return new Rectangle(_currentFrame * _frameWidth, 0, _frameWidth, FrameHeight); } }

        public Animation(Texture2D texture, int frameWidth, int frameHeight, string name)
        {
            Texture = texture;
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
            Name = name;
        }

        public void Play()
        {
            _currentFrame = 0;
            HasFinished = false;
        }
        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _frameTime += elapsed;

            if (_frameTime >= _frameDelay)
            {
                _currentFrame++;
                if (_currentFrame >= FrameCount)
                {
                    if (IsLooping)
                    {
                        _currentFrame = 0;
                    }
                    else
                    {
                        _currentFrame = FrameCount - 1;
                        HasFinished = true;
                    }
                }

                _frameTime = 0;
            }
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            
        }
    }
}

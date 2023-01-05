﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Overhaul_Of_Apocalyptica.Entities;
using Overhaul_Of_Apocalyptica.Entities.Characters;
using Overhaul_Of_Apocalyptica.Controls;
using System;
using System.Collections.Generic;

namespace Overhaul_Of_Apocalyptica
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private EntityManager entityManager;
        private Texture2D NinjaSpriteSheet;
        private Texture2D ZombieSheet;
        private Texture2D SoldierSpriteSheet;
        private Texture2D DessertMap;
        private Texture2D WaveCounterSpriteSheet;
        private Texture2D HeartSpriteSheet;
        private Texture2D projectileSpriteSheet;
        private Texture2D soldierSpriteSheet;
        private WaveManager waveManager;
        private enum tempGameState {PLAYING, MENU, QUIT };
        private tempGameState _gameState;

        private List<IEntity> _menuComponents;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            entityManager = new EntityManager();
            
        }

        protected override void Initialize()
        {

            IsMouseVisible = true;
            _gameState = tempGameState.MENU;
            Button newGameButton = new Button(Content.Load<Texture2D>(@"Controls/ButtonTexture"), Content.Load<SpriteFont>(@"Fonts/ButtonFont"))
            {
                Text = ("New Game"),
                Position = new Vector2(Window.ClientBounds.Width/2- 79 ,Window.ClientBounds.Height/2)
                
            };
            newGameButton.Click += NewGameButton_Click;
            Button loadGameButton = new Button(Content.Load<Texture2D>(@"Controls/ButtonTexture"), Content.Load<SpriteFont>(@"Fonts/ButtonFont"))
            {
                Text = ("Load Game"),
                Position = new Vector2(Window.ClientBounds.Width / 2 - 79, ((Window.ClientBounds.Height / 2) + 50))

            };
            loadGameButton.Click += LoadGameButton_Click;
            Button optionsButton = new Button(Content.Load<Texture2D>(@"Controls/ButtonTexture"), Content.Load<SpriteFont>(@"Fonts/ButtonFont"))
            {
                Text = ("Options"),
                Position = new Vector2(Window.ClientBounds.Width / 2 - 79, ((Window.ClientBounds.Height / 2) + 100))

            };
            optionsButton.Click += OptionsButton_Click;
            Button quitButton = new Button(Content.Load<Texture2D>(@"Controls/ButtonTexture"), Content.Load<SpriteFont>(@"Fonts/ButtonFont"))
            {
                Text = ("Quit"),
                Position = new Vector2(Window.ClientBounds.Width / 2 - 79, ((Window.ClientBounds.Height / 2) + 150))

            };
            quitButton.Click += QuitButton_Click;
            _menuComponents = new List<IEntity>()
            {
                newGameButton,
                loadGameButton,
                optionsButton,
                quitButton
            };

            
            base.Initialize();
        }
        private void NewGameButton_Click(object sender, EventArgs e)
        {
            entityManager.Clear();

            _gameState = tempGameState.PLAYING;

            GameTime gameTime = new GameTime();

            Player player1 = new Soldier(SoldierSpriteSheet, HeartSpriteSheet, soldierSpriteSheet, gameTime);
            entityManager.AddEntity(player1);


            waveManager = new WaveManager(ZombieSheet, player1, entityManager, WaveCounterSpriteSheet, projectileSpriteSheet);


            player1.Activate();

            foreach (Zombie z in waveManager.zombiesSpawned)
            {
                entityManager.AddEntity(z);
            }

        }

        private void LoadGameButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
       
        private void OptionsButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        private void QuitButton_Click(object sender, EventArgs e)
        {
            Exit();
        }



        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            NinjaSpriteSheet = Content.Load<Texture2D>(@"SpriteSheets/NinjaSpriteSheet");
            ZombieSheet = Content.Load<Texture2D>(@"SpriteSheets/ApocZombieSpriteSheet");
            DessertMap = Content.Load<Texture2D>(@"SpriteSheets/Dessert1");
            SoldierSpriteSheet = Content.Load<Texture2D>(@"SpriteSheets/SoldierSpriteSheet2");
            WaveCounterSpriteSheet = Content.Load<Texture2D>(@"SpriteSheets/WaveCounterSprite");
            HeartSpriteSheet = Content.Load<Texture2D>(@"SpriteSheets/Heart");
            projectileSpriteSheet = Content.Load<Texture2D>(@"SpriteSheets/captainProjectile");
            soldierSpriteSheet = Content.Load<Texture2D>(@"SpriteSheets/SoldierBulletSprite");

            foreach (var b in _menuComponents)
            {
                entityManager.AddEntity(b);
            }


        }

        protected override void Update(GameTime gameTime)
        {
            if (_gameState== tempGameState.PLAYING)
            {

                waveManager.Update(gameTime);
                if (waveManager.zombiesToAdd.Count != 0)
                {
                    foreach (Zombie z in waveManager.zombiesToAdd)
                    {
                        entityManager.AddEntity(z);
                    }
                }
                base.Update(gameTime);

                entityManager.Update(gameTime);
                if (waveManager.isRunning == false) //not the most elegant of solutions however will be the best for the time being and in the long term for when game states are added
                {
                    waveManager.Intialise();
                }
            }
            else if(_gameState == tempGameState.MENU)
            {
                entityManager.Update(gameTime);
            }
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
           
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            
           

            if (_gameState == tempGameState.PLAYING)
            {
                _spriteBatch.Draw(DessertMap, new Rectangle(0,0,Window.ClientBounds.Width,Window.ClientBounds.Height), Color.Yellow);
                entityManager.Draw(_spriteBatch, gameTime);
                waveManager.Draw(_spriteBatch, gameTime);

            }
            else
            {
                entityManager.Draw(_spriteBatch, gameTime);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

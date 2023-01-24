using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Overhaul_Of_Apocalyptica.Entities;
using Overhaul_Of_Apocalyptica.Entities.Characters;
using Overhaul_Of_Apocalyptica.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using Overhaul_Of_Apocalyptica.Events;
using System.Configuration;

namespace Overhaul_Of_Apocalyptica
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private EntityManager _entityManager;
        private Texture2D _ninjaSpriteSheet;
        private Texture2D _zombieSheet;
        private Texture2D _soldierSpriteSheet;
        private Texture2D _dessertMap;
        private Texture2D _waveCounterSpriteSheet;
        private Texture2D _heartSpriteSheet;
        private Texture2D _projectileSpriteSheet;
        private Texture2D _soldierSpriteSheet2;
        private WaveManager _waveManager;
        private CollisionManager _collisionManager;
        private enum _GameState { PLAYING, MENU, INITIALISE, SAVESELECT };
        private _GameState _gameState;

        private List<IEntity> _menuComponents;

        private List<SaveSlot> _saveSlots;
        private List<Button> _saveButtons;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _entityManager = new EntityManager();
            _collisionManager = new CollisionManager(new List<ICollidable>());

            _collisionManager.Collision += Object_Collided;

        }

        private void Object_Collided(object sender, EventArgs e)
        {

        }


        protected override void Initialize()
        {

            IsMouseVisible = true;
            _gameState = _GameState.MENU;
            _saveSlots = new List<SaveSlot>()
            {
                new SaveSlot("PreviousSaves/Save1.txt"),
                new SaveSlot("PreviousSaves/Save2.txt"),
                new SaveSlot("PreviousSaves/Save3.txt"),
                new SaveSlot("PreviousSaves/Save4.txt")
            };

            Button titleButton = new Button(Content.Load<Texture2D>(@"Controls/TitleScreen"), Content.Load<SpriteFont>(@"Fonts/ButtonFont"))
            {
                Position = new Vector2(200, 100)

            };
            titleButton.Click += TitleButton_Click;

            Button newGameButton = new Button(Content.Load<Texture2D>(@"Controls/ButtonTexture"), Content.Load<SpriteFont>(@"Fonts/ButtonFont"))
            {
                Text = ("New Game"),
                Position = new Vector2(Window.ClientBounds.Width / 2 - 79, Window.ClientBounds.Height / 2)

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
                titleButton,
                newGameButton,
                loadGameButton,
                optionsButton,
                quitButton
            };


            base.Initialize();
        }

        private void TitleButton_Click(Button sender, ButtonClickedEventArgs e)
        {

        }

        private void NewGameButton_Click(Button sender, ButtonClickedEventArgs e)
        {
            _entityManager.Clear();

            _gameState = _GameState.SAVESELECT;

            GameTime gameTime = new GameTime();

            SaveFileMenu();

            //draw the save slots now
            foreach (IEntity E in _saveButtons)
            {
                _entityManager.AddEntity(E);
            }

        }

        private void LoadGameButton_Click(Button sender, ButtonClickedEventArgs e)
        {
            _entityManager.Clear();

            _gameState = _GameState.INITIALISE;
            using (StreamReader sr = new StreamReader(@"PreviousSaves/Save1.txt"))
            {
                sr.ReadLine();
                string statusOfFile = sr.ReadLine().Substring(8);

                if (statusOfFile == "Used")
                {
                    string Name = sr.ReadLine().Substring(6);
                    string Class = sr.ReadLine().Substring(7);
                    string Wave = sr.ReadLine().Substring(6);

                    GameTime gameTime = new GameTime();

                    Player player1;
                    switch (Class)
                    {
                        case "Soldier":
                            Player soldier = new Soldier(_soldierSpriteSheet, _heartSpriteSheet, _soldierSpriteSheet2, gameTime);
                            player1 = soldier;
                            break;
                        case "Ninja":
                            Player ninja = new Ninja(_ninjaSpriteSheet, _heartSpriteSheet);
                            player1 = ninja;
                            break;
                        default:
                            player1 = new Soldier(_soldierSpriteSheet, _heartSpriteSheet, _soldierSpriteSheet2, gameTime);
                            break;
                    }

                    _entityManager.AddEntity(player1);
                    _collisionManager.AddCollidable(player1);
                    player1.Activate();





                }
            }
        }

        private void OptionsButton_Click(Button sender, ButtonClickedEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void QuitButton_Click(Button sender, ButtonClickedEventArgs e)
        {
            Exit();
        }
        /// <summary>
        /// Loads all save files and makes them into buttons which are added to the entity manager
        /// </summary>
        private void SaveFileMenu()
        {
            int count = 0; // used to apply a modifier on to the x position or the y position.
            _saveButtons = new List<Button>();

            int[][] slotXY = new int[2][];

            //TODO USING BINARY CALULATION MAKE THE SLOTS POSITION CORRECTLY
            for (int y = 0; y < 2; y++)
            {
                
                for (int x = 0; x < 2; x++)
                {
                    if (_saveSlots[x + (y+count)].CurrentWave != -1)
                    {
                        //_saveSlots[x + y].Status + " " + + _saveSlots[x +y].PlayerClass + " " + _saveSlots[x + y].CurrentWave
                        string text = ( _saveSlots[x + y+ count].SlotName);
                        Button saveSlot = new Button(Content.Load<Texture2D>(@"Controls/ButtonTexture"), Content.Load<SpriteFont>(@"Fonts/ButtonFont"))
                        {
                            Text = (text),
                            Position = new Vector2(Window.ClientBounds.Width / 8 + (x * 450), Window.ClientBounds.Height / 4 + (y * 200))
                        };
                        //Event handler to a (saveSlot - button) 
                        saveSlot.Click += SaveSlot_Click; 
                        _saveButtons.Add(saveSlot);
                    }
                    else
                    {
                        Button emptySlot = new Button(Content.Load<Texture2D>(@"Controls/ButtonTexture"), Content.Load<SpriteFont>(@"Fonts/ButtonFont"))
                        {
                            Text = (_saveSlots[x + y + count].Status),
                            Position = new Vector2(Window.ClientBounds.Width / 8 + (x * 450), Window.ClientBounds.Height / 4 + (y * 200))

                        };
                        emptySlot.Click += SaveSlot_Click;
                        _saveButtons.Add(emptySlot);
                    }
                }
                count++;

            }
        }

        /// <summary>
        /// What happens once clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveSlot_Click(Button sender, EventArgs e)
        {
            GameTime gameTime = new GameTime();
            if (sender.Text.Contains("Empty"))
            {
                _entityManager.Clear();
                CreateCharacter();
            }
            else
            {
                _gameState = _GameState.INITIALISE;

                
                Player player1;
                switch (_saveSlots[_saveButtons.IndexOf(sender)].PlayerClass)
                {
                    case "Soldier":
                        Player soldier = new Soldier(_soldierSpriteSheet, _heartSpriteSheet, _soldierSpriteSheet2, gameTime);
                        player1 = soldier;
                        break;
                    case "Ninja":
                        Player ninja = new Ninja(_ninjaSpriteSheet, _heartSpriteSheet);
                        player1 = ninja;
                        break;
                    default:
                        player1 = new Soldier(_soldierSpriteSheet, _heartSpriteSheet, _soldierSpriteSheet2, gameTime);
                        break;
                }

                _entityManager.AddEntity(player1);
                _collisionManager.AddCollidable(player1);
                player1.Activate();
                _entityManager.Clear(); 
            }
        }
        private void CreateCharacter()
        {

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _ninjaSpriteSheet = Content.Load<Texture2D>(@"SpriteSheets/NinjaSpriteSheet");
            _zombieSheet = Content.Load<Texture2D>(@"SpriteSheets/ApocZombieSpriteSheet");
            _dessertMap = Content.Load<Texture2D>(@"SpriteSheets/Dessert1");
            _soldierSpriteSheet = Content.Load<Texture2D>(@"SpriteSheets/SoldierSpriteSheet2");
            _waveCounterSpriteSheet = Content.Load<Texture2D>(@"SpriteSheets/WaveCounterSprite");
            _heartSpriteSheet = Content.Load<Texture2D>(@"SpriteSheets/Heart");
            _projectileSpriteSheet = Content.Load<Texture2D>(@"SpriteSheets/captainProjectile");
            _soldierSpriteSheet2 = Content.Load<Texture2D>(@"SpriteSheets/SoldierBulletSprite");
            foreach (var b in _menuComponents)
            {
                _entityManager.AddEntity(b);
            }



        }

        protected override void Update(GameTime gameTime)
        {
            if (_gameState == _GameState.PLAYING)
            {

                _waveManager.Update(gameTime);

                foreach (Zombie z in _waveManager.ZombiesToAdd)
                {
                    _entityManager.AddEntity(z);
                    _collisionManager.AddCollidable(z);
                }



                base.Update(gameTime);

                _entityManager.Update(gameTime);
                _collisionManager.Update(gameTime);

            }
            else if (_gameState == _GameState.MENU)
            {
                _entityManager.Update(gameTime);
            }
            else if (_gameState == _GameState.INITIALISE)
            {
                _entityManager.Update(gameTime);
                _collisionManager.Update(gameTime);
                _gameState = _GameState.PLAYING;
                _waveManager = new WaveManager(_zombieSheet, _entityManager, _collisionManager, _waveCounterSpriteSheet, _projectileSpriteSheet);

                _waveManager.Intialise();



            }
            else if (_gameState == _GameState.SAVESELECT)
            {
                _entityManager.Update(gameTime);
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (gameTime.IsRunningSlowly == true)
            {
                Debug.WriteLine("Game running slowly");
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();



            if (_gameState == _GameState.PLAYING)
            {
                _spriteBatch.Draw(_dessertMap, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.Yellow);
                _entityManager.Draw(_spriteBatch, gameTime);
                _waveManager.Draw(_spriteBatch, gameTime);

            }
            else
            {
                _entityManager.Draw(_spriteBatch, gameTime);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
    
}



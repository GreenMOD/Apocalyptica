using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Overhaul_Of_Apocalyptica.Entities;
using Overhaul_Of_Apocalyptica.Entities.Characters;
using Overhaul_Of_Apocalyptica.Controls;
using System.Collections.Generic;
using System.Diagnostics;
using Overhaul_Of_Apocalyptica.FireworkAnimationComponents;

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
        private Texture2D _soldierBulletSprite;
        private Texture2D _shurikenSprite;
        private Texture2D _heavySpriteSheet1;
        private Texture2D _soldierIcon;
        private Texture2D _ninjaIcon;
        private Texture2D _heavyIcon;
        private Texture2D _fireworkTexture;
        private Texture2D _particleTexture;
        private Texture2D _gameOverTexture;

        private WaveManager _waveManager;
        private CollisionManager _collisionManager;
        private enum _GameState { PLAYING, MENU, INITIALISE, SAVELOAD , SAVENEW ,OPTIONS,KEYCHANGE, GAMEOVER, GAMEWON};
        private _GameState _gameState;

        private List<Button> _menuComponents;

        private List<SaveSlot> _saveSlots;
        private List<Button> _saveButtons;
        private int _currentSaveSlotIndex;

        private int _waveStartIndex = 0;

        private List<Keys> _movementKeys = new List<Keys>() {Keys.W,Keys.S,Keys.A,Keys.D };    

        private Player _player1;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _entityManager = new EntityManager();
            _collisionManager = new CollisionManager(new List<ICollidable>());
        }

        /// <summary>
        /// Outputs a menu of buttons to select from: New Game, Load Game, Options, Quit Game
        /// </summary>
        protected override void Initialize()
        {

            IsMouseVisible = true;

            DisplayTitleScreen();

            base.Initialize();
        }
        #region Main Menu
        private void DisplayTitleScreen()
        {
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
            _menuComponents = new List<Button>()
            {
                titleButton,
                newGameButton,
                loadGameButton,
                optionsButton,
                quitButton
            };

        }
        private void TitleButton_Click(Button sender)
        {

            Firework f = new Firework(_fireworkTexture, new Vector2(0, 0.2f), _particleTexture);

            _entityManager.AddEntity(f);
        }
        /// <summary>
        /// a save file menu showing 4 slots. Each slot can be overriden if they contain an itwm or they can open an empty file for character creation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewGameButton_Click(Button sender)
        {
            _entityManager.Clear();

            _gameState = _GameState.SAVENEW;

            SaveFileMenu();

        }
        /// <summary>
        /// Outputs a save file menu showing 4 slots. Each slot can be loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadGameButton_Click(Button sender)
        {
            _entityManager.Clear();

            _gameState = _GameState.SAVELOAD;

            SaveFileMenu();
        }
        private void OptionsButton_Click(Button sender)
        {
            _entityManager.Clear();

            _gameState = _GameState.OPTIONS;

            Button upButton = new Button(Content.Load<Texture2D>(@"Controls/ButtonTexture"), Content.Load<SpriteFont>(@"Fonts/ButtonFont"))
            {
                
                Position = new Vector2(Window.ClientBounds.Width / 2 - 80 , 100),
                Text = _movementKeys[0].ToString()

            };
            upButton.Click += KeyMap_Clicked;

            Button downButton = new Button(Content.Load<Texture2D>(@"Controls/ButtonTexture"), Content.Load<SpriteFont>(@"Fonts/ButtonFont"))
            {
                Position = new Vector2(Window.ClientBounds.Width / 2 - 80, 150),
                Text = _movementKeys[1].ToString()

            };
            downButton.Click += KeyMap_Clicked;

            Button leftButton = new Button(Content.Load<Texture2D>(@"Controls/ButtonTexture"), Content.Load<SpriteFont>(@"Fonts/ButtonFont"))
            {
                Position = new Vector2(Window.ClientBounds.Width / 2 - 240, 150),
                Text = _movementKeys[2].ToString()

            };
            leftButton.Click += KeyMap_Clicked;

            Button rightButton = new Button(Content.Load<Texture2D>(@"Controls/ButtonTexture"), Content.Load<SpriteFont>(@"Fonts/ButtonFont"))
            {
                Position = new Vector2(Window.ClientBounds.Width / 2 + 80, 150),
                Text = _movementKeys[3].ToString()

            };
            rightButton.Click += KeyMap_Clicked;

            _menuComponents = new List<Button>
            {
                upButton,
                downButton,
                leftButton,
                rightButton
            };

            _entityManager.AddEntity(upButton);
            _entityManager.AddEntity(downButton);
            _entityManager.AddEntity(leftButton);
            _entityManager.AddEntity(rightButton);
        }
     
        private void QuitButton_Click(Button sender)
        {
            Exit();
        }
        #endregion

        #region Save Menu
        /// <summary>
        /// Loads all save files and makes them into buttons which are added to the entity manager
        /// </summary>
        private void SaveFileMenu()
        {
            int count = 0; // used to apply a modifier on to the x position or the y position.
            _saveButtons = new List<Button>();

            int[][] slotXY = new int[2][];

            for (int y = 0; y < 2; y++)
            {

                for (int x = 0; x < 2; x++)
                {
                    if (_saveSlots[x + (y + count)].CurrentWave != -1)
                    {
                        //_saveSlots[x + y].Status + " " + + _saveSlots[x +y].PlayerClass + " " + _saveSlots[x + y].CurrentWave
                        string text = (_saveSlots[x + y + count].SlotName);
                        Button saveSlot = new Button(Content.Load<Texture2D>(@"Controls/ButtonTexture"), Content.Load<SpriteFont>(@"Fonts/ButtonFont"))
                        {
                            Text = (_saveSlots[x + y + count].Status),
                            Position = new Vector2(Window.ClientBounds.Width / 8 + (x * 450), Window.ClientBounds.Height / 4 + (y * 200))
                        };
                        //Event handler to a (saveSlot - button) 
                        saveSlot.Click += SaveSlot_Clicked;
                        _saveButtons.Add(saveSlot);
                    }
                    else
                    {
                        Button emptySlot = new Button(Content.Load<Texture2D>(@"Controls/ButtonTexture"), Content.Load<SpriteFont>(@"Fonts/ButtonFont"))
                        {
                            Text = (_saveSlots[x + y + count].Status),
                            Position = new Vector2(Window.ClientBounds.Width / 8 + (x * 450), Window.ClientBounds.Height / 4 + (y * 200))

                        };
                        emptySlot.Click += SaveSlot_Clicked;
                        _saveButtons.Add(emptySlot);
                    }
                }
                count++;

            }
            foreach (IEntity E in _saveButtons)
            {
                _entityManager.AddEntity(E);
            }
        }

        /// <summary>
        /// What happens once clicked. It will determine wehter to load a save from the file or to override another save. 
        /// </summary>
        /// <param name="sender">Button that is being selected</param>
        /// <param name="e"></param>
        private void SaveSlot_Clicked(Button sender)
        {
            GameTime gameTime = new GameTime();
            if (sender.Text.Contains("Empty"))
            {
                _entityManager.Clear();
                _currentSaveSlotIndex = _saveButtons.IndexOf(sender);
                CreateCharacter();
            }
            else if (_gameState == _GameState.SAVELOAD)
            {
                _gameState = _GameState.INITIALISE;
                _currentSaveSlotIndex = _saveButtons.IndexOf(sender);
                switch (_saveSlots[_currentSaveSlotIndex].PlayerClass)
                {
                    case "Soldier":
                        Player soldier = new Soldier(_soldierSpriteSheet, _heartSpriteSheet, _soldierBulletSprite, gameTime) { MovementKeys = _movementKeys };
                        _player1 = soldier;
                        break;
                    case "Ninja":
                        Player ninja = new Ninja(_ninjaSpriteSheet, _heartSpriteSheet, _shurikenSprite, gameTime) { MovementKeys = _movementKeys };
                        _player1 = ninja;
                        break;
                    case "Heavy":
                        Player heavy = new Heavy(_heavySpriteSheet1, _heartSpriteSheet, _soldierBulletSprite, gameTime) { MovementKeys = _movementKeys };
                        _player1 = heavy;
                        break;
                    default:
                        _player1 = new Soldier(_soldierSpriteSheet, _heartSpriteSheet, _soldierBulletSprite, gameTime) { MovementKeys = _movementKeys };
                        break;
                }

                _entityManager.AddEntity(_player1);
                _collisionManager.AddCollidable(_player1);
                _player1.Activate();
                _entityManager.Clear();
                _waveStartIndex = _saveSlots[_currentSaveSlotIndex].CurrentWave;
            }
            else if (_gameState == _GameState.SAVENEW)
            {
                if (sender.hasBeenClicked)
                {
                    _entityManager.Clear();
                    _currentSaveSlotIndex = _saveButtons.IndexOf(sender);
                    CreateCharacter();
                }
                else
                {
                    sender.Text = ("Confirm Override?");
                }

            }
        }
        #endregion

        #region Character Creation

        /// <summary>
        /// Opens the character creation menu then saves the character info into the save slot
        /// </summary>
        /// <param name="saveSlot">The save slot that will store this new game</param>
        private void CreateCharacter()
        {
            
            Button ninja = new Button(_ninjaIcon, Content.Load<SpriteFont>(@"Fonts/ButtonFont"))
            {
                Text = "Ninja",
                Position = new Vector2(0, (Window.ClientBounds.Height / 2)),
                TextColor = Color.Transparent

        };
            ninja.Click += CharacterButton_Clicked;

            _entityManager.AddEntity(ninja);
            Button soldier = new Button(_soldierIcon, Content.Load<SpriteFont>(@"Fonts/ButtonFont"))
            {
                Text = "Soldier",
                Position = new Vector2(260, (Window.ClientBounds.Height / 2)),
                TextColor = Color.Transparent

            };
            soldier.Click += CharacterButton_Clicked;
            _entityManager.AddEntity(soldier);
            Button heavy = new Button(_heavyIcon, Content.Load<SpriteFont>(@"Fonts/ButtonFont"))
            {
                Text = "Heavy",
                Position = new Vector2(520, (Window.ClientBounds.Height / 2)),
                TextColor = Color.Transparent

            };
            heavy.Click += CharacterButton_Clicked;
            _entityManager.AddEntity(heavy);
        }
        private void CharacterButton_Clicked(Button button)
        {
            GameTime gameTime = new GameTime();
            switch (button.Text)
            {
                case "Soldier":
                    Player soldier = new Soldier(_soldierSpriteSheet, _heartSpriteSheet, _soldierBulletSprite, gameTime) { MovementKeys = _movementKeys };
                    _player1 = soldier;
                    _saveSlots[_currentSaveSlotIndex].Status = "Used";
                    _saveSlots[_currentSaveSlotIndex].PlayerName = "Player1";
                    _saveSlots[_currentSaveSlotIndex].PlayerClass = "Soldier";
                    _saveSlots[_currentSaveSlotIndex].CurrentWave = 1;
                    _saveSlots[_currentSaveSlotIndex].OverrideSave();

                    break;
                case "Ninja":
                    Player ninja = new Ninja(_ninjaSpriteSheet, _heartSpriteSheet,_shurikenSprite,gameTime) { MovementKeys = _movementKeys };
                    _player1 = ninja;
                    _saveSlots[_currentSaveSlotIndex].Status = "Used";
                    _saveSlots[_currentSaveSlotIndex].PlayerName = "Player1";
                    _saveSlots[_currentSaveSlotIndex].PlayerClass = "Ninja";
                    _saveSlots[_currentSaveSlotIndex].CurrentWave = 1;
                    _saveSlots[_currentSaveSlotIndex].OverrideSave();

                    break;
                case "Heavy":
                    Player heavy = new Heavy(_heavySpriteSheet1, _heartSpriteSheet, _soldierBulletSprite, gameTime) { MovementKeys = _movementKeys };
                    _player1 = heavy;
                    _saveSlots[_currentSaveSlotIndex].Status = "Used";
                    _saveSlots[_currentSaveSlotIndex].PlayerName = "Player1";
                    _saveSlots[_currentSaveSlotIndex].PlayerClass = "Heavy";
                    _saveSlots[_currentSaveSlotIndex].CurrentWave = 1;
                    _saveSlots[_currentSaveSlotIndex].OverrideSave();
                    break;
                default:
                    _player1 = new Soldier(_soldierSpriteSheet, _heartSpriteSheet, _soldierBulletSprite, gameTime) { MovementKeys = _movementKeys };
                    break;
            }
            _entityManager.AddEntity(_player1);
            _collisionManager.AddCollidable(_player1);
            _player1.Activate();
            _entityManager.Clear();
            _gameState = _GameState.INITIALISE;
            
        }
        #endregion

        #region Options
        private void KeyMap_Clicked(Button sender)
        {
            sender.Text = ("Press Key");
            _gameState = _GameState.KEYCHANGE;
        }
        private void ChangeKey(Keys now)
        {
            foreach (Button b in _menuComponents)
            {
                if (b.hasBeenClicked)
                {
                    b.hasBeenClicked = false;
                    b.Text = now.ToString();
                    _movementKeys[_menuComponents.IndexOf(b)] = now;
                }
            }
            _gameState = _GameState.OPTIONS;
        }
        #endregion
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            _ninjaSpriteSheet = Content.Load<Texture2D>(@"SpriteSheets/NinjaSpriteSheet");
            _shurikenSprite = Content.Load<Texture2D>(@"SpriteSheets/Shuriken");
            _ninjaIcon = Content.Load<Texture2D>(@"SpriteSheets/NinjaIcon");

            _soldierSpriteSheet = Content.Load<Texture2D>(@"SpriteSheets/SoldierSpriteSheet2");
            _soldierBulletSprite = Content.Load<Texture2D>(@"SpriteSheets/SoldierBulletSprite");
            _soldierIcon = Content.Load<Texture2D>(@"SpriteSheets/SoldierIcon");

            _heavySpriteSheet1 = Content.Load<Texture2D>(@"SpriteSheets/HeavySpriteSheet2");
            _heavyIcon = Content.Load<Texture2D>(@"SpriteSheets/HeavyIcon");

            _zombieSheet = Content.Load<Texture2D>(@"SpriteSheets/ApocZombieSpriteSheet");

            _dessertMap = Content.Load<Texture2D>(@"SpriteSheets/Dessert1");

            _waveCounterSpriteSheet = Content.Load<Texture2D>(@"SpriteSheets/WaveCounterSprite");

            _heartSpriteSheet = Content.Load<Texture2D>(@"SpriteSheets/Heart");

            _projectileSpriteSheet = Content.Load<Texture2D>(@"SpriteSheets/captainProjectile");



            _fireworkTexture= Content.Load<Texture2D>(@"SpriteSheets/Particle2");
            _particleTexture= Content.Load<Texture2D>(@"SpriteSheets/Particle1");

            _gameOverTexture = Content.Load<Texture2D>(@"Controls/GameOver");

            foreach (var b in _menuComponents)
            {
                _entityManager.AddEntity(b);
            }



        }

        protected override void Update(GameTime gameTime)
        {
            if (_gameState == _GameState.PLAYING)
            {
                if (_waveManager.GameWon)
                {
                    _gameState = _GameState.GAMEWON;
                    _entityManager.Clear();
                    _waveManager.IsRunning = false;

                    
                }
                else
                {
                    _waveManager.Update(gameTime);

                    _entityManager.Update(gameTime);

                    foreach (Projectile b in _player1.Ranged.BulletsToAdd)
                    {
                        _entityManager.AddEntity(b);
                        _collisionManager.AddCollidable(b);
                    }
                    _player1.Ranged.BulletsToAdd.Clear();
                    foreach (Projectile b in _player1.Ranged.BulletsToRemove)
                    {
                        _entityManager.RemoveEntity(b);
                        _collisionManager.RemoveCollidable(b);
                    }
                    _player1.Ranged.BulletsToAdd.Clear();
                    _player1.Ranged.BulletsToRemove.Clear();

                    base.Update(gameTime);


                    _collisionManager.Update(gameTime);
                    if (_saveSlots[_currentSaveSlotIndex].CurrentWave != _waveManager.CurrentWave)
                    {
                        _saveSlots[_currentSaveSlotIndex].CurrentWave = _waveManager.CurrentWave + 1;
                        _saveSlots[_currentSaveSlotIndex].OverrideSave();
                    }
                    if (_player1.Health <= 0)
                    {
                        _gameState = _GameState.GAMEOVER;
                    }
                }

            }
            else if (_gameState == _GameState.INITIALISE)
            {
                _entityManager.Update(gameTime);
                _collisionManager.Update(gameTime);
                _gameState = _GameState.PLAYING;
                _waveManager = new WaveManager(_zombieSheet, _entityManager, _collisionManager, _waveCounterSpriteSheet, _projectileSpriteSheet);
                _waveManager.CurrentWave = _waveStartIndex;
                _waveManager.Intialise();



            }
            else if (_gameState == _GameState.GAMEOVER)
            {
                _entityManager.Clear();
                _collisionManager.Collidables.Clear();

                Button gameOver = new Button(_gameOverTexture, Content.Load<SpriteFont>(@"Fonts/ButtonFont"))
                {
                    Position = new Vector2(Window.ClientBounds.Width / 2 - _gameOverTexture.Width/2, Window.ClientBounds.Height / 2- _gameOverTexture.Height/2),
                };
                _entityManager.AddEntity(gameOver);
                _entityManager.Update(gameTime);

                _gameState = _GameState.SAVENEW;
                
            }
            else if(_gameState == _GameState.KEYCHANGE)
            {
                KeyboardState now = Keyboard.GetState();
                if (now.GetPressedKeys().Length > 0)
                {
                    ChangeKey(now.GetPressedKeys()[0]);
                }
            }
            else if (_gameState == _GameState.GAMEWON)
            {
                Button GameWon = new Button(Content.Load<Texture2D>(@"Controls/ButtonTexture"), Content.Load<SpriteFont>(@"Fonts/ButtonFont"))
                {
                    Text = "Congratulations",
                    Position = (Window.ClientBounds.Center.ToVector2())


                };
                _entityManager.AddEntity(GameWon);

                TitleButton_Click(GameWon);
                TitleButton_Click(GameWon);
                TitleButton_Click(GameWon);
                TitleButton_Click(GameWon);
                TitleButton_Click(GameWon);
                TitleButton_Click(GameWon);
                TitleButton_Click(GameWon);
                TitleButton_Click(GameWon);
                TitleButton_Click(GameWon);
                TitleButton_Click(GameWon);
                TitleButton_Click(GameWon);
                TitleButton_Click(GameWon);
                TitleButton_Click(GameWon);
                TitleButton_Click(GameWon);
                TitleButton_Click(GameWon);
                TitleButton_Click(GameWon);

                _entityManager.Update(gameTime);
                _entityManager.RemoveEntity(GameWon);
            }
            else
            {
                _entityManager.Update(gameTime);
            }
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) && ((_gameState == _GameState.SAVELOAD) || ((_gameState == _GameState.SAVENEW) || (_gameState == _GameState.OPTIONS))))
            {
                _entityManager.Clear();
                DisplayTitleScreen();
                foreach (var m in _menuComponents)
                {
                    _entityManager.AddEntity(m);
                }
               
            }
             
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
            else if ((_gameState == _GameState.SAVELOAD) || (_gameState == _GameState.SAVENEW))
            {
                GraphicsDevice.Clear(Color.Black);
                _entityManager.Draw(_spriteBatch, gameTime);
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



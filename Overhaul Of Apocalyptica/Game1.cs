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
using Overhaul_Of_Apocalyptica.Entities.Projectiles;
using System.Security.Cryptography.Xml;
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
        private WaveManager _waveManager;
        private CollisionManager _collisionManager;
        private enum _GameState { PLAYING, MENU, INITIALISE, SAVELOAD , SAVENEW };
        private _GameState _gameState;

        private List<IEntity> _menuComponents;

        private List<SaveSlot> _saveSlots;
        private List<Button> _saveButtons;
        private int _currentSaveSlotIndex;

        private int _waveStartIndex = 0;

        private Player _player1;

        private List<Texture2D> _ninjaAnimationTextures = new List<Texture2D>();
        private List<Texture2D> _soldierAnimationTextures = new List<Texture2D>();
        private List<Texture2D> _heavyAnimationTextures = new List<Texture2D>();
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

        private void TitleButton_Click(Button sender)
        {

            Firework f =  new Firework(_fireworkTexture, new Vector2(0, 0.2f), _particleTexture);

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
             
        }
        private void QuitButton_Click(Button sender)
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
            else if (_gameState ==_GameState.SAVELOAD)
            {
                _gameState = _GameState.INITIALISE;
                _currentSaveSlotIndex = _saveButtons.IndexOf(sender);
                switch (_saveSlots[_currentSaveSlotIndex].PlayerClass)
                {
                    case "Soldier":
                        Player soldier = new Soldier(_soldierSpriteSheet, _heartSpriteSheet, _soldierBulletSprite, gameTime);
                        _player1 = soldier;
                        break;
                    case "Ninja":
                        Player ninja = new Ninja(_ninjaSpriteSheet, _heartSpriteSheet,_shurikenSprite,gameTime);
                        _player1 = ninja;
                        break;
                    case "Heavy":
                        Player heavy = new Heavy(_heavySpriteSheet1, _heartSpriteSheet,_soldierBulletSprite,gameTime);
                        _player1 = heavy;
                        break;
                    default:
                        _player1 = new Soldier(_soldierSpriteSheet, _heartSpriteSheet, _soldierBulletSprite, gameTime);
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
                    Player soldier = new Soldier(_soldierSpriteSheet, _heartSpriteSheet, _soldierBulletSprite, gameTime);
                    _player1 = soldier;
                    _saveSlots[_currentSaveSlotIndex].Status = "Used";
                    _saveSlots[_currentSaveSlotIndex].PlayerName = "Player1";
                    _saveSlots[_currentSaveSlotIndex].PlayerClass = "Soldier";
                    _saveSlots[_currentSaveSlotIndex].CurrentWave = 1;
                    _saveSlots[_currentSaveSlotIndex].OverrideSave();

                    break;
                case "Ninja":
                    Player ninja = new Ninja(_ninjaSpriteSheet, _heartSpriteSheet,_shurikenSprite,gameTime);
                    _player1 = ninja;
                    _saveSlots[_currentSaveSlotIndex].Status = "Used";
                    _saveSlots[_currentSaveSlotIndex].PlayerName = "Player1";
                    _saveSlots[_currentSaveSlotIndex].PlayerClass = "Ninja";
                    _saveSlots[_currentSaveSlotIndex].CurrentWave = 1;
                    _saveSlots[_currentSaveSlotIndex].OverrideSave();

                    break;
                case "Heavy":
                    Player heavy = new Heavy(_heavySpriteSheet1, _heartSpriteSheet, _soldierBulletSprite, gameTime);
                    _player1 = heavy;
                    _saveSlots[_currentSaveSlotIndex].Status = "Used";
                    _saveSlots[_currentSaveSlotIndex].PlayerName = "Player1";
                    _saveSlots[_currentSaveSlotIndex].PlayerClass = "Heavy";
                    _saveSlots[_currentSaveSlotIndex].CurrentWave = 1;
                    _saveSlots[_currentSaveSlotIndex].OverrideSave();
                    break;
                default:
                    _player1 = new Soldier(_soldierSpriteSheet, _heartSpriteSheet, _soldierBulletSprite, gameTime);
                    break;
            }
            _entityManager.AddEntity(_player1);
            _collisionManager.AddCollidable(_player1);
            _player1.Activate();
            _entityManager.Clear();
            _gameState = _GameState.INITIALISE;
            
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            _ninjaSpriteSheet = Content.Load<Texture2D>(@"SpriteSheets/NinjaSpriteSheet");
            _shurikenSprite = Content.Load<Texture2D>(@"SpriteSheets/Shuriken");
            _ninjaIcon = Content.Load<Texture2D>(@"SpriteSheets/NinjaIcon");

            _soldierSpriteSheet = Content.Load<Texture2D>(@"SpriteSheets/SoldierSpriteSheet2");
            _soldierBulletSprite = Content.Load<Texture2D>(@"SpriteSheets/SoldierBulletSprite");
            _soldierIcon = Content.Load<Texture2D>(@"SpriteSheets/SoldierIcon");

            _heavySpriteSheet1 = Content.Load<Texture2D>(@"SpriteSheets/HeavySpriteSheet1");
            _heavyIcon = Content.Load<Texture2D>(@"SpriteSheets/HeavyIcon");

            _zombieSheet = Content.Load<Texture2D>(@"SpriteSheets/ApocZombieSpriteSheet");

            _dessertMap = Content.Load<Texture2D>(@"SpriteSheets/Dessert1");

            _waveCounterSpriteSheet = Content.Load<Texture2D>(@"SpriteSheets/WaveCounterSprite");

            _heartSpriteSheet = Content.Load<Texture2D>(@"SpriteSheets/Heart");

            _projectileSpriteSheet = Content.Load<Texture2D>(@"SpriteSheets/captainProjectile");



            _fireworkTexture= Content.Load<Texture2D>(@"SpriteSheets/Particle2");
            _particleTexture= Content.Load<Texture2D>(@"SpriteSheets/Particle1");

            _soldierAnimationTextures.Add(Content.Load<Texture2D>(@"SpriteSheets/SoldierAnimations/SoldierLeftAnimation"));
            _soldierAnimationTextures.Add(Content.Load<Texture2D>(@"SpriteSheets/SoldierAnimations/SoldierRightAnimation"));
            _soldierAnimationTextures.Add(Content.Load<Texture2D>(@"SpriteSheets/SoldierAnimations/SoldierUpAnimation"));
            _soldierAnimationTextures.Add(Content.Load<Texture2D>(@"SpriteSheets/SoldierAnimations/SoldierDownAnimation"));

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

                _entityManager.Update(gameTime);
                _collisionManager.Update(gameTime);
                if (_saveSlots[_currentSaveSlotIndex].CurrentWave != _waveManager.CurrentWave)
                {
                    _saveSlots[_currentSaveSlotIndex].CurrentWave = _waveManager.CurrentWave+1;
                    _saveSlots[_currentSaveSlotIndex].OverrideSave();
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
            else
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
            else if ((_gameState == _GameState.SAVELOAD) || (_gameState == _GameState.SAVENEW))
            {
                GraphicsDevice.Clear(Color.SteelBlue);
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



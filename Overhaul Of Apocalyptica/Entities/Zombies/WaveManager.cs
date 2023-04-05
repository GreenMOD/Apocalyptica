using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Overhaul_Of_Apocalyptica.Entities;
using Overhaul_Of_Apocalyptica.Entities.Zombies;
using Overhaul_Of_Apocalyptica.Entities.Characters;

namespace Overhaul_Of_Apocalyptica
{
    class WaveManager : IEntity
    {
        #region Declarations

        private List<Zombie> _zombiesLeft = new List<Zombie>();// all zombies that havenet been spawned yet but are to be
        private List<Zombie> _zombiesToAdd = new List<Zombie>();// all zombies are to be spawned
        private List<Zombie> _zombiesToRemove = new List<Zombie>();


        private Texture2D _zombieSpriteSheet;
        private Texture2D _projectilesSpriteSheet;

        protected List<Rectangle> frames = new List<Rectangle>() { new Rectangle(2, 1, 5, 31), new Rectangle(8, 1, 14, 31), new Rectangle(23, 1, 18, 32), new Rectangle(42, 1, 24, 31), new Rectangle(68, 1, 19, 31) }; 

        private EntityManager _entityManager;
        private CollisionManager _collisionManager;

        private float _waveRestPeriod = 10f;
        private float _waveRestStart;

        private enum managerStates { NewWave, NextWave, WaveInProgress, RestPeriod, WavesWon }
        private managerStates _waveStates;


        private List<Player> _players;
        #endregion


        #region Properties

        public List<List<Zombie>> Waves = new List<List<Zombie>>();
        public int CurrentWave { get; set; }
        public List<Zombie> ZombiesSpawned { get; set; }//contains the zombies that are already in play

        public Sprite WaveCounter { get; set; }

        public bool IsRunning { get; set; }

        public bool GameWon
        {
            get
            {
                if (_waveStates == managerStates.WavesWon)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion


        #region Constructor

        public WaveManager(Texture2D ZombieSheet, EntityManager entityManager, CollisionManager collsionManager, Texture2D waveCounterSprite, Texture2D projectiles)
        {
            _entityManager = entityManager;
            WaveCounter = new Sprite(waveCounterSprite, frames, new Vector2(400, 100));
            _zombieSpriteSheet = ZombieSheet;
            _projectilesSpriteSheet = projectiles;
            IsRunning = false;

            _players = _entityManager.GetEntities<Player>();
            _collisionManager = collsionManager;
            ZombiesSpawned = new List<Zombie>();
            _waveStates = managerStates.WaveInProgress;

        }
        #endregion 


        #region New Wave and NextWave



        /// <summary>
        /// Incrments the wave counter and spawns 3 zombies from the next wave unless there is less than 3. 
        /// </summary>
        public void BeginWave()
        {
            if (Waves.Count - 1 < CurrentWave)
            {
                _waveStates = managerStates.WavesWon;
            }
            else
            {
                _zombiesLeft = Waves[CurrentWave];
                if (_zombiesLeft.Count < 3)
                {
                    SpawnZombie(1);
                }
                else
                {
                    SpawnZombie(3);
                }
                _waveStates = managerStates.WaveInProgress;
            }




        }
        #endregion


        #region IEntity Methods
        /// <summary>
        /// Uses its current wave state to determine how to respond. Will maintain all zombies while a wave is in progress 
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            switch (_waveStates)
            {
                case managerStates.NewWave:
                    BeginWave();
                    break;
                case managerStates.NextWave:
                    CurrentWave++;
                    BeginWave();
                    break;
                case managerStates.WaveInProgress:

                    foreach (Zombie z in _zombiesToAdd)
                    {
                        ZombiesSpawned.Add(z);
                        _entityManager.AddEntity(z);
                        _collisionManager.AddCollidable(z);
                    }

                    foreach (Zombie z in ZombiesSpawned)
                    {
                        if (z.Health <= 0)
                        {
                            _zombiesToRemove.Add(z);
                        }

                    }

                    foreach (Zombie z in _zombiesToRemove)
                    {
                        ZombiesSpawned.Remove(z);
                        _entityManager.RemoveEntity(z);
                        _collisionManager.RemoveCollidable(z);
                    }




                    _zombiesToAdd.Clear();
                    _zombiesToRemove.Clear();

                    if (ZombiesSpawned.Count == 0)
                    {
                        if (_zombiesLeft.Count == 0)
                        {
                            _waveStates = managerStates.RestPeriod;
                            _waveRestStart = (float)gameTime.TotalGameTime.TotalSeconds;

                        }
                        else if (_zombiesLeft.Count < 3)
                        {
                            SpawnZombie(_zombiesLeft.Count);
                        }
                        else
                        {
                            Random rng = new Random();
                            SpawnZombie(rng.Next(1, 3));
                        }

                    }

                    break;

                case managerStates.RestPeriod:
                    if (gameTime.TotalGameTime.TotalSeconds - _waveRestStart >= _waveRestPeriod)
                    {
                        _waveStates = managerStates.NextWave;
                    }
                    break;
            }


        }
        /// <summary>
        /// Draws  the wave counter to mark what wave the player is on.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            WaveCounter.Frame = CurrentWave;
            WaveCounter.Update(gameTime, new Vector2(400f, 50f));
            WaveCounter.Draw(spriteBatch, gameTime, 2.5f);

        }
        #endregion 


        #region Spawning
        /// <summary>
        /// A set amount of zombies (mob) are added to a list that will be spawned when the next Update is called
        /// </summary>
        /// <param name="numZombies"></param>
        public void SpawnZombie(int numZombies)
        {
            for (int i = 0; i < numZombies; i++)
            {
                _zombiesToAdd.Add(_zombiesLeft[i]);

            }
            _zombiesLeft.RemoveRange(0, numZombies);

        }
        #endregion


        #region Initialisation and Wave Generation
        /// <summary>
        /// Loads all prewritten waves into a list called waves and begins the game.
        /// </summary>
        public void Intialise()
        {
            Waves = new List<List<Zombie>>();
            Random rng = new Random();
            IsRunning = true;
            //Wave1
            List<Zombie> Wave1 = new List<Zombie>();
            for (int i = 0; i < 10; i++)
            {
                Zombie b = new Walker(_zombieSpriteSheet, _entityManager, new Vector2(rng.Next(0, 800), rng.Next(480, 700)), _players); 
                Wave1.Add(b);

            }
            //Wave2
            List<Zombie> Wave2 = new List<Zombie>();
            for (int i = 0; i < 2; i++)
            {
                Zombie b = new Captain(_zombieSpriteSheet, new Vector2(rng.Next(0, 800), rng.Next(480, 700)), _entityManager, _collisionManager, _projectilesSpriteSheet);
                Wave2.Add(b);
            }
            for (int i = 0; i < 10; i++)
            {
                Zombie w2 = new Walker(_zombieSpriteSheet, _entityManager, new Vector2(rng.Next(0, 800), rng.Next(480, 700)), _players);
                Wave2.Add(w2);
            }
            //Wave3
            List<Zombie> Wave3 = new List<Zombie>();
            for (int i = 0; i < 2; i++)
            {
                Wave3.Add(new Captain(_zombieSpriteSheet, new Vector2(rng.Next(0, 800), rng.Next(480, 700)), _entityManager, _collisionManager, _projectilesSpriteSheet));
                for (int j = 0; j < 3; j++)
                {
                    Wave3.Add(new Screamer(_zombieSpriteSheet, new Vector2(rng.Next(0, 800), rng.Next(480, 700)), _entityManager));
                }
            }
            //Wave4
            List<Zombie> Wave4 = new List<Zombie>();
            for (int i = 0; i < 2; i++)
            {
                Wave4.Add(new Captain(_zombieSpriteSheet, new Vector2(rng.Next(0, 800), rng.Next(480, 700)), _entityManager, _collisionManager, _projectilesSpriteSheet));
                for (int j = 0; j < 3; j++)
                {
                    Wave4.Add(new Screamer(_zombieSpriteSheet, new Vector2(rng.Next(0, 800), rng.Next(480, 700)), _entityManager));
                    Wave4.Add(new Walker(_zombieSpriteSheet, _entityManager, new Vector2(rng.Next(0, 800), rng.Next(480, 700)), _players));
                }
            }
            List<Zombie> Wave5 = new List<Zombie>();
            Zombie z = new Walker(_zombieSpriteSheet, _entityManager, new Vector2(rng.Next(0, 800), rng.Next(480, 700)), _players);
            z.Health = 200;
            z.MaxVelocity = 0.5f;
            z.MaxForce = 0.5f;
            Wave5.Add(z);
            //Gathering all waves



            Waves.Add(Wave1);
            Waves.Add(Wave2);
            Waves.Add(Wave3);
            Waves.Add(Wave4);
            Waves.Add(Wave5);

            _waveStates = managerStates.NewWave;
        }
        #endregion




    }
}
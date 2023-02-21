using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Overhaul_Of_Apocalyptica.Entities;
using Overhaul_Of_Apocalyptica.Entities.Zombies;
using Overhaul_Of_Apocalyptica.Entities.Characters;
using System.Security.AccessControl;
using System.Net.Http.Headers;

namespace Overhaul_Of_Apocalyptica
{
    class WaveManager : IEntity
    {
        public List<List<Zombie>> Waves = new List<List<Zombie>>();
        private Random _rng = new Random();
        public int CurrentWave { get; set; }
        public List<Zombie> ZombiesSpawned { get; set; }//contains the zombies that are already in play
        private List<Zombie> _zombiesLeft = new List<Zombie>();// all zombies that havenet been spawned yet but are to be
        private List<Zombie> _zombiesToAdd = new List<Zombie>();// all zombies are to be spawned
        private List<Zombie> _zombiesToRemove = new List<Zombie>(); 

        Texture2D _zombieSpriteSheet;
        Texture2D _projectilesSpriteSheet;

        public Sprite WaveCounter { get; set; }
        List<Rectangle> frames = new List<Rectangle>() {new Rectangle(2,1,5,31), new Rectangle(8,1,14,31), new Rectangle(23, 1, 18, 32), new Rectangle(42, 1, 24, 31), new Rectangle(68, 1, 19, 31)}; //TODO THIS ONLY GOES UP TO 5

        private EntityManager _entityManager;
        private CollisionManager _collisionManager;

        private List<Player> _players;
        public bool IsRunning { get; set; }

        private float _waveRestPeriod = 10f;
        private float _waveRestStart;

        private enum managerStates {NewWave,NextWave,WaveInProgress, RestPeriod}
        private managerStates _waveStates;


        public WaveManager(Texture2D ZombieSheet,EntityManager entityManager, CollisionManager collsionManager, Texture2D waveCounterSprite,Texture2D projectiles)
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
            
        }/// <summary>
        /// Incrments the wave counter and spawns 5 zombies from the next wave
        /// </summary>
        public void BeginWave()
        {
            _zombiesLeft = Waves[CurrentWave];
            SpawnZombie(3);
            _waveStates = managerStates.WaveInProgress;

        }
       
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
                        if (_zombiesLeft.Count ==0)
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
                            SpawnZombie(_rng.Next(1,3));
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
        /// Compiles all zombies that have been spawned in and draws them
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch,GameTime gameTime)
        {
            
            WaveCounter.Frame = CurrentWave;
            WaveCounter.Update(gameTime, new Vector2(400f, 50f));
            WaveCounter.Draw(spriteBatch, gameTime,2.5f);
            
        }
        /// <summary>
        /// A set amount of zombies are added to a list that will be spawned when the next Update is called
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
        /// <summary>
        /// Randomly generates number of zombies in waves using set parameters for each wave. isRunning becomes true. This is done to prevent exceptions caused by monogame's loop occuring
        /// </summary>
        public void Intialise()
        {
            IsRunning = true;
            //Wave1 Load
            List<Zombie> Wave1 = new List<Zombie>();
            for (int i = 0; i < 10; i++)
            {
                Zombie b = new Walker(_zombieSpriteSheet, _entityManager, new Vector2(_rng.Next(0, 800), _rng.Next(480, 700)),_players); //TODO make it so it only updates zombies that are in there
                Wave1.Add(b);
              
            }
            //Wave2
            List<Zombie> Wave2 = new List<Zombie>();
            for (int i = 0; i < 2; i++)
            {
                Zombie b = new Captain(_zombieSpriteSheet, new Vector2(_rng.Next(0, 800), _rng.Next(480, 700)), _entityManager,_collisionManager, _projectilesSpriteSheet);
                Wave2.Add(b);
            }
            for (int i = 0; i < 10; i++)
            {
                Zombie w2 = new Walker(_zombieSpriteSheet, _entityManager, new Vector2(_rng.Next(0, 800), _rng.Next(480, 700)), _players);
                Wave2.Add(w2);
            }
            //Wave3
            List<Zombie> Wave3 = new List<Zombie>();
            for (int i = 0; i < 2; i++)
            {
              Wave3.Add(new Captain(_zombieSpriteSheet, new Vector2(_rng.Next(0, 800), _rng.Next(480, 700)), _entityManager, _collisionManager, _projectilesSpriteSheet));
                for (int j = 0; j < 3; j++)
                {
                    Wave3.Add(new Screamer(_zombieSpriteSheet, new Vector2(_rng.Next(0, 800), _rng.Next(480, 700)), _entityManager));
                }
            }
            List<Zombie> Wave4 = new List<Zombie>();
            for (int i = 0; i < 2; i++)
            {
                Wave3.Add(new Captain(_zombieSpriteSheet, new Vector2(_rng.Next(0, 800), _rng.Next(480, 700)), _entityManager, _collisionManager, _projectilesSpriteSheet));
                for (int j = 0; j < 3; j++)
                {
                    Wave4.Add(new Screamer(_zombieSpriteSheet, new Vector2(_rng.Next(0, 800), _rng.Next(480, 700)), _entityManager));
                    Wave4.Add(new Walker(_zombieSpriteSheet, _entityManager, new Vector2(_rng.Next(0, 800), _rng.Next(480, 700)), _players));
                }
            }
            List<Zombie> Wave5 = new List<Zombie>();
            Zombie z = new Walker(_zombieSpriteSheet, _entityManager, new Vector2(_rng.Next(0, 800), _rng.Next(480, 700)), _players);
            z.Health = 200;
            z.MaxVelocity = 0.2f;
            z.MaxForce = 0.2f;
            Wave5.Add(z);
            //etc.

            //Gathering all waves



            Waves.Add(Wave1);
            Waves.Add(Wave2);
            Waves.Add(Wave3);
            Waves.Add(Wave4);
            Waves.Add(Wave5);

            _waveStates = managerStates.NewWave;
        }
       




    }
}
//Spare code i foun in update
////foreach (Zombie z in zombiesSpawned)
//{
//    List<Zombie> zombiesCheckCollsion = zombiesSpawned;
//    zombiesCheckCollsion.Remove(z);
//    for (int i = 0; i < index - 1; i++)
//    {
//        if (zombiesCheckCollsion[i].CollisionBox.Intersects(z.CollisionBox))
//        {
//            z.Separate(zombiesCheckCollsion[i].Position); 
//        }
//    }
//}

//if (zombiesLeft.Count != 0)//& (_numZombiesSpawned != 0) )
//{
//    float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
//    if ((gameTime.TotalGameTime.TotalSeconds - lastSpawned) >= spawnBuffer)
//    {


//        SpawnZombie(1);
//        lastSpawned = gameTime.TotalGameTime.TotalSeconds;
//    }
//}
//Separate(zombiesSpawned);

